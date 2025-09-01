using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using VendorPortal.Application.Helpers;
using VendorPortal.Application.Interfaces.SyncExternalData;
using VendorPortal.Application.Models.Common;
using VendorPortal.Application.Models.ExtenalModel;
using VendorPortal.Application.Models.v1.Response;
using VendorPortal.Domain.Interfaces.v1;
using VendorPortal.Domain.Models.WolfApprove.StoreModel;
using VendorPortal.Domain.Models.WolfApprove.StoreModel.TempDefinedTable;
using VendorPortal.Infrastructure.Extensions;
using VendorPortal.Logging;
using static VendorPortal.Application.Models.Common.AppEnum;

namespace VendorPortal.Application.Services.SyncExternalData
{
    public class KubbossService : IKubbossService
    {
        private readonly DbContext _dbContext;
        private readonly AppConfigHelper _appConfigHelper;
        private readonly IWolfApproveRepository _wolfApproveRepository;
        public KubbossService(DbContext dbContext, AppConfigHelper appConfigHelper, IWolfApproveRepository wolfApproveRepository)
        {
            _wolfApproveRepository = wolfApproveRepository;
            _appConfigHelper = appConfigHelper;
            _dbContext = dbContext;
        }

        public async Task<QuotationResponse> SyncQuotationFromKubboss(string supplierId, string rfqId)
        {
            var response = new QuotationResponse();
            try
            {
                var verify = await _wolfApproveRepository.SP_GET_RFQ_DETAIL(rfqId);
                if (verify == null || !verify.Any())
                {
                    response = new QuotationResponse()
                    {
                        status = new Status()
                        {
                            code = ResponseCode.NotFound.Text(),
                            message = ResponseCode.NotFound.Description()
                        }
                    };
                    return response;
                }
                else
                {
                    var sqlParameter1 = new SqlParameter[]
                    {
                        new SqlParameter("@nRFQID", rfqId),
                    };
                    var quoData = await _dbContext.ExcuteStoreQueryListAsync<SP_GET_QUOTATION_ID_BY_RFQID>("SP_GET_QUOTATION_ID_BY_RFQID", sqlParameter1);
                    if (quoData == null || quoData.Count == 0)
                    {
                        response = new QuotationResponse()
                        {
                            status = new Status()
                            {
                                code = ResponseCode.NotFound.Text(),
                                message = "Quotation not found for the given RFQ ID."
                            }
                        };
                        return response;
                    }
                    else
                    {
                        var sqlParameter = new SqlParameter[]
                        {
                            new SqlParameter("@sChannel", _appConfigHelper.GetConfiguration("KubbossChannel").ToString()),
                        };
                        var configToken = await _dbContext.ExcuteStoreQuerySingleAsync<SP_GET_SYSENDPOINT>("SP_GET_SYSENDPOINT", sqlParameter);
                        var endPoint = _appConfigHelper.GetConfiguration("EndPoint:Kubboss").ToString();
                        HttpClient client = new HttpClient();
                        client.BaseAddress = new Uri(endPoint);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {configToken.sToken}");
                        var data = new List<QuotationData>();
                        foreach (var item in quoData)
                        {
                            var result = await client.GetAsync($"api/quotations/{item.nQuotationID}");
                            if (result.IsSuccessStatusCode)
                            {
                                var quo_content = await result.Content.ReadAsStringAsync();
                                var quotationResponse = JsonConvert.DeserializeObject<SyncQuotationResponse>(quo_content);
                                if (quotationResponse.status.code == ResponseCode.Success.Text() && quotationResponse.data != null)
                                {
                                    var _net_amount = quotationResponse.data.net_amount?.Replace(",", "") ?? "0";
                                    var _discount = quotationResponse.data.discount.Replace(",", "");
                                    var _sub_total = quotationResponse.data.sub_total.Replace(",", "");
                                    var _total_amount = quotationResponse.data.total_amount.Replace(",", "");
                                    var _vat_amount = quotationResponse.data.vat_amount.Replace(",", "");

                                    data.Add(new QuotationData()
                                    {
                                        id = quotationResponse.data.id,
                                        quotation_number = quotationResponse.data.quotation_number,
                                        rfq_number = quotationResponse.data.rfq_number,
                                        supplier_id = quotationResponse.data.supplier_id,
                                        company_id = quotationResponse.data.company_id,
                                        status = quotationResponse.data.status,
                                        transfer_date = quotationResponse.data.transfer_date,
                                        net_amount = _net_amount != "0" ? Decimal.Parse(_net_amount) : 0.00m,
                                        discount = _discount != "0" ? Decimal.Parse(_discount) : 0.00m,
                                        sub_total = _sub_total != "0" ? Decimal.Parse(_sub_total) : 0.00m,
                                        total_amount =  _total_amount != "0" ? Decimal.Parse(_total_amount) : 0.00m,
                                        vat_amount = _vat_amount != "0" ? Decimal.Parse(_vat_amount) : 0.00m,
                                        vat_rate = quotationResponse.data.vat_rate,
                                        supplier = new SupplierData
                                        {
                                            id = quotationResponse.data.supplier.id,
                                            name = quotationResponse.data.supplier.name,
                                            tax_id = quotationResponse.data.supplier.tax_id
                                        },
                                        created_at = quotationResponse.data.created_at,
                                        updated_at = quotationResponse.data.updated_at,
                                        lines = quotationResponse.data.lines.Select(s => new QuotationLineData
                                        {
                                            id = s.id,
                                            quantity = s.quantity,
                                            rfq_description = s.rfq_description,
                                            rfq_item_code = s.rfq_item_code,
                                            rfq_item_name = s.rfq_item_name,
                                            rfq_line_number = s.rfq_line_number,
                                            rfq_uom_name = s.rfq_uom_name,
                                            unit_price = s.unit_price
                                        }).ToList(),
                                        documents = quotationResponse.data.documents.Select(s=> new QuotationDocumentData
                                        {
                                            uuid = s.uuid,
                                            file_name = s.file_name,
                                            file_url = s.file_url,
                                        }).ToList(),
                                        questions = quotationResponse.data.questions.Select(s=> new QuotationQuestionData
                                        {
                                            id = s.id,
                                            answer = s.answer,
                                            created_at = s.created_at,
                                            description = s.description,
                                            question = s.question,
                                            question_id = s.question_id,
                                            question_number = s.question_number
                                        }).ToList(),
                                        address = new QuotationAddressData()
                                        {
                                            address_1 = quotationResponse.data.address?.address_1,
                                            address_2 = quotationResponse.data.address?.address_2,
                                            district_name = quotationResponse.data.address?.district_name,
                                            name = quotationResponse.data.address?.name,
                                            postal_code = quotationResponse.data.address?.postal_code,
                                            province_name = quotationResponse.data.address?.province_name,
                                            sub_district_name = quotationResponse.data.address?.sub_district_name
                                        }
                                    });
                                }
                                else
                                {
                                    Logger.LogError(new Exception($"Failed to sync quotation from Kubboss. Status code: {result.StatusCode} , Data {JsonConvert.SerializeObject(quotationResponse.data)}"), "SyncQuotationFromKubboss");
                                }
                            }
                            else
                            {
                                Logger.LogError(new Exception($"Failed to sync quotation from Kubboss. Status Header : {result.IsSuccessStatusCode}"), "SyncQuotationFromKubboss");
                            }
                        }
                        if (data.Count > 0)
                        {

                            response.data = data;
                            response.status = new Status()
                            {
                                code = ResponseCode.Success.Text(),
                                message = ResponseCode.Success.Description()
                            };
                        }
                        else
                        {
                            response.data = null;
                            response.status = new Status()
                            {
                                code = ResponseCode.NotFound.Text(),
                                message = ResponseCode.NotFound.Description()
                            };
                        }
                        return response;
                    }
                }

            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "SyncQuotationFromKubboss");
            }
            return response;
        }

        public async Task<BaseResponse> SyncVendorFromKubboss(DateTime dateTime)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                var sqlParameter = new SqlParameter[]
                {
                    new SqlParameter("@sChannel", _appConfigHelper.GetConfiguration("KubbossChannel").ToString()),
                };

                var spResult = await _dbContext.ExcuteStoreQuerySingleAsync<SP_GET_SYSENDPOINT>("SP_GET_SYSENDPOINT", sqlParameter);
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(spResult.sEndPoint);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {spResult.sToken}");

                var jData = JsonConvert.SerializeObject(new { last_updated = dateTime.ToString("yyyy-MM-ddTHH:mm:ss") });
                StringContent content = new StringContent(jData, Encoding.UTF8, "application/json");

                var result = await client.PostAsync("", content);
                if (result.IsSuccessStatusCode)
                {
                    var responseContent = await result.Content.ReadAsStringAsync();
                    // replace empty array with null to avoid deserialization error
                    var utf8EncodedContent = Encoding.UTF8.GetString(Encoding.Default.GetBytes(responseContent.Replace("[]", "null")));
                    var syncVendorResponse = JsonConvert.DeserializeObject<SyncVendorResponse>(utf8EncodedContent);
                    if (syncVendorResponse.data.Count > 0)
                    {
                        foreach (var vendor in syncVendorResponse.data)
                        {
                            var sqlParameterVendor = new SqlParameter[]
                            {
                                new SqlParameter("@nVendorID", vendor.id),
                                new SqlParameter("@sVendorName", vendor.name),
                                new SqlParameter("@sVendorTaxID", vendor.tax_id),
                                new SqlParameter("@sVendorType", vendor.type),
                                new SqlParameter("@sVendorLogoUrl", vendor.logo_url),
                                new SqlParameter("@sJuristicalPersonCertificate", vendor.juristical_person_certificate),
                                new SqlParameter("@FinancialDocument", vendor.financial_document),
                                new SqlParameter("@IsJuristicalPerson", vendor.is_juristical_person ? 1:0),
                                new SqlParameter("@IsVerifyComplete", vendor.is_verify_complete ? 1 : 0),
                                new SqlParameter("@sKeyContactName", vendor.key_contact?.name),
                                new SqlParameter("@sKeyContactSurname", vendor.key_contact?.surname),
                                new SqlParameter("@sKeyContactEmail", vendor.key_contact?.email),
                                new SqlParameter("@sKeyContactPhone", vendor.key_contact?.phone),
                                new SqlParameter("@sKeyContactPosition", vendor.key_contact?.position),
                            };
                            var result_insert = await _dbContext.ExecuteStoreNonQueryAsync("SP_INSERT_VENDOR_FROM_KUBBOSS", sqlParameterVendor);



                            // other documents
                            if (vendor.other_documents != null && vendor.other_documents.Count > 0)
                            {
                                var other_doc = new List<KB_TEMP_OTHERDOCUMENT>();
                                foreach (var other in vendor.other_documents)
                                {
                                    var otherDocument = new KB_TEMP_OTHERDOCUMENT()
                                    {
                                        vendor_id = int.Parse(vendor.id),
                                        sFile_name = other.file_name,
                                        sFile_url = other.file_url,
                                    };
                                    other_doc.Add(otherDocument);
                                }
                                var sqlParameterOtherDocument = new SqlParameter[]
                                {
                                    new SqlParameter("@TempOtherDocument", other_doc),
                                };
                                await _dbContext.ExecuteStoreNonQueryAsync("SP_INSERT_VENDOR_OTHER_DOCUMENTS_FROM_KUBBOSS", sqlParameterOtherDocument);
                            }

                            // addresses
                            if (vendor.addresses != null && vendor.addresses.Count > 0)
                            {
                                var tempAddress = new List<KB_TEMP_ADDRESSES>();
                                foreach (var address in vendor.addresses)
                                {
                                    var tempAddressModel = new KB_TEMP_ADDRESSES()
                                    {
                                        sAddress_1 = address.address_1,
                                        sAddress_2 = address.address_2,
                                        sProvince_name = address.province_name,
                                        sDistrict_name = address.district_name,
                                        sSub_district_name = address.sub_district_name,
                                        sPostal_code = address.postal_code,
                                    };
                                    tempAddress.Add(tempAddressModel);
                                }
                                var sqlParameterAddress = new SqlParameter[]
                                {
                                    new SqlParameter("@TempAddress", tempAddress)
                                };
                                await _dbContext.ExecuteStoreNonQueryAsync("SP_INSERT_VENDOR_ADDRESS_FROM_KUBBOSS", sqlParameterAddress);
                            }


                        }
                    }
                    else
                    {

                    }
                    response = new BaseResponse()
                    {
                        status = new Status()
                        {
                            code = ResponseCode.Success.Text(),
                            message = ResponseCode.Success.Description()
                        }
                    };
                }
                else
                {
                    response = new BaseResponse()
                    {
                        status = new Status()
                        {
                            code = result.StatusCode.ToString(),
                            message = "Failed to sync vendor from Kubboss."
                        }
                    };
                    Logger.LogError(new Exception($"Failed to sync vendor from Kubboss. Status code: {result.StatusCode}"), "SyncVendorFromKubboss");
                }
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "SyncVendorFromKubboss");
            }
            return response;
        }

    }
}