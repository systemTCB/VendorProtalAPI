using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using HandlebarsDotNet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VendorPortal.Application.Helpers;
using VendorPortal.Application.Interfaces.SyncExternalData;
using VendorPortal.Application.Models.Common;
using VendorPortal.Application.Models.ExtenalModel;
using VendorPortal.Application.Models.v1.Request;
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
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMasterDataRepository _masterDataRepository;
        public KubbossService(DbContext dbContext, AppConfigHelper appConfigHelper, IWolfApproveRepository wolfApproveRepository, IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, IMasterDataRepository masterDataRepository)
        {
            _wolfApproveRepository = wolfApproveRepository;
            _appConfigHelper = appConfigHelper;
            _dbContext = dbContext;
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _masterDataRepository = masterDataRepository;
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
                        decimal ParseDecimal(string value)
                        {
                            if (string.IsNullOrWhiteSpace(value))
                                return 0m;

                            decimal.TryParse(value.Replace(",", ""), out var result);
                            return result;
                        }

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
                            if (result != null && result.IsSuccessStatusCode)
                            {
                                var quo_content = await result.Content.ReadAsStringAsync();
                                if (quo_content.Contains("\"data\":[]"))
                                {
                                    Logger.LogError(new Exception($"Quotation not found. QuotationID = {item.nQuotationID}"), "SyncQuotationFromKubboss");
                                    continue;
                                }

                                var jObj = JObject.Parse(quo_content);

                                var statusCode = jObj["status"]?["code"]?.Value<int>();

                                if (statusCode == 404)
                                {
                                    Logger.LogError(
                                        new Exception($"Quotation not found. QuotationID = {item.nQuotationID}"),
                                        "SyncQuotationFromKubboss"
                                    );

                                    continue;
                                }

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

                                        //######################## ของเก่า #############################
                                        //net_amount = _net_amount != "0" ? Decimal.Parse(_net_amount) : 0.00m,
                                        //discount = _discount != "0" ? Decimal.Parse(_discount) : 0.00m,
                                        //sub_total = _sub_total != "0" ? Decimal.Parse(_sub_total) : 0.00m,
                                        //total_amount = _total_amount != "0" ? Decimal.Parse(_total_amount) : 0.00m,
                                        //vat_amount = _vat_amount != "0" ? Decimal.Parse(_vat_amount) : 0.00m,
                                        //######################## ของเก่า #############################

                                        //######################## ของใหม่ #############################
                                        net_amount = ParseDecimal(quotationResponse.data.net_amount),
                                        discount = ParseDecimal(quotationResponse.data.discount),
                                        sub_total = ParseDecimal(quotationResponse.data.sub_total),
                                        total_amount = ParseDecimal(quotationResponse.data.total_amount),
                                        vat_amount = ParseDecimal(quotationResponse.data.vat_amount).ToString(),
                                        //######################## ของใหม่ #############################

                                        vat_rate = quotationResponse.data.vat_rate,
                                        payment_condition = quotationResponse.data.payment_condition,
                                        remark = quotationResponse.data.remark,
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
                                        documents = quotationResponse.data.documents.Select(s => new QuotationDocumentData
                                        {
                                            uuid = s.uuid,
                                            file_name = s.file_name,
                                            file_url = s.file_url,
                                        }).ToList(),
                                        questions = quotationResponse.data.questions.Select(s => new QuotationQuestionData
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
                                            sub_district_name = quotationResponse.data.address?.sub_district_name,
                                            branch = quotationResponse.data.address?.branch,
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
                                Logger.LogError(new Exception($"Get quotation failed. QuotationID = {item.nQuotationID}"), "SyncQuotationFromKubboss");
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

        public async Task<ActionResultResponse> RegsiterSuppliersFromKubboss(string supplier_id, string buyerCode, string docNo)
        {
            await Logger.LogInfo($"START | supplier_id:{supplier_id} | buyerCode:{buyerCode} | docNo:{docNo}", "RegsiterSuppliersFromKubboss");

            try
            {
                var ctx = await ResolveClientSystemAsync(buyerCode);
                await Logger.LogInfo($"Resolved system | SystemCode:{ctx.SystemCode} | BaseUrl:{ctx.BaseUrl}", "RegsiterSuppliersFromKubboss");

                var questionnaires = await GetSupplierQuestionnaires(supplier_id, ctx.Client);
                await Logger.LogInfo($"Questionnaires fetched | count:{questionnaires?.Count ?? 0}", "RegsiterSuppliersFromKubboss");

                if (questionnaires == null || !questionnaires.Any())
                {
                    await Logger.LogInfo($"ABORT | Questionnaire not found | supplier_id:{supplier_id}", "RegsiterSuppliersFromKubboss");
                    return ActionResultResponse.Fail("Questionnaire not found");
                }

                var routes = await GetActiveBuyerRoute(buyerCode);
                await Logger.LogInfo($"Active buyer routes fetched | buyerCode:{buyerCode} | count:{routes?.Count ?? 0}", "RegsiterSuppliersFromKubboss");

                var buyerRoute = routes.FirstOrDefault(x => x.ActionType == "CREATE_REGISTER");
                await Logger.LogInfo($"Route match | ActionType:CREATE_REGISTER | Found:{buyerRoute != null}", "RegsiterSuppliersFromKubboss");

                if (buyerRoute == null)
                {
                    await Logger.LogInfo($"ABORT | Buyer route not found | buyerCode:{buyerCode}", "RegsiterSuppliersFromKubboss");
                    return ActionResultResponse.Fail("Buyer route not found");
                }

                var answerData = questionnaires.FirstOrDefault(x =>
                    string.Equals(
                        x["data"]?["company_questionnaire"]?["company"]?.ToString()?.Trim(),
                        buyerRoute.CompanyName?.Trim(),
                        StringComparison.OrdinalIgnoreCase
                    )
                );
                await Logger.LogInfo($"Answer match by CompanyName:'{buyerRoute.CompanyName}' | Found:{answerData != null}", "RegsiterSuppliersFromKubboss");

                if (answerData == null)
                {
                    await Logger.LogInfo($"WARNING | No matching questionnaire answer for CompanyName:'{buyerRoute.CompanyName}' -> sending empty payload", "RegsiterSuppliersFromKubboss");
                }

                var payloadObj = answerData ?? new JObject();
                var jObj = JObject.FromObject(payloadObj);
                jObj["data"]["docNo"] = docNo;
                var payloadJson = JsonConvert.SerializeObject(jObj);

                await Logger.LogInfo($"Payload built | Buyer:{buyerRoute.BuyerCode} | Payload:{payloadJson}", "RegsiterSuppliersFromKubboss");

                await Logger.LogInfo($"Calling SendToBuyer | Buyer:{buyerRoute.BuyerCode}", "RegsiterSuppliersFromKubboss");
                var sent = await SendToBuyer(buyerRoute, payloadJson);
                await Logger.LogInfo($"SendToBuyer returned | Buyer:{buyerRoute.BuyerCode} | Result:{sent}", "RegsiterSuppliersFromKubboss");

                if (!sent)
                {
                    await Logger.LogInfo($"WARNING | SendToBuyer returned false but flow reports Success anyway | Buyer:{buyerRoute.BuyerCode}", "RegsiterSuppliersFromKubboss");
                }

                return ActionResultResponse.Success("Send to buyer success");
            }
            catch (Exception ex)
            {
                await Logger.LogInfo($"EXCEPTION | supplier_id:{supplier_id} | buyerCode:{buyerCode} | {ex.GetType().Name}: {ex.Message}", "RegsiterSuppliersFromKubboss");
                Logger.LogError(ex, "RegsiterSuppliersFromKubboss");
                return ActionResultResponse.Fail("internal error");
            }
        }

        //public async Task<SupplierRegisterResponse> RegsiterSuppliersToKubboss(SupplierRegisterRequest request)
        //{
        //    SupplierRegisterResponse response = new SupplierRegisterResponse();
        //    DateTime createdDate = DateTime.Now;
        //    try
        //    {
        //        var host = _httpContextAccessor.HttpContext?.Request.Host.Host?.ToLower();

        //        var system = (await _wolfApproveRepository.SP_GET_Systems(host))?.FirstOrDefault();

        //        HttpClient client;

        //        if (system != null)
        //        {
        //            // ใช้ค่าจาก DB
        //            client = HttpClientHelper.CreateClient(system.BaseUrl, system.Token);
        //        }
        //        else
        //        {
        //            // Fallback ไปใช้ config เดิม
        //            var sqlParameter = new SqlParameter[]
        //            {
        //                new SqlParameter("@sChannel", _appConfigHelper.GetConfiguration("KubbossChannel"))
        //            };

        //            var configToken = await _dbContext.ExcuteStoreQuerySingleAsync<SP_GET_SYSENDPOINT>(
        //                "SP_GET_SYSENDPOINT",
        //                sqlParameter);

        //            var endPoint = _appConfigHelper.GetConfiguration("EndPoint:Kubboss");

        //            client = HttpClientHelper.CreateClient(endPoint, configToken?.sToken);
        //        }

        //        var json = JsonConvert.SerializeObject(request);

        //        var content = new StringContent(json, Encoding.UTF8, "application/json");

        //        var res = await client.PostAsync("/api/register-supplier-wolf", content);

        //        if (!res.IsSuccessStatusCode)
        //            throw new Exception("Failed to Regsiter Suppliers To Kubboss");

        //        var responseContent = await res.Content.ReadAsStringAsync();

        //        return JsonConvert.DeserializeObject<SupplierRegisterResponse>(responseContent);
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.LogError(ex, "Regsiter Suppliers To Kubboss");

        //        return new SupplierRegisterResponse
        //        {
        //            status = new Status
        //            {
        //                code = "500",
        //                message = " response failed"
        //            },
        //            data = null
        //        };

        //    }
        //}

        public async Task<SupplierRegisterResponse> RegsiterSuppliersToKubboss(SupplierRegisterRequest request)
        {
            SupplierRegisterResponse response = new SupplierRegisterResponse();
            DateTime createdDate = DateTime.Now;
            string logName = "RegisterSupplierKubboss";

            try
            {
                var clientSystem = _httpContextAccessor.HttpContext?.Request.Headers["X-Client-System"].ToString();
                Logger.LogInfo($"[ENTRY] X-Client-System header = '{clientSystem}'", logName);

                var system = (await _wolfApproveRepository.SP_GET_Systems(clientSystem))?.FirstOrDefault();

                HttpClient client;
                string usedBaseUrl;

                if (system != null)
                {
                    // ใช้ค่าจาก DB
                    await Logger.LogInfo($"Host '{clientSystem}' matched system in DB -> SystemCode: {system.SystemCode}, BaseUrl: {system.BaseUrl}",
                        logName);

                    client = HttpClientHelper.CreateClient(system.BaseUrl, system.Token);
                    usedBaseUrl = system.BaseUrl;
                }
                else
                {
                    // Fallback ไปใช้ config เดิม
                    var sqlParameter = new SqlParameter[]
                    {
                new SqlParameter("@sChannel", _appConfigHelper.GetConfiguration("KubbossChannel"))
                    };
                    var configToken = await _dbContext.ExcuteStoreQuerySingleAsync<SP_GET_SYSENDPOINT>(
                        "SP_GET_SYSENDPOINT",
                        sqlParameter);
                    var endPoint = _appConfigHelper.GetConfiguration("EndPoint:Kubboss");

                    await Logger.LogInfo($"Host '{clientSystem}' not found in DB -> fallback to config EndPoint:Kubboss = {endPoint}",
                        logName);

                    client = HttpClientHelper.CreateClient(endPoint, configToken?.sToken);
                    usedBaseUrl = endPoint;
                }

                var json = JsonConvert.SerializeObject(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                await Logger.LogInfo($"Sending request to {usedBaseUrl}/api/register-supplier-wolf", logName, json);

                var res = await client.PostAsync("/api/register-supplier-wolf", content);

                var responseContent = await res.Content.ReadAsStringAsync();

                await Logger.LogInfo($"Response from {usedBaseUrl} | StatusCode: {(int)res.StatusCode} {res.StatusCode}",
                    logName, responseContent);

                if (!res.IsSuccessStatusCode)
                    throw new Exception("Failed to Regsiter Suppliers To Kubboss");

                return JsonConvert.DeserializeObject<SupplierRegisterResponse>(responseContent);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Regsiter Suppliers To Kubboss");
                Logger.LogInfo($"Exception occurred: {ex.Message}", logName, ex.ToString());

                return new SupplierRegisterResponse
                {
                    status = new Status
                    {
                        code = "500",
                        message = " response failed"
                    },
                    data = null
                };
            }
        }

        public async Task<SupplierRegisterResponse> RegsiterSuppliersToKubbossTrial(SupplierRegisterRequestTrial request)
        {
            SupplierRegisterResponse response = new SupplierRegisterResponse();
            DateTime createdDate = DateTime.Now;
            string logName = "RegisterSupplierKubboss";

            try
            {
                var clientSystem = _httpContextAccessor.HttpContext?.Request.Headers["X-Client-System"].ToString();
                Logger.LogInfo($"[ENTRY] X-Client-System header = '{clientSystem}'", logName);

                var system = (await _wolfApproveRepository.SP_GET_Systems(clientSystem))?.FirstOrDefault();

                HttpClient client;
                string usedBaseUrl;

                if (system != null)
                {
                    // ใช้ค่าจาก DB
                    await Logger.LogInfo($"Host '{clientSystem}' matched system in DB -> SystemCode: {system.SystemCode}, BaseUrl: {system.BaseUrl}",
                        logName);

                    client = HttpClientHelper.CreateClient(system.BaseUrl, system.Token);
                    usedBaseUrl = system.BaseUrl;
                }
                else
                {
                    // Fallback ไปใช้ config เดิม
                    var sqlParameter = new SqlParameter[]
                    {
                new SqlParameter("@sChannel", _appConfigHelper.GetConfiguration("KubbossChannel"))
                    };
                    var configToken = await _dbContext.ExcuteStoreQuerySingleAsync<SP_GET_SYSENDPOINT>(
                        "SP_GET_SYSENDPOINT",
                        sqlParameter);
                    var endPoint = _appConfigHelper.GetConfiguration("EndPoint:Kubboss");

                    await Logger.LogInfo($"Host '{clientSystem}' not found in DB -> fallback to config EndPoint:Kubboss = {endPoint}",
                        logName);

                    client = HttpClientHelper.CreateClient(endPoint, configToken?.sToken);
                    usedBaseUrl = endPoint;
                }

                var json = JsonConvert.SerializeObject(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                await Logger.LogInfo($"Sending request to {usedBaseUrl}/api/register-supplier-trial", logName, json);

                var res = await client.PostAsync("/api/register-supplier-trial", content);

                var responseContent = await res.Content.ReadAsStringAsync();

                await Logger.LogInfo($"Response from {usedBaseUrl} | StatusCode: {(int)res.StatusCode} {res.StatusCode}",
                    logName, responseContent);

                if (!res.IsSuccessStatusCode)
                    throw new Exception("Failed to Regsiter Suppliers To Kubboss");

                return JsonConvert.DeserializeObject<SupplierRegisterResponse>(responseContent);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Regsiter Suppliers To Kubboss");
                Logger.LogInfo($"Exception occurred: {ex.Message}", logName, ex.ToString());

                return new SupplierRegisterResponse
                {
                    status = new Status
                    {
                        code = "500",
                        message = " response failed"
                    },
                    data = null
                };
            }
        }

        public async Task<JObject> VendorRegisterQuestionnaireUpdate(QuestionnaireUpdateRequest request)
        {
            SupplierRegisterResponse response = new SupplierRegisterResponse();
            DateTime createdDate = DateTime.Now;
            string logName = "RegisterSupplierKubboss";

            try
            {
                var clientSystem = _httpContextAccessor.HttpContext?.Request.Headers["X-Client-System"].ToString();
                Logger.LogInfo($"[ENTRY] X-Client-System header = '{clientSystem}'", logName);

                var system = (await _wolfApproveRepository.SP_GET_Systems(clientSystem))?.FirstOrDefault();

                HttpClient client;
                string usedBaseUrl;

                if (system != null)
                {
                    // ใช้ค่าจาก DB
                    await Logger.LogInfo($"Host '{clientSystem}' matched system in DB -> SystemCode: {system.SystemCode}, BaseUrl: {system.BaseUrl}",
                        logName);

                    client = HttpClientHelper.CreateClient(system.BaseUrl, system.Token);
                    usedBaseUrl = system.BaseUrl;
                }
                else
                {
                    // Fallback ไปใช้ config เดิม
                    var sqlParameter = new SqlParameter[]
                    {
                new SqlParameter("@sChannel", _appConfigHelper.GetConfiguration("KubbossChannel"))
                    };
                    var configToken = await _dbContext.ExcuteStoreQuerySingleAsync<SP_GET_SYSENDPOINT>(
                        "SP_GET_SYSENDPOINT",
                        sqlParameter);
                    var endPoint = _appConfigHelper.GetConfiguration("EndPoint:Kubboss");

                    await Logger.LogInfo($"Host '{clientSystem}' not found in DB -> fallback to config EndPoint:Kubboss = {endPoint}",
                        logName);

                    client = HttpClientHelper.CreateClient(endPoint, configToken?.sToken);
                    usedBaseUrl = endPoint;
                }

                var json = JsonConvert.SerializeObject(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                await Logger.LogInfo($"Sending request to {usedBaseUrl}/api/questionnaire/answer/update", logName, json);

                var res = await client.PostAsync("/api/questionnaire/answer/update", content);

                var responseContent = await res.Content.ReadAsStringAsync();

                await Logger.LogInfo($"Response from {usedBaseUrl} | StatusCode: {(int)res.StatusCode} {res.StatusCode}",
                    logName, responseContent);

                if (!res.IsSuccessStatusCode)
                    throw new Exception("Failed to Regsiter Suppliers To Kubboss");

                return JObject.Parse(responseContent);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Regsiter Suppliers To Kubboss");
                Logger.LogInfo($"Exception occurred: {ex.Message}", logName, ex.ToString());

                var errorResponse = new JObject
                {
                    ["success"] = false,
                    ["message"] = ex.Message
                };
                return errorResponse;

            }
        }

        private async Task<List<JObject>> GetSupplierQuestionnaires(string supplier_id, HttpClient client)
        {

            var response = await client.GetAsync($"/api/questionnaire/supplier/{supplier_id}/answers");
            if (!response.IsSuccessStatusCode) throw new Exception("Failed to fetch questionnaire list");

            var content = await response.Content.ReadAsStringAsync();
            var root = JObject.Parse(content);
            var items = root["data"] as JArray;
            if (items == null || !items.Any()) return new List<JObject>();

            var result = new List<JObject>();
            foreach (var item in items)
            {
                var questionnaireId = item["id"]?.ToString();
                if (string.IsNullOrEmpty(questionnaireId)) continue;
                var answer = await GetAnswer(client, questionnaireId);
                if (answer != null) result.Add(answer);
            }
            return result;
        }

        private async Task<JObject> GetAnswer(HttpClient client, string questionnaireId)
        {
            var res = await client.GetAsync($"/api/questionnaire/{questionnaireId}/answer");
            if (!res.IsSuccessStatusCode) return null;

            var content = await res.Content.ReadAsStringAsync();
            return JObject.Parse(content);
        }

        public async Task<List<SP_GET_Buyer_Code>> GetActiveBuyerRoute(string buyerCode)
        {
            return await _wolfApproveRepository.SP_GET_Buyer_Code(buyerCode.ToUpper());
        }

        public async Task<bool> SendToBuyer(SP_GET_Buyer_Code route, string payloadJson)
        {
            try
            {
                using var client = new HttpClient
                {
                    BaseAddress = new Uri(route.BaseUrl)
                };

                Logger.LogInfo($"Start Send | Buyer:{route.BuyerCode} | Method:{route.HttpMethod} | Url:{route.BaseUrl}{route.Path}", "SendToBuyer");

                client.DefaultRequestHeaders.TryAddWithoutValidation("IsCool", "true");

                if (route.AuthType?.ToUpper() == "BEARER")
                {
                    var token = await GetBearerToken(route);
                    await Logger.LogInfo($"Bearer token obtained | Buyer:{route.BuyerCode} | HasToken:{!string.IsNullOrEmpty(token)}", "SendToBuyer");

                    if (string.IsNullOrEmpty(token))
                    {
                        await Logger.LogInfo($"ABORT | Bearer token is null/empty | Buyer:{route.BuyerCode}", "SendToBuyer");
                        return false;
                    }

                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);
                }

                var content = new StringContent(payloadJson, Encoding.UTF8, route.ContentType ?? "application/json");

                await Logger.LogInfo($"Sending request now | Buyer:{route.BuyerCode} | {route.HttpMethod.ToUpper()} {route.BaseUrl}{route.Path}", "SendToBuyer");

                HttpResponseMessage response = route.HttpMethod.ToUpper() switch
                {
                    "POST" => await client.PostAsync(route.Path, content),
                    "PUT" => await client.PutAsync(route.Path, content),
                    _ =>
                    throw new NotSupportedException($"HTTP method {route.HttpMethod} not supported")
                };

                var responseBody = await response.Content.ReadAsStringAsync();
                Logger.LogInfo($@"
                ========== SEND TO BUYER ==========
                Buyer      : {route.BuyerCode}
                Method     : {route.HttpMethod}
                URL        : {route.BaseUrl}{route.Path}
                Auth Type  : {route.AuthType}
                ContentType: {route.ContentType ?? "application/json"}

                ----- Request -----
                {payloadJson}

                ----- Response -----
                Status : {(int)response.StatusCode} ({response.StatusCode})

                {responseBody}

                Result : {(response.IsSuccessStatusCode ? "SUCCESS" : "FAILED")}
                ==================================", "SendToBuyer");

                return response.IsSuccessStatusCode;

            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $@"
                ========== SEND TO BUYER EXCEPTION ==========
                Buyer  : {route?.BuyerCode}
                Method : {route?.HttpMethod}
                URL    : {route?.BaseUrl}{route?.Path}

                ----- Request -----
                {payloadJson}

                ----- Exception -----
                {ex}
                ============================================");
            }

            return false;
        }

        private async Task<string> GetBearerToken(SP_GET_Buyer_Code route)
        {
            var cacheKey = $"TOKEN_{route.BuyerCode}";

            if (_cache.TryGetValue(cacheKey, out string token))
            {
                await Logger.LogInfo($"Cache HIT | Buyer:{route.BuyerCode} | cacheKey:{cacheKey}", "GetBearerToken");
                return token;
            }

            await Logger.LogInfo($"Cache MISS | Buyer:{route.BuyerCode} | requesting new token", "GetBearerToken");

            if (string.IsNullOrWhiteSpace(route.BaseUrl) || string.IsNullOrWhiteSpace(route.AuthUrlPath))
            {
                await Logger.LogInfo($"ABORT | BaseUrl or AuthUrlPath is null/empty", "GetBearerToken");
                throw new Exception("Auth BaseUrl or AuthUrlPath is not configured");
            }

            var sw = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                using var client = new HttpClient
                {
                    BaseAddress = new Uri(route.BaseUrl),
                    Timeout = TimeSpan.FromSeconds(15)
                };

                object body;
                if (!string.IsNullOrEmpty(route.AuthBodyJson))
                {
                    body = JObject.Parse(route.AuthBodyJson);
                }
                else
                {
                    body = new
                    {
                        username = route.AuthUsername,
                        password = route.AuthPassword
                    };
                }

                var bodyJson = JsonConvert.SerializeObject(body);

                // สร้าง HttpRequestMessage เองแทน PostAsync ตรงๆ เพื่อ log ได้ครบทุก field ก่อนส่ง
                var request = new HttpRequestMessage(HttpMethod.Post, route.AuthUrlPath)
                {
                    Content = new StringContent(bodyJson, Encoding.UTF8, "application/json")
                };

                // ประกอบ log ให้เหมือน curl --location ... --header ... --data-raw ...
                var headerDump = string.Join("\n", client.DefaultRequestHeaders
                    .Select(h => $"  {h.Key}: {string.Join(",", h.Value)}"));

                var fullUrl = $"{client.BaseAddress}{route.AuthUrlPath}";

                await Logger.LogInfo($@"
        ========== OUTGOING AUTH REQUEST ==========
        Method     : POST
        Full URL   : {fullUrl}
        BaseAddress: {client.BaseAddress}
        RequestUri : {request.RequestUri}
        Headers (client.DefaultRequestHeaders):
        {(string.IsNullOrEmpty(headerDump) ? "  (none)" : headerDump)}
        Content-Type: application/json
        Body       : {bodyJson}
        ============================================", "GetBearerToken");

                var response = await client.SendAsync(request);

                await Logger.LogInfo($"Auth response received | Buyer:{route.BuyerCode} | Status:{(int)response.StatusCode} {response.StatusCode} | ElapsedAfterSend:{sw.ElapsedMilliseconds}ms", "GetBearerToken");

                var content = await response.Content.ReadAsStringAsync();

                // log response headers ด้วย เผื่อ gateway ตอบ header อะไรที่บอกใบ้ว่า route ผิดตรงไหน
                var responseHeaderDump = string.Join("\n", response.Headers
                    .Select(h => $"  {h.Key}: {string.Join(",", h.Value)}"));

                await Logger.LogInfo($@"
        ========== AUTH RESPONSE ==========
        Status     : {(int)response.StatusCode} {response.StatusCode}
        Headers    :
        {responseHeaderDump}
        Body       : {content}
        ====================================", "GetBearerToken");

                if (!response.IsSuccessStatusCode)
                {
                    await Logger.LogInfo($"FAILED to get token | Buyer:{route.BuyerCode} | Status:{(int)response.StatusCode} | Response:{content}", "GetBearerToken");
                    throw new Exception("Failed to get token");
                }

                var json = JObject.Parse(content);
                var tokenPath = route.TokenJsonPath ?? "$.Result";
                token = json.SelectToken(tokenPath)?.ToString();

                await Logger.LogInfo($"Token extracted | TokenFound:{!string.IsNullOrEmpty(token)}", "GetBearerToken");

                var expire = TimeSpan.FromMinutes(route.TokenExpireMinutes ?? 30);
                _cache.Set(cacheKey, token, expire);

                return token;
            }
            catch (TaskCanceledException ex)
            {
                await Logger.LogInfo($"TIMEOUT | Buyer:{route.BuyerCode} | after {sw.ElapsedMilliseconds}ms | {ex.Message}", "GetBearerToken");
                throw;
            }
            catch (Exception ex)
            {
                await Logger.LogInfo($"EXCEPTION | Buyer:{route.BuyerCode} | ElapsedBeforeFail:{sw.ElapsedMilliseconds}ms | {ex.GetType().Name}: {ex.Message}", "GetBearerToken");
                throw;
            }
        }

        private static readonly MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());

        public async Task<JObject> GetQuotationDetail(HttpClient client, string quoId)
        {
            var response = await client.GetAsync($"/api/quotations/{quoId}");

            if (!response.IsSuccessStatusCode)
                throw new Exception("Failed to get quotation from Kubboss");

            var content = await response.Content.ReadAsStringAsync();

            return JObject.Parse(content);
        }

        public async Task<JObject> GetSuppliersDetail(HttpClient client, string supplier_id)
        {
            var response = await client.GetAsync($"/api/suppliers/{supplier_id}");

            if (!response.IsSuccessStatusCode)
                throw new Exception("Failed to get quotation from Kubboss");

            var content = await response.Content.ReadAsStringAsync();

            return JObject.Parse(content);
        }

        public async Task<ProductMedicalResponse> GetProductMedical(string? sku, string? name, string? sortDirection, int page, int per_page)
        {
            try
            {
                var host = _httpContextAccessor.HttpContext?.Request.Host.Host.ToLower();
                var system = await _wolfApproveRepository.SP_GET_Systems(host);
                if (system == null || system.Count == 0)
                    throw new Exception("System not configured");

                var systemConfig = system.First();
                var baseUrl = systemConfig.BaseUrl;
                var token = systemConfig.Token;

                var client = HttpClientHelper.CreateClient(baseUrl, token);

                var query = new Dictionary<string, string?>
                {
                    ["sku"] = sku,
                    ["name"] = name,
                    ["sortDirection"] = sortDirection,
                    ["page"] = page.ToString(),
                    ["per_page"] = per_page.ToString()
                };

                var url = QueryHelpers.AddQueryString("api/product-medical", query);

                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    throw new Exception("Failed to call destination API");

                var content = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<ProductMedicalResponse>(content);
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "GetProductMedical");

                return new ProductMedicalResponse
                {
                    status = new Status()
                    {
                        code = "500",
                        message = "Failed to GetProductMedical"
                    },
                    data = null

                };
            }
        }

        public async Task<ProductMedicalByIdResponse> GetProductMedicalByID(string id)
        {
            try
            {
                var host = _httpContextAccessor.HttpContext?.Request.Host.Host.ToLower();
                var system = await _wolfApproveRepository.SP_GET_Systems(host);
                if (system == null || system.Count == 0)
                    throw new Exception("System not configured");

                var systemConfig = system.First();
                var baseUrl = systemConfig.BaseUrl;
                var token = systemConfig.Token;

                var client = HttpClientHelper.CreateClient(baseUrl, token);

                var response = await client.GetAsync($"/api/product-medical/{id}");

                if (!response.IsSuccessStatusCode)
                    throw new Exception("Failed to call destination API");

                var content = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<ProductMedicalByIdResponse>(content);
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "GetProductMedical By ID");

                return new ProductMedicalByIdResponse
                {
                    status = new Status()
                    {
                        code = "500",
                        message = "Failed to GetProductMedical By ID"
                    },
                    data = null

                };
            }
        }
        public async Task<POCreateResponse> CreatePOKubboss(HttpClient client, POCreateRequest request)
        {
            try
            {
                var json = JsonConvert.SerializeObject(request);

                var content = new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json");

                var res = await client.PostAsync("/api/document/purchase_order/create", content);


                var responseContent = await res.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<POCreateResponse>(responseContent);
            }
            catch (System.Exception ex)
            {

                Logger.LogError(ex, "CreatePOKubboss");

                return new POCreateResponse
                {
                    status = new Status
                    {
                        code = "500",
                        message = " response failed"
                    },
                    data = null
                };
            }
        }
        public async Task<POCreateV2Response> CreatePOKubbossV2(HttpClient client, POCreateV2Request request)
        {
            try
            {
                var json = JsonConvert.SerializeObject(request);

                var content = new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json");

                var res = await client.PostAsync("/api/document/v2/purchase_order/create", content);


                var responseContent = await res.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<POCreateV2Response>(responseContent);
            }
            catch (System.Exception ex)
            {

                Logger.LogError(ex, "CreatePOKubboss");

                return new POCreateV2Response
                {
                    status = new Status
                    {
                        code = "500",
                        message = " response failed"
                    },
                    data = null
                };
            }
        }
        public async Task<POCancelResponse> CancelPOKubboss(HttpClient client, POCancelRequest request)
        {
            try
            {
                var body = new
                {
                    remark = request.cancel_reason ?? "ยกเลิก PO"
                };

                var json = JsonConvert.SerializeObject(body);

                var content = new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json");


                var res = await client.PatchAsync($"api/purchase-order/{request.purchase_order_number}/cancel", content);

                var responseContent = await res.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<POCancelResponse>(responseContent);
            }
            catch (System.Exception ex)
            {

                Logger.LogError(ex, "CanlcelPOKubboss");

                return new POCancelResponse
                {
                    status = new Status
                    {
                        code = "500",
                        message = " response failed"
                    },
                    data = null
                };
            }
        }
        public async Task<RFQCancelResponse> CancelRFQKubboss(HttpClient client, RFQCancelRequest request)
        {
            try
            {
                var body = new
                {
                    remark = request.remark ?? "ยกเลิก RFQ"
                };

                var json = JsonConvert.SerializeObject(body);

                var content = new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json");


                var res = await client.PatchAsync($"api/rfq/{request.rfq_number}/cancel", content);

                var responseContent = await res.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<RFQCancelResponse>(responseContent);
            }
            catch (System.Exception ex)
            {

                Logger.LogError(ex, "CreatePOKubboss");

                return new RFQCancelResponse
                {
                    status = new Status
                    {
                        code = "500",
                        message = " response failed"
                    },
                    data = null
                };
            }
        }

        public async Task<QuotationAwardResponse> CreatePOAwardKubboss(HttpClient client, QuotationAwardRequest request)
        {
            try
            {
                var body = new
                {
                    purchase_order_number = request.purchase_order_number ?? "",
                    order_date = request.order_date ?? "",
                    require_date = request.require_date
                };

                var json = JsonConvert.SerializeObject(body);

                var content = new StringContent(
                    json,
                    Encoding.UTF8,
                    "application/json");


                var res = await client.PostAsync($"api/purchase-order/create/by-quotation/{request.quotation_id}", content);

                var responseContent = await res.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<QuotationAwardResponse>(responseContent);
            }
            catch (System.Exception ex)
            {

                Logger.LogError(ex, "CreatePOKubboss");

                return new QuotationAwardResponse
                {
                    status = new Status
                    {
                        code = "500",
                        message = " response failed"
                    },
                    data = null
                };
            }
        }

        public async Task<DocumentCreatetResponse> RequestDocuments(RequestDocumentRequest request)
        {

            DateTime createdDate = DateTime.Now;
            try
            {
                DocumentCreatetResponse result = await CreateDocument(request);

                if (result?.data == null)
                    return result;

                if (result.data.id == null)
                    return result;


                if (request.files?.Any() == true)
                {
                    foreach (var file in request.files)
                    {
                        await UploadMedia(
                            result.data.id,
                            file: file);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Regsiter Suppliers To Kubboss");

                return new DocumentCreatetResponse
                {
                    status = new Status
                    {
                        code = "500",
                        message = " response failed"
                    },
                    data = null
                };

            }
        }

        public async Task<DocumentUpdateResponse> RequestDocumentsUpdate(DocumentUpdateRequest request)
        {
            try
            {
                DocumentUpdateResponse result = await UpdateDocument(request);

                if (result?.data == null)
                    return result;

                if (result.data.id == null)
                    return result;

                return result;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Update Document To Kubboss");

                return new DocumentUpdateResponse
                {
                    status = new Status
                    {
                        code = "500",
                        message = " response failed"
                    },
                    data = null
                };

            }
        }
        public async Task<DocumentCreatetResponse> GetRequestDocumentsByID(string id, PutRequestDocuments request)
        {
            try
            {
                Logger.LogInfo($"Start GetRequestDocumentsByID | id={id} | buyerCode={request?.buyerCode}", "GetRequestDocumentsByID");

                var sqlParameter = new SqlParameter[] {
                        new SqlParameter("@sChannel", _appConfigHelper.GetConfiguration("KubbossChannel"))
                    };

                Logger.LogInfo("Before SP_GET_SYSENDPOINT", "GetRequestDocumentsByID");
                var configToken = await _dbContext.ExcuteStoreQuerySingleAsync<SP_GET_SYSENDPOINT>("SP_GET_SYSENDPOINT", sqlParameter);
                Logger.LogInfo($"After SP_GET_SYSENDPOINT | tokenIsNull={configToken?.sToken == null}", "GetRequestDocumentsByID");

                var endPoint = _appConfigHelper.GetConfiguration("EndPoint:Kubboss");
                Logger.LogInfo($"Kubboss endpoint={endPoint}", "GetRequestDocumentsByID");

                var client = HttpClientHelper.CreateClient(endPoint, configToken?.sToken);

                Logger.LogInfo($"Before GET /api/request-documents/{id}", "GetRequestDocumentsByID");
                var response = await client.GetAsync($"/api/request-documents/{id}");
                Logger.LogInfo($"After GET /api/request-documents/{id} | Status={(int)response.StatusCode} ({response.StatusCode})", "GetRequestDocumentsByID");

                if (!response.IsSuccessStatusCode)
                {
                    var errorBody = await response.Content.ReadAsStringAsync();
                    Logger.LogError(null, $"Kubboss API failed | id={id} | Status={(int)response.StatusCode} | Body={errorBody}");
                    throw new Exception("Failed to call destination API");
                }

                var content = await response.Content.ReadAsStringAsync();
                Logger.LogInfo($"Response body length={content?.Length}", "GetRequestDocumentsByID");

                var result = JsonConvert.DeserializeObject<DocumentCreatetResponse>(content);

                Logger.LogInfo("Before SP_GET_RequestDocument (local)", "GetRequestDocumentsByID");
                var localData = await _wolfApproveRepository.SP_GET_RequestDocument(id);
                Logger.LogInfo($"After SP_GET_RequestDocument | localDataIsNull={localData == null} | docNo={localData?.docNo} | companyCode={localData?.company_code}", "GetRequestDocumentsByID");

                string companyCode = null;
                if (result?.data != null && localData != null)
                {
                    result.data.docNo = localData.docNo;
                    result.data.memoId = localData.memoId;
                    companyCode = localData.company_code;

                    if (result?.data?.signatures != null)
                    {
                        foreach (var signature in result.data.signatures)
                        {
                            signature.user_id ??= 0;
                        }
                    }
                }
                else
                {
                    Logger.LogInfo($"Skip merge local data | resultDataIsNull={result?.data == null} | localDataIsNull={localData == null}", "GetRequestDocumentsByID");
                }

                var routeKey = !string.IsNullOrEmpty(companyCode) ? companyCode : request.buyerCode;
                Logger.LogInfo($"Resolved routeKey={routeKey} (source={(!string.IsNullOrEmpty(companyCode) ? "companyCode" : "request.buyerCode")})", "GetRequestDocumentsByID");

                if (string.IsNullOrEmpty(routeKey))
                {
                    // ป้องกัน route ไปมั่วๆ ถ้าไม่มีทั้งคู่
                    throw new InvalidOperationException($"Cannot resolve buyer route: missing both CompanyCode and buyerCode for document id={id}");
                }

                Logger.LogInfo($"Before GetActiveBuyerRoute | routeKey={routeKey}", "GetRequestDocumentsByID");
                var routes = await GetActiveBuyerRoute(routeKey);
                Logger.LogInfo($"After GetActiveBuyerRoute | count={routes?.Count() ?? 0}", "GetRequestDocumentsByID");

                var buyerRoute = routes.FirstOrDefault(x => x.ActionType == "CREATE_VENDORSIGNATURE");
                if (buyerRoute == null)
                {
                    throw new InvalidOperationException($"No active route found for key='{routeKey}', action='CREATE_VENDORSIGNATURE'");
                }
                Logger.LogInfo($"Found buyerRoute | BuyerCode={buyerRoute.BuyerCode} | BaseUrl={buyerRoute.BaseUrl} | Path={buyerRoute.Path}", "GetRequestDocumentsByID");

                var jObj = JObject.FromObject(result);
                var payloadJson = JsonConvert.SerializeObject(jObj);

                Logger.LogInfo($"Before SendToBuyer | BuyerCode={buyerRoute.BuyerCode} | id={id}", "GetRequestDocumentsByID");
                var sendResult = await SendToBuyer(buyerRoute, payloadJson);
                Logger.LogInfo($"After SendToBuyer | BuyerCode={buyerRoute.BuyerCode} | id={id} | Success={sendResult}", "GetRequestDocumentsByID");

                Logger.LogInfo($"End GetRequestDocumentsByID | id={id} | Success=true", "GetRequestDocumentsByID");
                return result;
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, $"Get RequestDocuments By ID | id={id} | buyerCode={request?.buyerCode}");
                return new DocumentCreatetResponse
                {
                    status = new Status()
                    {
                        code = "500",
                        message = "Failed to RequestDocuments By ID"
                    },
                    data = null
                };
            }
        }

        private async Task<DocumentCreatetResponse> CreateDocument(RequestDocumentRequest request)
        {
            DateTime createdDate = DateTime.Now;
            try
            {
                var sqlParameter = new SqlParameter[] {
                                new SqlParameter("@sChannel", _appConfigHelper.GetConfiguration("KubbossChannel"))
                            };

                var configToken = await _dbContext.ExcuteStoreQuerySingleAsync<SP_GET_SYSENDPOINT>("SP_GET_SYSENDPOINT", sqlParameter);

                var endPoint = _appConfigHelper.GetConfiguration("EndPoint:Kubboss");

                var client = HttpClientHelper.CreateClient(endPoint, configToken?.sToken);

                var supplierId = request.supplier_id;
                if (supplierId == null || supplierId == 0)
                {
                    var checkSupplierBody = new
                    {
                        email = request.email
                    };

                    var checkJson = JsonConvert.SerializeObject(checkSupplierBody);

                    var checkContent = new StringContent(checkJson, Encoding.UTF8, "application/json");

                    var checkRes = await client.PostAsync("/api/check-supplier", checkContent);

                    if (!checkRes.IsSuccessStatusCode)
                        throw new Exception("Failed to check supplier.");

                    var checkResult = await checkRes.Content.ReadAsStringAsync();

                    var checkObj = JObject.Parse(checkResult);

                    supplierId = checkObj["data"]?["id"]?.Value<int>() ?? 0;

                    if (supplierId == 0)
                        throw new Exception($"Supplier not found. Email: {request.email}");
                }

                var companyCode = request.company_code;
                var companyID = request.company_id;

                if (!string.IsNullOrEmpty(companyCode))
                {

                    var resultCOMPANY = await _masterDataRepository.SP_GET_MASTER_COMPANY(true);
                    if (resultCOMPANY != null)
                    {
                        var nCompanyCode = companyCode;
                        var data = resultCOMPANY.Where(e => e.nCompanyCode == nCompanyCode).FirstOrDefault();

                        companyID = data.nCompanyID.ToString();
                    }
                }

                var createRequestBody = new RequestDocumentRequest
                {
                    supplier_id = supplierId,
                    company_id = companyID,
                    company_code = request.company_code,
                    document_name = request.document_name,
                    reason = request.reason,
                    email = request.email,
                    is_require_signature = request.is_require_signature,
                    lang = request.lang
                };

                var json = JsonConvert.SerializeObject(createRequestBody);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var res = await client.PostAsync($"/api/request-documents", content);

                if (!res.IsSuccessStatusCode)
                    throw new Exception("Failed to call destination API");

                var responseContent = await res.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<DocumentCreatetResponse>(responseContent);


                if (result?.status?.code != "200" || result.data == null)
                {
                    throw new Exception(result?.status?.message ?? "Create document failed");
                }

                await _wolfApproveRepository.SP_INSERT_RequestDocument(
                    request.docNo,
                    request.memoId,
                    result.data.id, // kubboss_document_id
                    request.supplier_id.ToString(),
                    companyID,
                    request.company_code,
                    request.document_name,
                    request.reason,
                    request.email,
                    request.is_require_signature,
                    request.lang
                );

                return JsonConvert.DeserializeObject<DocumentCreatetResponse>(responseContent);

            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Request Document");

                return new DocumentCreatetResponse
                {
                    status = new Status
                    {
                        code = "500",
                        message = " response failed"
                    },
                    data = null
                };

            }
        }
        private async Task<DocumentUpdateResponse> UpdateDocument(DocumentUpdateRequest request)
        {
            DateTime createdDate = DateTime.Now;
            try
            {

                var localData = await _wolfApproveRepository.SP_GET_RequestDocument(request.id);

                var sqlParameter = new SqlParameter[] {
                                new SqlParameter("@sChannel", _appConfigHelper.GetConfiguration("KubbossChannel"))
                            };

                var configToken = await _dbContext.ExcuteStoreQuerySingleAsync<SP_GET_SYSENDPOINT>("SP_GET_SYSENDPOINT", sqlParameter);

                var endPoint = _appConfigHelper.GetConfiguration("EndPoint:Kubboss");

                var client = HttpClientHelper.CreateClient(endPoint, configToken?.sToken);


                var createRequestBody = new
                {
                    status = request.status,
                    reason = request.reason,
                    lang = request.lang,
                };

                var json = JsonConvert.SerializeObject(createRequestBody);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var res = await client.PatchAsync($"/api/request-documents/{localData.kubboss_document_id}/update-status", content);

                if (!res.IsSuccessStatusCode)
                    throw new Exception("Failed to call destination API");

                var responseContent = await res.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<DocumentCreatetResponse>(responseContent);


                if (result?.status?.code != "200" || result.data == null)
                {
                    throw new Exception(result?.status?.message ?? "Create document failed");
                }

                return JsonConvert.DeserializeObject<DocumentUpdateResponse>(responseContent);

            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Regsiter Suppliers To Kubboss");

                return new DocumentUpdateResponse
                {
                    status = new Status
                    {
                        code = "500",
                        message = " response failed"
                    },
                    data = null
                };

            }
        }

        private async Task UploadMedia(string modelId, IFormFile file = null)
        {
            try
            {

                var sqlParameter = new SqlParameter[] {
                                new SqlParameter("@sChannel", _appConfigHelper.GetConfiguration("KubbossChannel"))
                            };

                var configToken = await _dbContext.ExcuteStoreQuerySingleAsync<SP_GET_SYSENDPOINT>("SP_GET_SYSENDPOINT", sqlParameter);

                var endPoint = _appConfigHelper.GetConfiguration("EndPoint:Kubboss");

                var client = HttpClientHelper.CreateClient(endPoint, configToken?.sToken);

                using var form = new MultipartFormDataContent();

                form.Add(new StringContent(modelId), "model_id");
                form.Add(new StringContent("RequestDocument"), "type");
                form.Add(new StringContent(Guid.NewGuid().ToString()), "file_uuid");

                var streamContent = new StreamContent(file.OpenReadStream());

                streamContent.Headers.ContentType =
                    new System.Net.Http.Headers.MediaTypeHeaderValue(
                        file.ContentType);

                form.Add(
                    streamContent,
                    "file",
                    file.FileName);

                var response = await client.PostAsync(
                    "/api/media",
                    form);

                response.EnsureSuccessStatusCode();

            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "UploadMedia");

            }

        }

        public async Task<DeliveryOrdersResponse> GetDeliveryOrdersByID(string id, PutDeliveryOrdersRequest request)
        {
            try
            {
                var sqlParameter = new SqlParameter[] {
                                new SqlParameter("@sChannel", _appConfigHelper.GetConfiguration("KubbossChannel"))
                            };

                var configToken = await _dbContext.ExcuteStoreQuerySingleAsync<SP_GET_SYSENDPOINT>("SP_GET_SYSENDPOINT", sqlParameter);

                var endPoint = _appConfigHelper.GetConfiguration("EndPoint:Kubboss");

                var client = HttpClientHelper.CreateClient(endPoint, configToken?.sToken);

                var responseDelivery = await client.GetAsync($"/api/delivery-orders/{id}");

                if (!responseDelivery.IsSuccessStatusCode)
                    throw new Exception("Failed to call destination API");

                var content = await responseDelivery.Content.ReadAsStringAsync();

                var resultDeliveryOrders = JsonConvert.DeserializeObject<DeliveryOrdersResponse>(content);

                var routes = await GetActiveBuyerRoute(request.buyerCode);
                var buyerRoute = routes.FirstOrDefault(x => x.ActionType == "CREATE_DO");

                if (buyerRoute == null)
                    throw new Exception("Failed to call destination API");

                var jObj = JObject.Parse(content);

                var payloadObj = jObj["data"] ?? new JObject();

                var payloadJson = JsonConvert.SerializeObject(payloadObj);

                await SendToBuyer(buyerRoute, payloadJson);

                return resultDeliveryOrders;
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "Get Delivery Orders By ID");

                return new DeliveryOrdersResponse
                {
                    status = new Status()
                    {
                        code = "500",
                        message = "Failed to Delivery Orders"
                    },
                    data = null
                };
            }
        }
        public async Task<DeliveryOrdersUpdateResponse> DeliveryOrdersUpdateStatus(PutDeliveryOrdersUpdateRequest request)
        {
            try
            {
                var sqlParameter = new SqlParameter[] {
                                new SqlParameter("@sChannel", _appConfigHelper.GetConfiguration("KubbossChannel"))
                            };

                var configToken = await _dbContext.ExcuteStoreQuerySingleAsync<SP_GET_SYSENDPOINT>("SP_GET_SYSENDPOINT", sqlParameter);

                var endPoint = _appConfigHelper.GetConfiguration("EndPoint:Kubboss");

                var client = HttpClientHelper.CreateClient(endPoint, configToken?.sToken);

                var updateRequestBody = new
                {
                    status = request.status,
                    reason = request.reason,
                    lang = request.lang,
                };

                var json = JsonConvert.SerializeObject(updateRequestBody);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PutAsync($"/api/delivery-orders/{request.id}/update-status", content);


                if (!response.IsSuccessStatusCode)
                    throw new Exception("Failed to call destination API");

                var responseContent = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<DeliveryOrdersUpdateResponse>(responseContent);

                return result;
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "Get RequestDocuments By ID");

                return new DeliveryOrdersUpdateResponse
                {
                    status = new Status()
                    {
                        code = "500",
                        message = "Failed to RequestDocuments By ID"
                    },
                    data = null
                };
            }
        }

        public async Task<CreditNoteUpdateResponse> CreditNoteUpdateStatus(PutCreditNoteUpdateRequest request)
        {
            try
            {
                var sqlParameter = new SqlParameter[] {
                                new SqlParameter("@sChannel", _appConfigHelper.GetConfiguration("KubbossChannel"))
                            };

                var configToken = await _dbContext.ExcuteStoreQuerySingleAsync<SP_GET_SYSENDPOINT>("SP_GET_SYSENDPOINT", sqlParameter);

                var endPoint = _appConfigHelper.GetConfiguration("EndPoint:Kubboss");

                var client = HttpClientHelper.CreateClient(endPoint, configToken?.sToken);

                var updateRequestBody = new
                {
                    status = request.status,
                    reason = request.reason,
                    lang = request.lang,
                };

                var json = JsonConvert.SerializeObject(updateRequestBody);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PutAsync($"/api/credit-notes/{request.id}/update-status", content);


                if (!response.IsSuccessStatusCode)
                    throw new Exception("Failed to call destination API");

                var responseContent = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<CreditNoteUpdateResponse>(responseContent);

                return result;
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "CreditNoteUpdateStatus");

                return new CreditNoteUpdateResponse
                {
                    status = new Status()
                    {
                        code = "500",
                        message = "Failed to RequestDocuments By ID"
                    },
                    data = null
                };
            }
        }

        public async Task<DebitNoteUpdateResponse> DebitNoteUpdateStatus(PutDebitNoteUpdateRequest request)
        {
            try
            {
                var sqlParameter = new SqlParameter[] {
                                new SqlParameter("@sChannel", _appConfigHelper.GetConfiguration("KubbossChannel"))
                            };

                var configToken = await _dbContext.ExcuteStoreQuerySingleAsync<SP_GET_SYSENDPOINT>("SP_GET_SYSENDPOINT", sqlParameter);

                var endPoint = _appConfigHelper.GetConfiguration("EndPoint:Kubboss");

                var client = HttpClientHelper.CreateClient(endPoint, configToken?.sToken);

                var updateRequestBody = new
                {
                    status = request.status,
                    reason = request.reason,
                    lang = request.lang,
                };

                var json = JsonConvert.SerializeObject(updateRequestBody);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PutAsync($"/api/debit-notes/{request.id}/update-status", content);


                if (!response.IsSuccessStatusCode)
                    throw new Exception("Failed to call destination API");

                var responseContent = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<DebitNoteUpdateResponse>(responseContent);

                return result;
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "DebitNoteUpdateResponse");

                return new DebitNoteUpdateResponse
                {
                    status = new Status()
                    {
                        code = "500",
                        message = "Failed to RequestDocuments By ID"
                    },
                    data = null
                };
            }
        }

        public async Task<InvoicesUpdateResponse> InvoicesUpdateStatus(PutInvoicesUpdateRequest request)
        {
            try
            {
                var sqlParameter = new SqlParameter[] {
                                new SqlParameter("@sChannel", _appConfigHelper.GetConfiguration("KubbossChannel"))
                            };

                var configToken = await _dbContext.ExcuteStoreQuerySingleAsync<SP_GET_SYSENDPOINT>("SP_GET_SYSENDPOINT", sqlParameter);

                var endPoint = _appConfigHelper.GetConfiguration("EndPoint:Kubboss");

                var client = HttpClientHelper.CreateClient(endPoint, configToken?.sToken);

                var updateRequestBody = new
                {
                    status = request.status,
                    reason = request.reason,
                    lang = request.lang,
                };

                var json = JsonConvert.SerializeObject(updateRequestBody);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PutAsync($"/api/invoices/{request.id}/update-status", content);


                if (!response.IsSuccessStatusCode)
                    throw new Exception("Failed to call destination API");

                var responseContent = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<InvoicesUpdateResponse>(responseContent);

                return result;
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "Get RequestDocuments By ID");

                return new InvoicesUpdateResponse
                {
                    status = new Status()
                    {
                        code = "500",
                        message = "Failed to RequestDocuments By ID"
                    },
                    data = null
                };
            }
        }

        public async Task<InvoicesByIDResponse> GetInvoicesByID(string id, CreateInvoiceRequest request)
        {
            try
            {
                var sqlParameter = new SqlParameter[] {
                                new SqlParameter("@sChannel", _appConfigHelper.GetConfiguration("KubbossChannel"))
                            };

                var configToken = await _dbContext.ExcuteStoreQuerySingleAsync<SP_GET_SYSENDPOINT>("SP_GET_SYSENDPOINT", sqlParameter);

                var endPoint = _appConfigHelper.GetConfiguration("EndPoint:Kubboss");

                var client = HttpClientHelper.CreateClient(endPoint, configToken?.sToken);

                var responseDelivery = await client.GetAsync($"/api/invoices/{id}");

                if (!responseDelivery.IsSuccessStatusCode)
                    throw new Exception("Failed to call destination API");

                var content = await responseDelivery.Content.ReadAsStringAsync();

                var resultInvoices = JsonConvert.DeserializeObject<InvoicesByIDResponse>(content);

                var routes = await GetActiveBuyerRoute(request.buyerCode);
                var buyerRoute = routes.FirstOrDefault(x => x.ActionType == "CREATE_INVOICES");

                if (buyerRoute == null)
                    throw new Exception("Failed to call destination API");

                var jObj = JObject.Parse(content);

                var payloadObj = jObj["data"] ?? new JObject();

                var payloadJson = JsonConvert.SerializeObject(payloadObj);

                await SendToBuyer(buyerRoute, payloadJson);

                return resultInvoices;
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "Get invoices  By ID");

                return new InvoicesByIDResponse
                {
                    status = new Status()
                    {
                        code = "500",
                        message = "Failed to invoices"
                    },
                    data = null
                };
            }
        }

        public async Task<SubscriptionsResponse> Subscriptions(SubscriptionsRequest request)
        {
            try
            {
                var sqlParameter = new SqlParameter[] {
                                new SqlParameter("@sChannel", _appConfigHelper.GetConfiguration("KubbossChannel"))
                            };

                var configToken = await _dbContext.ExcuteStoreQuerySingleAsync<SP_GET_SYSENDPOINT>("SP_GET_SYSENDPOINT", sqlParameter);

                var endPoint = _appConfigHelper.GetConfiguration("EndPoint:Kubboss");

                var client = HttpClientHelper.CreateClient(endPoint, configToken?.sToken);

                // STEP 1 : Check Supplier Registered
                var checkRequest = new
                {
                    company_id = request.company_id,
                    email = request.email
                };

                var checkJson = JsonConvert.SerializeObject(checkRequest);

                var checkContent = new StringContent(checkJson, Encoding.UTF8, "application/json");

                var responseCheck = await client.PostAsync("/api/check-supplier-registered", checkContent);

                if (!responseCheck.IsSuccessStatusCode)
                    throw new Exception("Failed to check supplier registered.");

                var checkResponseString = await responseCheck.Content.ReadAsStringAsync();

                var checkResult = JsonConvert.DeserializeObject<SubscriptionsIDResponse>(checkResponseString);

                if (checkResult?.data == null || string.IsNullOrWhiteSpace(checkResult.data.subscription_id))
                {
                    throw new Exception("Subscription ID not found.");
                }

                // STEP 2 : Update Status
                var updateRequest = new SubscriptionsUpdateRequest
                {
                    status = request.status,
                    remark = request.remark
                };

                var updateJson = JsonConvert.SerializeObject(updateRequest);

                var updateContent = new StringContent(updateJson, Encoding.UTF8, "application/json");

                var responseUpdate = await client.PatchAsync($"/api/subscriptions/{checkResult.data.subscription_id}/update-status", updateContent);

                if (!responseUpdate.IsSuccessStatusCode)
                    throw new Exception("Failed to update subscription status.");

                var updateResponseString = await responseUpdate.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<SubscriptionsResponse>(updateResponseString);

                return result;
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "Get Subscriptions");

                return new SubscriptionsResponse
                {
                    status = new Status()
                    {
                        code = "500",
                        message = "Failed to Subscriptions"
                    },
                    data = null
                };
            }
        }

        public async Task<CreditNoteByIDResponse> GetCreditNotesByID(string id, PutCreditNotesRequest request)
        {
            try
            {
                var sqlParameter = new SqlParameter[] {
                                new SqlParameter("@sChannel", _appConfigHelper.GetConfiguration("KubbossChannel"))
                            };

                var configToken = await _dbContext.ExcuteStoreQuerySingleAsync<SP_GET_SYSENDPOINT>("SP_GET_SYSENDPOINT", sqlParameter);

                var endPoint = _appConfigHelper.GetConfiguration("EndPoint:Kubboss");

                var client = HttpClientHelper.CreateClient(endPoint, configToken?.sToken);

                var responseCreditNote = await client.GetAsync($"/api/credit-notes/{id}");

                if (!responseCreditNote.IsSuccessStatusCode)
                    throw new Exception("Failed to call destination API");

                var content = await responseCreditNote.Content.ReadAsStringAsync();

                var resultCreditNotes = JsonConvert.DeserializeObject<CreditNoteByIDResponse>(content);

                var routes = await GetActiveBuyerRoute(request.buyerCode);
                var buyerRoute = routes.FirstOrDefault(x => x.ActionType == "CREATE_CREDITNOTES");

                if (buyerRoute == null)
                    throw new Exception("Failed to call destination API");

                var jObj = JObject.Parse(content);

                var payloadObj = jObj["data"] ?? new JObject();

                var payloadJson = JsonConvert.SerializeObject(payloadObj);

                await SendToBuyer(buyerRoute, payloadJson);

                return resultCreditNotes;
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "Get CreditNote  By ID");

                return new CreditNoteByIDResponse
                {
                    status = new Status()
                    {
                        code = "500",
                        message = "Failed to CreditNote"
                    },
                    data = null
                };
            }
        }

        public async Task<DebitNotesByIDResponse> GetDebitNotesByID(string id, PutDebitNotesRequest request)
        {
            try
            {
                var sqlParameter = new SqlParameter[] {
                                new SqlParameter("@sChannel", _appConfigHelper.GetConfiguration("KubbossChannel"))
                            };

                var configToken = await _dbContext.ExcuteStoreQuerySingleAsync<SP_GET_SYSENDPOINT>("SP_GET_SYSENDPOINT", sqlParameter);

                var endPoint = _appConfigHelper.GetConfiguration("EndPoint:Kubboss");

                var client = HttpClientHelper.CreateClient(endPoint, configToken?.sToken);

                var responseDebitNotes = await client.GetAsync($"/api/debit-notes/{id}");

                if (!responseDebitNotes.IsSuccessStatusCode)
                    throw new Exception("Failed to call destination API");

                var content = await responseDebitNotes.Content.ReadAsStringAsync();

                var resultCreditNotes = JsonConvert.DeserializeObject<DebitNotesByIDResponse>(content);

                var routes = await GetActiveBuyerRoute(request.buyerCode);
                var buyerRoute = routes.FirstOrDefault(x => x.ActionType == "CREATE_DEBITNOTES");

                if (buyerRoute == null)
                    throw new Exception("Failed to call destination API");

                var jObj = JObject.Parse(content);

                var payloadObj = jObj["data"] ?? new JObject();

                var payloadJson = JsonConvert.SerializeObject(payloadObj);

                await SendToBuyer(buyerRoute, payloadJson);

                return resultCreditNotes;
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "Get CreditNote  By ID");

                return new DebitNotesByIDResponse
                {
                    status = new Status()
                    {
                        code = "500",
                        message = "Failed to CreditNote"
                    },
                    data = null
                };
            }
        }

        public async Task<POByIDResponse> GetPOByID(string id, CreatePORequest request)
        {
            try
            {
                var sqlParameter = new SqlParameter[] {
                                new SqlParameter("@sChannel", _appConfigHelper.GetConfiguration("KubbossChannel"))
                            };

                var configToken = await _dbContext.ExcuteStoreQuerySingleAsync<SP_GET_SYSENDPOINT>("SP_GET_SYSENDPOINT", sqlParameter);

                var endPoint = _appConfigHelper.GetConfiguration("EndPoint:Kubboss");

                var client = HttpClientHelper.CreateClient(endPoint, configToken?.sToken);

                var responseDelivery = await client.GetAsync($"/api/purchase-orders/{id}");

                if (!responseDelivery.IsSuccessStatusCode)
                    throw new Exception("Failed to call destination API");

                var content = await responseDelivery.Content.ReadAsStringAsync();

                var resultInvoices = JsonConvert.DeserializeObject<POByIDResponse>(content);

                var routes = await GetActiveBuyerRoute(request.buyerCode);
                var buyerRoute = routes.FirstOrDefault(x => x.ActionType == "CREATE_PO");

                if (buyerRoute == null)
                    throw new Exception("Failed to call destination API");

                var jObj = JObject.Parse(content);

                var payloadObj = jObj["data"] ?? new JObject();

                var payloadJson = JsonConvert.SerializeObject(payloadObj);

                await SendToBuyer(buyerRoute, payloadJson);

                return resultInvoices;
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "Get invoices  By ID");

                return new POByIDResponse
                {
                    status = new Status()
                    {
                        code = "500",
                        message = "Failed to invoices"
                    },
                    data = null
                };
            }
        }

        #region ResolveClientSystemAsync
        public class SystemEndpointContext
        {
            public string SystemCode { get; set; }
            public string BaseUrl { get; set; }
            public string Token { get; set; }
            public HttpClient Client { get; set; }
        }

        private async Task<SystemEndpointContext> ResolveClientSystemAsync(string buyerCode = null)
        {
            string logName = "RegisterSupplierKubboss";
            var clientSystem = _httpContextAccessor.HttpContext?.Request.Headers["X-Client-System"].ToString();
            Logger.LogInfo($"[ENTRY] X-Client-System header = '{clientSystem}', buyerCode = '{buyerCode}'", logName);

            List<SP_GET_Systems> matched = null;

            if (!string.IsNullOrWhiteSpace(clientSystem))
            {
                matched = await _wolfApproveRepository.SP_GET_Systems(clientSystem);
            }

            if ((matched == null || !matched.Any()) && !string.IsNullOrWhiteSpace(buyerCode))
            {
                matched = await _wolfApproveRepository.SP_GET_Systems(buyerCode);

                if (matched != null && matched.Any())
                {
                    Logger.LogInfo($"Resolved system from buyerCode '{buyerCode}'",logName);
                }
            }

            var system = matched?.FirstOrDefault();

            if (system != null)
            {
                Logger.LogInfo($"Using system from DB -> SystemCode: {system.SystemCode}, BaseUrl: {system.BaseUrl}",logName);

                return new SystemEndpointContext
                {
                    SystemCode = system.SystemCode,
                    BaseUrl = system.BaseUrl,
                    Token = system.Token,
                    Client = HttpClientHelper.CreateClient(system.BaseUrl, system.Token)
                };
            }

            // ทั้ง header และ buyerCode ไม่ match เลย -> fallback เดิม
            var sqlParameter = new SqlParameter[]
            {
        new SqlParameter("@sChannel", _appConfigHelper.GetConfiguration("KubbossChannel"))
            };
            var configToken = await _dbContext.ExcuteStoreQuerySingleAsync<SP_GET_SYSENDPOINT>(
                "SP_GET_SYSENDPOINT", sqlParameter);
            var endPoint = _appConfigHelper.GetConfiguration("EndPoint:Kubboss");

            Logger.LogInfo($"No system matched (header='{clientSystem}', buyerCode='{buyerCode}') -> fallback to config EndPoint:Kubboss = {endPoint}",logName);

            return new SystemEndpointContext
            {
                SystemCode = "KUBBOSS",
                BaseUrl = endPoint,
                Token = configToken?.sToken,
                Client = HttpClientHelper.CreateClient(endPoint, configToken?.sToken)
            };
        }
        #endregion
    }
}