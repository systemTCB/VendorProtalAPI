using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using VendorPortal.Application.Helpers;
using VendorPortal.Application.Interfaces.SyncExternalData;
using VendorPortal.Application.Models.Common;
using VendorPortal.Application.Models.ExtenalModel;
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
        public KubbossService(DbContext dbContext, AppConfigHelper appConfigHelper)
        {
            _appConfigHelper = appConfigHelper;
            _dbContext = dbContext;
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
                            // ระบบจะไม่เก็บ quotations ที่ได้จาก kubboss ให้เก็บเฉพาะ quotations ที่ได้จาก vendor portal เท่านั้น


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