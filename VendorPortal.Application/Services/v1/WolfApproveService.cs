using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HandlebarsDotNet;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VendorPortal.Application.Helpers;
using VendorPortal.Application.Interfaces.SyncExternalData;
using VendorPortal.Application.Interfaces.v1;
using VendorPortal.Application.Models.Common;
using VendorPortal.Application.Models.v1.Request;
using VendorPortal.Application.Models.v1.Response;
using VendorPortal.Domain.Interfaces.v1;
using VendorPortal.Domain.Models.WolfApprove.StoreModel;
using VendorPortal.Infrastructure.Extensions;
using VendorPortal.Logging;
using static VendorPortal.Application.Models.Common.AppEnum;
using static VendorPortal.Application.Models.Common.KubbossCommonModel;

namespace VendorPortal.Application.Services.v1
{
    public class WolfApproveService : IWolfApproveService
    {
        private readonly IWolfApproveRepository _wolfApproveRepository;
        private readonly IMasterDataRepository _masterDataRepository;
        private readonly AppConfigHelper _appConfigHelper;
        private readonly IHttpContextAccessor _httpContext;
        private readonly string _baseUrl = string.Empty;
        private readonly IConfiguration _config;
        private readonly DbContext _dbContext;
        private readonly IKubbossService _kubBossService;
        private readonly IBuyerApiService _buyerApiService;

        public WolfApproveService(IHttpContextAccessor httpContext,
            IWolfApproveRepository wolfApproveRepository,
            IMasterDataRepository masterDataRepository,
            AppConfigHelper appConfigHelper,
            IConfiguration config,
            DbContext dbContext,
            IKubbossService kubBossService,
            IBuyerApiService buyerApiService
            )
        {
            _httpContext = httpContext;
            _appConfigHelper = appConfigHelper;
            _baseUrl = $"{_httpContext.HttpContext.Request.Scheme}://{_httpContext.HttpContext.Request.Host}{_httpContext.HttpContext.Request.Path}";
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            _wolfApproveRepository = wolfApproveRepository;
            _masterDataRepository = masterDataRepository;
            _config = config;
            _dbContext = dbContext;
            _kubBossService = kubBossService;
            _buyerApiService = buyerApiService;

        }

        public async Task<ClaimConfirmResponse> ConfirmClaimStatus(string claim_id, ClaimConfirmRequest request)
        {
            ClaimConfirmResponse result = new();
            try
            {
                var store = await _wolfApproveRepository.SP_UPDATE_CLAIM_ORDER_CONFIRM(claim_id, request.Status, request.Reason, request.Description);
                if (store != null)
                {
                    result = new ClaimConfirmResponse()
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
                    // รอดูก่อนว่าตรงนี้ทำงานยังไง อาจจะต้องมี reason ที่แตกต่างกัน
                    result = new ClaimConfirmResponse()
                    {
                        status = new Status()
                        {
                            code = ResponseCode.NotFound.Text(),
                            message = ResponseCode.NotFound.Description()
                        }
                    };
                }
            }
            catch (ApplicationException ex)
            {
                Logger.LogError(ex, "ConfirmClaimStatus", $"claim_id: {claim_id} , request: {request}");
                result = new ClaimConfirmResponse()
                {
                    status = new Status()
                    {
                        code = ResponseCode.NotImplement.Text(),
                        message = ResponseCode.NotImplement.Description()
                    },
                    Data = null
                };
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "ConfirmClaimStatus", $"claim_id: {claim_id} , request: {request}");
                result = new ClaimConfirmResponse()
                {
                    status = new Status()
                    {
                        code = ResponseCode.InternalServerError.Text(),
                        message = ResponseCode.InternalServerError.Description()
                    },
                    Data = null
                };
            }
            return result;
        }

        public async Task<PurchaseOrderConfirmResponse> ConfirmPurchaseOrderStatus(string purchase_order_id, PurchaseOrderConfirmRequest request)
        {
            PurchaseOrderConfirmResponse response = new();
            try
            {
                var store = await _wolfApproveRepository.SP_PUT_PURCHASE_ORDER_CONFIRM(purchase_order_id, request.status, request.reason, request.description);
                if (store != null)
                {
                    response = new PurchaseOrderConfirmResponse
                    {
                        Data = new PurchaseOrderConfirmData
                        {
                            Status = store.Status
                        }
                    };
                    response.status = new Status
                    {
                        code = ResponseCode.Success.Text(),
                        message = ResponseCode.Success.Description()
                    };
                }
                else
                {
                    response = new PurchaseOrderConfirmResponse()
                    {
                        status = new Status()
                        {
                            code = ResponseCode.NotFound.Text(),
                            message = ResponseCode.NotFound.Description()
                        },
                        Data = null
                    };
                }
            }
            catch (ApplicationException ex)
            {
                Logger.LogError(ex, "ConfirmPurchaseOrderStatus", $"purchase_order_id: {purchase_order_id} , request: {request}");
                response = new PurchaseOrderConfirmResponse()
                {
                    status = new Status()
                    {
                        code = ResponseCode.NotImplement.Text(),
                        message = ResponseCode.NotImplement.Description()
                    },
                    Data = null
                };
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "ConfirmPurchaseOrderStatus", $"purchase_order_id: {purchase_order_id} , request: {request}");
                response = new PurchaseOrderConfirmResponse()
                {
                    status = new Status()
                    {
                        code = ResponseCode.InternalServerError.Text(),
                        message = ResponseCode.InternalServerError.Description()
                    },
                    Data = null
                };

            }
            return response;
        }

        public async Task<CompaniesConnectResponse> ConnectCompanies(string supplier_id, CompaniesConnectRequest request)
        {
            CompaniesConnectResponse result = new();
            try
            {
                var store = await _wolfApproveRepository.SP_PUT_CONNECT_COMPANIES_REQUEST(supplier_id, request.Company_request_code);
            }
            catch (ApplicationException ex)
            {
                result = new CompaniesConnectResponse()
                {
                    status = new Status()
                    {
                        code = ResponseCode.NotImplement.Text(),
                        message = ResponseCode.NotImplement.Description()
                    },
                    Data = null
                };
                Logger.LogError(ex, "ConnectCompanies", $"supplier_id: {supplier_id} , request: {request}");
            }
            catch (System.Exception ex)
            {
                result = new CompaniesConnectResponse()
                {
                    status = new Status()
                    {
                        code = ResponseCode.InternalServerError.Text(),
                        message = ResponseCode.InternalServerError.Description()
                    },
                    Data = null
                };
                Logger.LogError(ex, "ConnectCompanies", $"supplier_id: {supplier_id} , request: {request}");

            }
            return result;
        }

        public async Task<BaseResponse<List<ClaimResponse>>> GetClaimList(
            string supplier_id,
            string company_id,
            string status,
            string from_date,
            string to_date,
            string page,
            string per_page,
            string order_direction,
            string order_by)
        {
            BaseResponse<List<ClaimResponse>> response = new();
            page = page ?? "1";
            per_page = per_page ?? "10";
            try
            {
                //kick out if company_id is empty
                if (string.IsNullOrEmpty(company_id))
                {
                    response = Utility.PagingCalculator<List<ClaimResponse>>(int.Parse(page), int.Parse(per_page), 0, _baseUrl);
                    response.status = new Status()
                    {
                        code = ResponseCode.BadRequest.Text(),
                        message = ResponseCode.BadRequest.Description()
                    };
                    response.data = null;
                    return response;
                }
                var store = await _wolfApproveRepository.SP_GET_CLAIM_LIST(supplier_id: string.IsNullOrWhiteSpace(supplier_id) ? null : Convert.ToInt16(supplier_id),
                                                                           company_id: string.IsNullOrWhiteSpace(company_id) ? null : Convert.ToInt16(company_id),
                                                                           status_id: string.IsNullOrWhiteSpace(status) ? null : Convert.ToInt16(status),
                                                                           from_date: from_date ?? null,
                                                                           to_date: to_date ?? null);
                if (store.Count != 0)
                {
                    var data = new List<ClaimResponse>();
                    data = [.. store.Select(s => new ClaimResponse{
                            id = s.nClaimID,
                            claim_date = s.dClaimDate,
                            claim_description = s.sClaimDescription,
                            claim_option = s.sClaimOption,
                            claim_reason = s.sClaimReason,
                            claim_return_address = s.sClaimReturnAddress,
                            claim_number = s.sClaimCode,
                            company_id = s.nCompanyID,
                            company_name = s.sCompanyName,
                            create_date = s.dClaimDate,
                            po_number = s.sPOCode,
                            status_id = s.nStatusID,
                            status = s.sStatusName,
                            purchase_date = s.dPOCreatedDate,
                            supplier_id = s.nSupplierID
                        })];
                    response = Utility.PagingCalculator<List<ClaimResponse>>(int.Parse(page), int.Parse(per_page), store.Count, _baseUrl);
                    response.data = data;
                    response.status = new Status()
                    {
                        code = ResponseCode.Success.Text(),
                        message = ResponseCode.Success.Description()
                    };

                }
                else
                {
                    response = new BaseResponse<List<ClaimResponse>>()
                    {
                        status = new Status()
                        {
                            code = ResponseCode.NotFound.Text(),
                            message = ResponseCode.NotFound.Description()
                        }
                    };
                }

            }
            catch (ApplicationException ex)
            {
                Logger.LogError(ex, "GetClaimList");
                response = new BaseResponse<List<ClaimResponse>>()
                {
                    status = new Status()
                    {
                        code = ResponseCode.NotImplement.Text(),
                        message = ResponseCode.NotImplement.Description()
                    }
                };
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "GetClaimList");
                response = new BaseResponse<List<ClaimResponse>>()
                {
                    status = new Status()
                    {
                        code = ResponseCode.InternalServerError.Text(),
                        message = ResponseCode.InternalServerError.Description()
                    }
                };
            }
            return response;
        }

        public async Task<ClaimDetailResponse> GetClaimDetail(string claim_id, string supplier_id)
        {
            ClaimDetailResponse result = new();
            try
            {
                var store = await _wolfApproveRepository.SP_GET_CLAIM_DETAIL(claim_id, supplier_id);
                if (store != null)
                {
                    var data = new ClaimDetailData()
                    {
                        claim_date = store.Claim_date.ToString(),
                        claim_description = store.Claim_description,
                        claim_option = store.Claim_option,
                        claim_reason = store.Claim_reason,
                        code = store.Code,
                        company_name = store.Company_name,
                        create_date = store.Created_date.ToString(),
                        documents = [.. store.Documents.Select(s => new Document()
                        {
                            fileUrl = s.File_url,
                            name = s.Name
                        })],
                        id = store.Id,
                        // Lines = [.. store.Lines.Select(s => new Line()
                        // {
                        //     Description = s.Description,
                        //     Id = s.Id,
                        //     Item_code = s.Item_code,
                        //     Item_name = s.Item_name,
                        //     Line_number = s.Line_number,
                        //     Quantity = s.Quantity,
                        //     Uom_name = s.Uom_name,
                        //     Unit_price = s.Unit_price
                        // })],
                        status = new ClaimStatus()
                        {
                            Name = [.. store.Status.Select(s => s.Name)]
                        },
                        claim_return_address = store.Claim_return_address,
                        purchase_order = new ClaimPurchaseOrderData()
                        {
                            code = store.Purchase_order.Code,
                            purchase_date = store.Purchase_order.Purchase_date
                        }
                    };
                    result = new ClaimDetailResponse()
                    {
                        Data = data,
                        status = new Status()
                        {
                            code = ResponseCode.Success.Text(),
                            message = ResponseCode.Success.Description()
                        }
                    };
                }
                else
                {
                    result = new ClaimDetailResponse()
                    {
                        status = new Status()
                        {
                            code = ResponseCode.NotFound.Text(),
                            message = ResponseCode.NotFound.Description()
                        }
                    };
                }
            }
            catch (ApplicationException ex)
            {
                result = new ClaimDetailResponse()
                {
                    status = new Status()
                    {
                        code = ResponseCode.NotImplement.Text(),
                        message = ResponseCode.NotImplement.Description()
                    }
                };
                Logger.LogError(ex, "GetClaimDetail", $"claim_id: {claim_id} , supplier_id: {supplier_id}");
            }
            catch (System.Exception ex)
            {
                result = new ClaimDetailResponse()
                {
                    status = new Status()
                    {
                        code = ResponseCode.InternalServerError.Text(),
                        message = ResponseCode.InternalServerError.Description()
                    }
                };
                Logger.LogError(ex, "GetClaimDetail", $"claim_id: {claim_id} , supplier_id: {supplier_id}");
            }
            return result;
        }

        public async Task<CompaniesDetailResponse> GetCompaniesDetail(string company_id)
        {
            CompaniesDetailResponse result = new();
            try
            {
                var store = await _wolfApproveRepository.SP_GET_COMPANIES_DETAIL(company_id);
                if (store != null)
                {
                    result.Data = new CompaniesDetailData()
                    {
                        company_address = new CompanyAddress()
                        {
                            address_1 = store.Company_address.Address_1,
                            address_2 = store.Company_address.Address_2,
                            branch = store.Company_address.Branch,
                            district = store.Company_address.District,
                            province = store.Company_address.Province,
                            sub_district = store.Company_address.Sub_district,
                            tax_number = store.Company_address.Tax_id,
                            zip_code = store.Company_address.Zip_code
                        },
                        company_contract = new CompanyContract()
                        {
                            email = store.Company_contact.Email,
                            first_name = store.Company_contact.First_name,
                            last_name = store.Company_contact.Last_name,
                            phone = store.Company_contact.Phone
                        },
                        id = store.Id,
                        name = store.Name,
                        request_date = store.Request_date.ToString(),
                        request_status = store.Request_status,
                        website = store.Website
                    };
                    result.status = new Status()
                    {
                        code = ResponseCode.Success.Text(),
                        message = ResponseCode.Success.Description()
                    };
                }
                else
                {
                    result = new CompaniesDetailResponse()
                    {
                        status = new Status()
                        {
                            code = ResponseCode.NotFound.Text(),
                            message = ResponseCode.NotFound.Description()
                        },
                        Data = null
                    };
                }
            }
            catch (ApplicationException ex)
            {
                Logger.LogError(ex, "GetCompaniesDetail", $"company_id: {company_id}");
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "GetCompaniesDetail", $"company_id: {company_id}");
            }
            return result;
        }

        public async Task<BaseResponse<List<CompaniesResponse>>> GetCompaniesList(string supplier_id)
        {
            BaseResponse<List<CompaniesResponse>> response = new();
            try
            {
                var store = await _wolfApproveRepository.SP_GET_COMPANIES_LIST(supplier_id);
                if (store.Count != 0)
                {
                    response.data = [.. store.Select(s=> new CompaniesResponse{
                        id = s.Id,
                        company_contact =  new CompanyContract(){
                            first_name = s.sContractFirstName,
                            email = s.sContractEmail,
                            last_name = s.sContractLastName,
                            phone = s.sContractPhone
                        },
                        name = s.Name,
                        request_date = s.Request_date,
                        request_status = s.Request_status,

                    })];
                    response.status = new Status
                    {
                        code = ResponseCode.Success.Text(),
                        message = ResponseCode.Success.Description()
                    };
                }
                else
                {
                    response = new BaseResponse<List<CompaniesResponse>>()
                    {
                        status = new Status()
                        {
                            code = ResponseCode.NotFound.Text(),
                            message = ResponseCode.NotFound.Description()
                        },
                        data = null
                    };
                }
            }
            catch (ApplicationException ex)
            {
                Logger.LogError(ex, "GetCompaniesList", $"supplier_id: {supplier_id}");
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "GetCompaniesList", $"supplier_id: {supplier_id}");
            }
            return response;
        }

        public async Task<CountResponse> GetCountClaimPo(string supplier_id)
        {
            CountResponse result = new();
            try
            {
                var store = await _wolfApproveRepository.SP_GET_COUNT_PO_CLAIM();
                result = new CountResponse()
                {
                    Data = new CountData()
                    {
                        count_claim = store.Count_claim.GetValueOrDefault(),
                        count_po = store.Count_po.GetValueOrDefault()
                    },
                    status = new Status()
                    {
                        code = ResponseCode.Success.Text(),
                        message = ResponseCode.Success.Description()
                    }
                };
            }
            catch (ApplicationException ex)
            {
                result = new CountResponse()
                {
                    status = new Status()
                    {
                        code = ResponseCode.NotImplement.Text(),
                        message = ResponseCode.NotImplement.Description()
                    }
                };
                Logger.LogError(ex, "GetCount", $"supplier_id: {supplier_id}");
            }
            catch (System.Exception ex)
            {
                result = new CountResponse()
                {
                    status = new Status()
                    {
                        code = ResponseCode.InternalServerError.Text(),
                        message = ResponseCode.InternalServerError.Description()
                    }
                };
                Logger.LogError(ex, "GetCount", $"supplier_id: {supplier_id}");
            }
            return result;
        }

        public async Task<BaseResponse<List<PurchaseOrderResponse>>> GetPurchaseOrderList(
            string q,
            string supplier_id,
            string number,
            string start_date,
            string end_date,
            string purchase_type_id,
            string status_id,
            string category_id,
            int page,
            int per_page,
            string order_direction,
            string order_by)
        {
            BaseResponse<List<PurchaseOrderResponse>> response = new();
            try
            {
                var store = await _wolfApproveRepository.SP_GET_PURCHASE_ORDER_LIST();
                if (store.Count != 0)
                {
                    page = page <= 0 ? 1 : page;
                    per_page = per_page <= 0 ? 10 : per_page;
                    response = Utility.PagingCalculator<List<PurchaseOrderResponse>>(page, per_page, store.Count, _baseUrl);
                    store = Utility.ItemPerpageCalculator<Domain.Models.WolfApprove.StoreModel.SP_GET_PURCHASE_ORDER>(store, page, per_page);
                    response.data = [.. store.Select(s => new PurchaseOrderResponse()
                    {
                        id = s.nPOID,
                        cancel_description = s.sCancelDesc ?? "",
                        cancel_reason = s.sCancelReason ?? "",
                        catagory_id = s.nCategoryID,
                        category_name = s.sCategoryName,
                        po_number = s.sPOCode,
                        company_contract = new CompanyContract{
                            first_name = s.sContractFirstName,
                            last_name = s.sContractLastName,
                            email = s.sContractEmail,
                            phone = s.sContractPhone
                        },
                        company_id = s.nCompanyID,
                        company_name = s.sCompanyName,
                        project_name = s.sProjectName,
                        description = s.sProjectDesc,
                        net_amount = s.dNetAmount,
                        order_date = s.dOrderDate,
                        payment_condition = s.sPaymentCondition,
                        purchase_type_name = s.sProcurementTypeName,
                        quotation_number = s.sQuotationCode,
                        remark = s.sRemark,
                        require_date = s.dRequireDate,
                        ship_to = s.sShipTo,
                        status_id = s.nStatusID,
                        status = s.sStatusName,
                        rfq_status_id = s.nRFQStatusID,
                        rfq_status_name = s.sRFQStatusName
                    })];
                    response.status = new Status()
                    {
                        code = ResponseCode.Success.Text(),
                        message = ResponseCode.Success.Description()
                    };
                }
                else
                {
                    response = new BaseResponse<List<PurchaseOrderResponse>>()
                    {
                        status = new Status()
                        {
                            code = ResponseCode.NotFound.Text(),
                            message = ResponseCode.NotFound.Description()
                        }
                    };
                }
            }
            catch (ApplicationException ex)
            {
                Logger.LogError(ex, "GetPurchaseOrderList");
                response = new BaseResponse<List<PurchaseOrderResponse>>()
                {
                    status = new Status()
                    {
                        code = ResponseCode.NotImplement.Text(),
                        message = ResponseCode.NotImplement.Description()
                    }
                };
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "GetPurchaseOrderList");
                response = new BaseResponse<List<PurchaseOrderResponse>>()
                {
                    status = new Status()
                    {
                        code = ResponseCode.InternalServerError.Text(),
                        message = ResponseCode.InternalServerError.Description()
                    }
                };
            }
            return response;
        }

        public async Task<PurchaseOrderDetailResponse> GetPurchaseOrderDetail(string order_id, string supplier_id)
        {
            PurchaseOrderDetailResponse response = new();
            try
            {
                if (!string.IsNullOrEmpty(order_id) && !string.IsNullOrEmpty(supplier_id))
                {
                    var result = await _wolfApproveRepository.SP_GET_PURCHASE_ORDER_DETAIL(order_id, supplier_id);
                    if (result != null)
                    {
                        var _temp = result.FirstOrDefault();
                        if (_temp != null)
                        {
                            response.Data = new PurchaseOrderDetailData()
                            {
                                id = _temp.nPOID,
                                cancel_description = _temp.sCancelDesc,
                                cancel_reason = _temp.sCancelReason,
                                category_name = _temp.sCategoryName,
                                po_number = _temp.sPOCode,
                                company_address = new CompanyAddress()
                                {
                                    address_1 = _temp.sAddress1,
                                    address_2 = _temp.sAddress2,
                                    branch = _temp.sBranch,
                                    district = _temp.sDistrict,
                                    province = _temp.sProvince,
                                    sub_district = _temp.sSubDistrict,
                                    tax_number = _temp.sTaxNumber,
                                    zip_code = _temp.sZipCode
                                },
                                company_contract = new CompanyContract()
                                {
                                    email = _temp.sContractEmail,
                                    first_name = _temp.sContractFirstName,
                                    last_name = _temp.sContractLastName,
                                    phone = _temp.sContractPhone
                                },
                                company_id = _temp.nCompanyID,
                                company_name = _temp.sCompanyName,
                                project_name = _temp.sProjectName,
                                description = _temp.sProjectDesc,
                                discount = _temp.dDiscount,
                                order_date = _temp.dOrderDate,
                                request_date = _temp.dRequireDate,
                                payment_condition = _temp.sPaymentCondition,

                                quotation_number = _temp.sQuotationCode,

                                remark = _temp.sRemark,
                                status = _temp.sStatusName,
                                sub_totoal = _temp.dSubtotal,
                                ship_to = _temp.sShipTo,
                                lines = new List<Line>()
                                // net_amount = _temp.dNetAmount,
                                // total_amount = _temp.dTotalAmount,
                                // vat_amount = _temp.dVatAmount

                            };
                            // ItemLine
                            var i = 1;
                            foreach (var item in result)
                            {
                                response.Data.lines.Add(new Line()
                                {
                                    id = item.nLineID,
                                    description = item.sItemDescption,
                                    item_code = item.sItemCode,
                                    item_name = item.sItemName,
                                    line_number = i.ToString(),
                                    quantity = item.nQuantity,
                                    uom_name = item.sItemUomName,
                                    unit_price = item.dUnitPrice
                                });
                                i++;
                            }
                            // Document
                            // var _documnet = xxx 
                            // for 
                            response.status = new Status()
                            {
                                code = ResponseCode.Success.Text(),
                                message = ResponseCode.Success.Description()
                            };
                        }
                        else
                        {
                            response = new PurchaseOrderDetailResponse()
                            {
                                status = new Status()
                                {
                                    code = ResponseCode.NotFound.Text(),
                                    message = ResponseCode.NotFound.Description()
                                },
                                Data = null
                            };
                        }
                    }
                    else
                    {
                        response = new PurchaseOrderDetailResponse()
                        {
                            status = new Status()
                            {
                                code = ResponseCode.NotFound.Text(),
                                message = ResponseCode.NotFound.Description()
                            },
                            Data = null
                        };
                    }
                }
                else
                {
                    response = new PurchaseOrderDetailResponse()
                    {
                        status = new Status()
                        {
                            code = ResponseCode.BadRequest.Text(),
                            message = "กรุณากรอกข้อมูล order_id และ supplier_id"
                        },
                        Data = null
                    };
                }
            }
            catch (ApplicationException ex)
            {
                Logger.LogError(ex, "GetPurchaseOrderShow", $"order_id: {order_id} , supplier_id: {supplier_id}");
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "GetPurchaseOrderShow", $"order_id: {order_id} , supplier_id: {supplier_id}");
            }
            return response;
        }

        public async Task<POCreateResponse> CreatePO(POCreateRequest request, string domain)
        {
            POCreateResponse response = new POCreateResponse();
            DateTime createdDate = DateTime.Now;
            try
            {

                var sqlParameter = new SqlParameter[] {
                                new SqlParameter("@sChannel", _appConfigHelper.GetConfiguration("KubbossChannel"))
                            };

                var configToken = await _dbContext.ExcuteStoreQuerySingleAsync<SP_GET_SYSENDPOINT>("SP_GET_SYSENDPOINT", sqlParameter);

                var endPoint = _appConfigHelper.GetConfiguration("EndPoint:Kubboss");

                var client = HttpClientHelper.CreateClient(endPoint, configToken?.sToken);

                POCreateResponse resCreatePO = await _kubBossService.CreatePOKubboss(client, request);

                if (resCreatePO?.data == null)
                    return resCreatePO;

                if (resCreatePO.data.id == null)
                    return resCreatePO;

                #region ส่งไป upload 
                if (request.attachments != null && request.attachments.Any())
                {
                    foreach (var file in request.attachments)
                    {
                        string contentType = "application/octet-stream";
                        string base64 = file.file_base64;

                        if (!string.IsNullOrWhiteSpace(base64) && base64.StartsWith("data:"))
                        {
                            var parts = base64.Split(',');

                            if (parts.Length == 2)
                            {
                                // data:application/pdf;base64
                                var header = parts[0];

                                contentType = header
                                    .Replace("data:", "")
                                    .Replace(";base64", "");

                                base64 = parts[1];
                            }
                        }
                        var bytes = Convert.FromBase64String(base64);

                        await UploadMedia(resCreatePO.data.id, fileBytes: bytes, fileName: file.file_name, contentType: contentType, uploadForm: "PurchaseOrder");
                    }
                }
                #endregion

                #region เก็บไฟล์ของ base64
                if (request.attachments != null && request.attachments.Any())
                {
                    string rootPath = _config["FileUpload:RootPath"];
                    List<TEMP_RFQ_DOCUMENT> documents = new();
                    foreach (var item in request.attachments)
                    {

                        if (!string.IsNullOrEmpty(item.file_base64))
                        {
                            try
                            {
                                var poFolder = Path.Combine(rootPath, "po", resCreatePO.data.purchase_order_number);

                                if (!Directory.Exists(poFolder))
                                    Directory.CreateDirectory(poFolder);

                                var base64 = item.file_base64;
                                if (base64.Contains(","))
                                {
                                    base64 = base64.Substring(base64.IndexOf(",") + 1);
                                }

                                var bytes = Convert.FromBase64String(base64);

                                var originalFileName = Path.GetFileName(item.file_name ?? "file");

                                foreach (char c in Path.GetInvalidFileNameChars())
                                {
                                    originalFileName = originalFileName.Replace(c, '_');
                                }

                                var extension = Path.GetExtension(originalFileName);

                                if (string.IsNullOrWhiteSpace(extension))
                                {
                                    var fileType = item.file_type ?? "";

                                    if (!fileType.StartsWith("."))
                                    {
                                        fileType = "." + fileType;
                                    }

                                    originalFileName += fileType;
                                }

                                var finalFileName = $"{Path.GetFileNameWithoutExtension(originalFileName)}_" + $"{DateTime.Now:yyyyMMddHHmmss}" + $"{Path.GetExtension(originalFileName)}";
                                var fullPath = Path.Combine(poFolder, finalFileName);
                                await File.WriteAllBytesAsync(fullPath, bytes);

                                var fileUrl = $"{domain}/uploads/po/{resCreatePO.data.purchase_order_number}/{finalFileName}";

                                documents.Add(new TEMP_RFQ_DOCUMENT()
                                {
                                    nRFQID = resCreatePO.data.id.ToString(),
                                    sFileName = finalFileName,
                                    sFilePath = fileUrl,
                                    sFileSeq = item.file_seq,
                                    CreatedBy = "system",
                                });
                            }
                            catch (Exception ex)
                            {
                                Logger.LogError(ex, "CreatePO File", $"request: {JsonConvert.SerializeObject(request)}");
                                continue;
                            }
                        }
                    }
                    var res = await _wolfApproveRepository.SP_INSERT_NEWRFQ_DOCUMENT(documents);
                    Logger.LogInfo("Insert Document", "CreatePO", $"result: {res.Message}");
                }
                #endregion



                return resCreatePO;

            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "CreatePO", $"request: {JsonConvert.SerializeObject(request)}");
            }
            return response;
        }

        public async Task<POCancelResponse> CancelPO(POCancelRequest request)
        {
            POCancelResponse response = new POCancelResponse();
            DateTime createdDate = DateTime.Now;
            try
            {

                var sqlParameter = new SqlParameter[] {
                                new SqlParameter("@sChannel", _appConfigHelper.GetConfiguration("KubbossChannel"))
                            };

                var configToken = await _dbContext.ExcuteStoreQuerySingleAsync<SP_GET_SYSENDPOINT>("SP_GET_SYSENDPOINT", sqlParameter);

                var endPoint = _appConfigHelper.GetConfiguration("EndPoint:Kubboss");

                var client = HttpClientHelper.CreateClient(endPoint, configToken?.sToken);

                var resCreatePO = await _kubBossService.CancelPOKubboss(client, request);

                return resCreatePO;
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "CreatePO", $"request: {JsonConvert.SerializeObject(request)}");
            }
            return response;
        }

        public async Task<QuotationAwardResponse> CreatePOAward(QuotationAwardRequest request)
        {
            QuotationAwardResponse response = new QuotationAwardResponse();
            DateTime createdDate = DateTime.Now;
            try
            {

                var sqlParameter = new SqlParameter[] {
                                new SqlParameter("@sChannel", _appConfigHelper.GetConfiguration("KubbossChannel"))
                            };

                var configToken = await _dbContext.ExcuteStoreQuerySingleAsync<SP_GET_SYSENDPOINT>("SP_GET_SYSENDPOINT", sqlParameter);

                var endPoint = _appConfigHelper.GetConfiguration("EndPoint:Kubboss");

                var client = HttpClientHelper.CreateClient(endPoint, configToken?.sToken);

                var resCreatePO = await _kubBossService.CreatePOAwardKubboss(client, request);

                return resCreatePO;
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "CreatePO", $"request: {JsonConvert.SerializeObject(request)}");
            }
            return response;
        }

        public async Task<BaseResponse<List<RFQDataItem>>> GetRFQ_List(int pageSize, int page, string supplier_id, string company_id, string number, string start_date, string end_date, string purchase_type_id, string request_for_type, string status_id, string category_id, string order_direction, string order_by, string q)
        {
            var result = new BaseResponse<List<RFQDataItem>>();
            try
            {
                List<SP_GET_RFQ_LIST> item = new List<SP_GET_RFQ_LIST>();
                List<SP_GET_RFQ_LIST> itemSpecific = new List<SP_GET_RFQ_LIST>();

                item = await _wolfApproveRepository.SP_GET_RFQ_LIST();
                if (!string.IsNullOrEmpty(supplier_id))
                {
                    itemSpecific = await _wolfApproveRepository.SP_GET_RFQ_LIST_SPECIFIC_BY_SUP_ID(supplier_id);

                    if (itemSpecific.Count != 0)
                    {
                        item.AddRange(itemSpecific);
                    }
                }
                if (item != null && item.Any())
                {
                    // fillter
                    if (!string.IsNullOrEmpty(company_id))
                    {
                        item = [.. item.Where(s => s.nCompanyID == int.Parse(company_id))];
                    }
                    if (!string.IsNullOrEmpty(number))
                    {
                        item = [.. item.Where(s => s.sRFQNumber.Contains(number))];
                    }
                    if (DateTime.TryParse(start_date, out var filterStart) && DateTime.TryParse(end_date, out var filterEnd))
                    {
                        filterStart = filterStart.Date;
                        filterEnd = filterEnd.Date;

                        item = [.. item.Where(s =>
                            s.dStartDate.Date <= filterEnd &&
                            s.dEndDate.Date >= filterStart
                        )];
                    }
                    if (!string.IsNullOrEmpty(purchase_type_id))
                    {
                        item = [.. item.Where(s => s.nProcurementTypeID == int.Parse(purchase_type_id))];
                    }
                    if (!string.IsNullOrEmpty(status_id))
                    {
                        item = [.. item.Where(s => s.nStatusID == int.Parse(status_id))];
                    }
                    if (!string.IsNullOrEmpty(category_id))
                    {
                        item = [.. item.Where(s => s.nCategoryID == int.Parse(category_id))];
                    }
                    if (!string.IsNullOrEmpty(q))
                    {
                        //item = [.. item.Where(s => s.sRFQNumber.Contains(q) || s.sProjectName.Contains(q) || s.sCompanyName.Contains(q))];
                        item = [.. item.Where(s =>
                            (!string.IsNullOrEmpty(s.sRFQNumber) &&
                             s.sRFQNumber.IndexOf(q, StringComparison.OrdinalIgnoreCase) >= 0) ||

                            (!string.IsNullOrEmpty(s.sProjectName) &&
                             s.sProjectName.IndexOf(q, StringComparison.OrdinalIgnoreCase) >= 0) ||

                            (!string.IsNullOrEmpty(s.sCompanyName) &&
                             s.sCompanyName.IndexOf(q, StringComparison.OrdinalIgnoreCase) >= 0)
                        )];
                    }
                    if (!string.IsNullOrEmpty(order_direction))
                    {
                        item = order_direction.ToLower() == "asc" ? item.OrderBy(s => GetRFQRunningNo(s.sRFQNumber)).ToList() : item.OrderByDescending(s => GetRFQRunningNo(s.sRFQNumber)).ToList();
                    }
                    if (!string.IsNullOrEmpty(request_for_type))
                    {
                        item = item.Where(s => !string.IsNullOrEmpty(s.RequestForType) && s.RequestForType.Equals(request_for_type, StringComparison.OrdinalIgnoreCase)).ToList();
                    }
                    page = page <= 0 ? 1 : page;
                    pageSize = pageSize <= 0 ? 10 : pageSize;
                    var dataList = Utility.ItemPerpageCalculator<Domain.Models.WolfApprove.StoreModel.SP_GET_RFQ_LIST>(item, page, pageSize);
                    var data = dataList.Select(s => new RFQDataItem()
                    {
                        category_name = s.sCategoryName,
                        rfq_number = s.sRFQNumber,
                        description = s.sProjectDesc,
                        company_contract = new CompanyContract
                        {
                            email = s.sContractEmail,
                            first_name = s.sContractFirstName,
                            last_name = s.sContractLastName,
                            phone = s.sContractPhone
                        },
                        company_name = s.sCompanyName,
                        contract_value = s.nContractValue,
                        end_date = s.dEndDate,
                        id = s.nRFQID.ToString(),
                        net_amount = s.dNetAmount,
                        payment_condition = s.sPaymentCondition,
                        procurement_type_id = s.nProcurementTypeID,
                        procurement_type_name = s.sProcurementTypeName,
                        project_name = s.sProjectName,
                        remark = s.sRemark,
                        require_date = s.dRequireDate,
                        start_date = s.dStartDate,
                        status = s.sStatusName,
                        is_specific = s.bIsSpecific == true ? "Y" : "N",
                        request_for_type = s.RequestForType,
                        cancel_remark = s.CancelRemark
                    }).ToList();
                    result = Utility.PagingCalculator<List<RFQDataItem>>(page, pageSize, item.Count, _baseUrl);
                    //result.data = [.. data.OrderByDescending(o => o.rfq_number)];
                    result.data = data;
                    result.status = new Status()
                    {
                        code = ResponseCode.Success.Text(),
                        message = ResponseCode.Success.Description()
                    };
                }
                else
                {
                    result.status = new Status()
                    {
                        code = ResponseCode.NotFound.Text(),
                        message = ResponseCode.NotFound.Description()
                    };
                }
            }
            catch (ApplicationException ex)
            {
                result = new BaseResponse<List<RFQDataItem>>()
                {
                    status = new Status()
                    {
                        code = ResponseCode.NotImplement.Text(),
                        message = ResponseCode.NotImplement.Description()
                    },
                    data = null,
                };
                Logger.LogError(ex, "GetRFQ_List");
            }
            catch (System.Exception ex)
            {
                result = new BaseResponse<List<RFQDataItem>>()
                {
                    status = new Status
                    {
                        code = ResponseCode.InternalServerError.Text(),
                        message = ResponseCode.InternalServerError.Description(),

                    },
                    data = null
                };

                Logger.LogError(ex, "GetRFQ_List");
            }
            return result;
        }

        private static int GetRFQRunningNo(string rfqNumber)
        {
            if (string.IsNullOrWhiteSpace(rfqNumber))
                return 0;

            var lastPart = rfqNumber.Split('-').LastOrDefault();

            return int.TryParse(lastPart, out var no)
                ? no
                : 0;
        }

        public async Task<RFQShowResponse> GetRFQ_Show(string rfq_id)
        {
            RFQShowResponse response = new RFQShowResponse();
            try
            {
                var sp_result = await _wolfApproveRepository.SP_GET_RFQ_DETAIL(rfq_id);
                if (sp_result != null && sp_result.Any())
                {
                    var tempList = new RFQShowData();
                    var _companyInfo = sp_result.FirstOrDefault();
                    tempList = new RFQShowData
                    {
                        id = _companyInfo.nRFQID.ToString(),
                        request_for_type = _companyInfo.RequestForType,
                        cancel_remark = _companyInfo.CancelRemark,
                        company_address = new CompanyAddress
                        {
                            address_1 = _companyInfo.sAddress1,
                            address_2 = _companyInfo.sAddress2,
                            branch = _companyInfo.sBranch,
                            district = _companyInfo.sDistrict,
                            province = _companyInfo.sProvince,
                            sub_district = _companyInfo.sSubDistrict,
                            tax_number = _companyInfo.sTaxNumber,
                            zip_code = _companyInfo.sZipCode
                        },
                        company_contract = new CompanyContract
                        {
                            email = _companyInfo.sRequesterEmail,
                            first_name = _companyInfo.sRequesterName,
                            last_name = _companyInfo.sRequesterLastName,
                            phone = _companyInfo.sRequesterTel,
                        },
                        category_name = _companyInfo.sCategoryName,
                        company_id = _companyInfo.nCompanyID,
                        company_name = _companyInfo.sCompanyName,
                        contract_value = _companyInfo.nContractValue,
                        description = _companyInfo.sProjectDesc,
                        discount = _companyInfo.dDiscount,
                        end_date = _companyInfo.dEndDate,
                        net_amount = _companyInfo.dNetAmount,
                        rfq_number = _companyInfo.sRFQNumber,
                        payment_condition = _companyInfo.sPaymentCondition,
                        project_name = _companyInfo.sProjectName,
                        procurement_type_id = _companyInfo.nProcurementTypeID,
                        procurement_type_name = _companyInfo.sProcurementTypeName,
                        remark = _companyInfo.sRemark,
                        require_date = _companyInfo.dRequireDate,
                        start_date = _companyInfo.dStartDate,
                        status = _companyInfo.sStatusName,
                        sub_total = _companyInfo.dSubTotal,
                        total_amount = _companyInfo.dTotalAmount,
                        vat_amount = _companyInfo.dVatAmount,
                        lines = new List<Line>(),
                        documents = new List<Document>(),
                        questionnaire = new List<KubbossCommonModel.Questionnaire>()
                    };
                    int i = 1;
                    foreach (var item in sp_result.OrderBy(s => s.nLineID))
                    {
                        tempList.lines.Add(new Line
                        {
                            description = item.sItemDescption,
                            id = item.nLineID,
                            line_number = i.ToString(),
                            item_code = item.sItemCode,
                            item_name = item.sItemName,
                            item_category = item.sItemCategory,
                            quantity = item.nQuantity,
                            uom_name = item.sItemUomName,
                            unit_price = item.dUnitPrice,
                            total_amount = item.dTotalAmount,
                            vat_amount = item.dVatAmount,
                            vat_rate = item.dVatRate
                        });
                        i++;
                    }

                    var _documnet = await _wolfApproveRepository.SP_GET_RFQ_DOCUMENT(rfq_id);
                    if (_documnet.Any())
                    {
                        foreach (var item in _documnet)
                        {
                            tempList.documents.Add(new Document
                            {
                                fileUrl = item.sFilePath,
                                name = item.sFileName
                            });
                        }
                    }

                    var questionnaire = await _wolfApproveRepository.SP_GET_RFQ_QUESTIONNAIRE_BY_RFQID(rfq_id);
                    if (questionnaire.Any())
                    {
                        foreach (var item in questionnaire)
                        {
                            tempList.questionnaire.Add(new KubbossCommonModel.Questionnaire
                            {
                                question_id = item.nQuestion_ID,
                                question = item.sQuestion,
                                question_number = item.nQuestionNumber,
                                answer = item.sAnswer,
                                code = item.sCode
                            });
                        }
                    }

                    response = new RFQShowResponse()
                    {
                        status = new Status()
                        {
                            code = ResponseCode.Success.Text(),
                            message = ResponseCode.Success.Text()
                        },
                        data = tempList
                    };
                }
                else
                {
                    response = new RFQShowResponse()
                    {
                        status = new Status()
                        {
                            code = ResponseCode.NotFound.Text(),
                            message = ResponseCode.NotFound.Text()
                        }
                    };
                }
            }
            catch (System.Exception ex)
            {
                response = new RFQShowResponse()
                {
                    status = new Status()
                    {
                        code = ResponseCode.InternalServerError.Text(),
                        message = ResponseCode.InternalServerError.Description()
                    }
                };
                Logger.LogError(ex, "GetRFQ_Show");
            }
            return response;
        }

        public async Task<RFQCreateResponse> CreateAndUpdateRFQ(RFQCreateRequest request, string domain)
        {
            RFQCreateResponse response = new();
            DateTime createdDate = DateTime.Now;
            try
            {
                var _companyList = await _masterDataRepository.SP_GET_MASTER_COMPANY(isShowAll: true);
                var companyData = _companyList.Where(e => e.nCompanyID == request.company_id).FirstOrDefault();
                if (!string.IsNullOrEmpty(request.rfq_id))
                {
                    string rootPath = _config["FileUpload:RootPath"];
                    List<RFQUpdateDocument> document = new List<RFQUpdateDocument>();
                    if (request.attachments != null && request.attachments.Any())
                    {
                        var rfqFolder = Path.Combine(rootPath, "rfq", request.rfq_number);
                        if (Directory.Exists(rfqFolder))
                        {
                            Directory.Delete(rfqFolder, true);
                        }
                        Directory.CreateDirectory(rfqFolder);

                        foreach (var attach in request.attachments.OrderBy(o => o.file_seq))
                        {
                            #region เก็บไฟล์ของ base64
                            if (!string.IsNullOrEmpty(attach.file_base64) && string.IsNullOrEmpty(attach.file_path))
                            {

                                var base64 = attach.file_base64;
                                if (base64.Contains(","))
                                {
                                    base64 = base64.Substring(base64.IndexOf(",") + 1);
                                }

                                var bytes = Convert.FromBase64String(base64);

                                var originalFileName = Path.GetFileName(attach.file_name);

                                foreach (char c in Path.GetInvalidFileNameChars())
                                {
                                    originalFileName = originalFileName.Replace(c, '_');
                                }

                                var extension = Path.GetExtension(originalFileName);

                                if (string.IsNullOrWhiteSpace(extension))
                                {
                                    var fileType = attach.file_type ?? "";

                                    if (!fileType.StartsWith("."))
                                    {
                                        fileType = "." + fileType;
                                    }

                                    originalFileName += fileType;
                                }

                                var finalFileName = $"{Path.GetFileNameWithoutExtension(originalFileName)}_" + $"{DateTime.Now:yyyyMMddHHmmss}" + $"{Path.GetExtension(originalFileName)}";
                                var fullPath = Path.Combine(rfqFolder, finalFileName);

                                await File.WriteAllBytesAsync(fullPath, bytes);

                                var fileUrl = $"{domain}/uploads/rfq/{request.rfq_number}/{finalFileName}";

                                document.Add(new RFQUpdateDocument
                                {
                                    file_seq = attach.file_seq,
                                    file_name = finalFileName,
                                    file_path = fileUrl
                                });
                            }
                            #endregion
                            else
                            {

                                document.Add(new RFQUpdateDocument
                                {
                                    file_seq = attach.file_seq,
                                    file_name = attach.file_name,
                                    file_path = attach.file_path
                                });
                            }
                        }
                    }

                    var update_rfq_response = await UpdateRFQ(new RFQUpdateRequest
                    {
                        rfq_id = request.rfq_id,
                        end_date = request.end_date,
                        start_date = request.start_date,
                        documents = document,
                        modified_by = request.created_by
                    });

                    response = new RFQCreateResponse()
                    {
                        status = update_rfq_response.status,
                    };

                    if (update_rfq_response.data != null)
                    {
                        response.data = new RFQCreateData
                        {
                            rfq_id = update_rfq_response.data.rfq_id,
                            company_id = companyData.nCompanyID,
                            company_name = companyData.sCompanyName,
                            created_date = DateTime.Now,
                            rfq_number = request.rfq_number
                        };
                    }
                    return response;

                }

                var _pocurement_type = await _masterDataRepository.SP_GET_MASTER_PROCUREMENTTYPE(isShowAll: true);
                var _catagory = await _masterDataRepository.SP_GET_MASTER_CATEGORY(isShowAll: true);


                if (companyData == null)
                {
                    response = new RFQCreateResponse
                    {
                        status = new Status()
                        {
                            code = ResponseCode.Unprocessable.Text(),
                            message = "ไม่พบข้อมูล Company ที่ระบุ"
                        },
                        data = null
                    };
                    return response;
                }
                var procurementData = _pocurement_type.Where(e => e.nProcurementTypeID == request.procurement_type_id).FirstOrDefault();
                if (procurementData == null)
                {
                    response = new RFQCreateResponse
                    {
                        status = new Status()
                        {
                            code = ResponseCode.Unprocessable.Text(),
                            message = "ไม่พบข้อมูล Procurement Type ที่ระบุ"
                        },
                        data = null
                    };
                    return response;
                }
                var catagotyData = _catagory.Where(e => e.nCategoryID == request.procurement_category_id).FirstOrDefault();
                if (catagotyData == null)
                {
                    response = new RFQCreateResponse
                    {
                        status = new Status()
                        {
                            code = ResponseCode.Unprocessable.Text(),
                            message = "ไม่พบข้อมูล Catagory ที่ระบุ"
                        },
                        data = null
                    };
                    return response;
                }

                string statusName = "Pending";
                if (request.start_date <= DateTime.Now)
                {
                    statusName = "Open";
                }

                string sup_id = string.Empty;
                if (request.supplier_id.Count != 0)
                    sup_id = string.Join(",", request.supplier_id);

                var chkRevision = request.revision;
                var projectName = request.project_name;

                if (chkRevision > 0)
                {
                    projectName += $" (ต่อรองครั้งที่ {request.revision})";
                }

                var result = await _wolfApproveRepository.SP_INSERT_NEWRFQ(
                        request.rfq_number,
                        company_id: companyData.nCompanyID,
                        company_name: companyData.sCompanyName,
                        rfq_status: statusName,
                        request.sub_total,
                        request.discount,
                        request.total_amount,
                        request.net_amount,
                        request.payment_condition,
                        projectName,
                        request.project_description,
                        request.procurement_type_id ?? 0,
                        procurement_type_name: procurementData.sProcurementTypeName,
                        catagory_id: request.procurement_category_id ?? 0,
                        category_name: catagotyData.sCategoryName,
                        start_date: request.start_date,
                        end_date: request.end_date,
                        required_date: request.required_date,
                        status_id: 1,
                        status_name: statusName,
                        request.contract_value,
                        request.remark,
                        requesterName: request.requester?.requesterName,
                        requesterLastName: request.requester?.requesterLastName,
                        requesterEmail: request.requester?.requesterEmail,
                        requesterTel: request.requester?.requesterTel,
                        request.created_by,
                        string.IsNullOrEmpty(request.is_specific) ? "N" : request.is_specific,
                        string.IsNullOrEmpty(sup_id) ? "" : sup_id,
                        requestForType: "RFQ",
                        revision: request.revision
                    );
                // Check if the RFQ was created successfully
                if (result.Result == true)
                {
                    // Store Insert Item
                    if (request.items != null && request.items.Any())
                    {
                        List<TEMP_RFQ_ITEM> itemList = new();
                        foreach (var item in request.items)
                        {
                            itemList.Add(new TEMP_RFQ_ITEM()
                            {
                                sRFQID = result.RFQID?.ToString(),
                                nLineID = item.line_id,
                                sItemCode = item.item_code,
                                sItemName = item.item_name,
                                sItemUomName = item.item_uom_name,
                                sItemDescption = item.item_descption,
                                sItemCategory = item.item_category,
                                nQuantity = item.quantity,
                                dUnitPrice = item.unit_price,
                                dVatRate = item.vat_rate,
                                dVatAmount = item.vat_amount,
                                dTotalAmount = item.total_amount,
                                IsActive = true,
                                CreatedBy = request.created_by,
                            });
                        }
                        var res = await _wolfApproveRepository.SP_INSERT_NEWREQ_ITEMLINES(itemList);
                        Logger.LogInfo("Insert Item", "CreateRFQ", $"result: {res.Message}");

                    }
                    if (request.questionaires != null && request.questionaires.Any())
                    {
                        List<TEMP_RFQ_QUESTIONNAIRE> questionnaireList = new();

                        foreach (var item in request.questionaires)
                        {
                            questionnaireList.Add(new TEMP_RFQ_QUESTIONNAIRE()
                            {
                                nRFQID = result.RFQID?.ToString(),
                                sQuestionNumber = item.questionnaire_number,
                                sQuestion = item.questionnaire_detail,
                                sAnswer = null,
                                sCode = item.code
                            });
                        }
                        var res = await _wolfApproveRepository.SP_INSERT_NEWRFQ_QUESTIONNAIRE(questionnaireList);
                        Logger.LogInfo("Insert Questionnaire", "CreateRFQ", $"result: {res.Message}");

                    }
                    if (request.attachments != null && request.attachments.Any())
                    {
                        string rootPath = _config["FileUpload:RootPath"];
                        List<TEMP_RFQ_DOCUMENT> documents = new();
                        foreach (var item in request.attachments)
                        {
                            #region เก็บไฟล์ของ base64
                            if (!string.IsNullOrEmpty(item.file_base64))
                            {
                                try
                                {
                                    var rfqFolder = Path.Combine(rootPath, "rfq", request.rfq_number);

                                    if (!Directory.Exists(rfqFolder))
                                        Directory.CreateDirectory(rfqFolder);

                                    var base64 = item.file_base64;
                                    if (base64.Contains(","))
                                    {
                                        base64 = base64.Substring(base64.IndexOf(",") + 1);
                                    }

                                    var bytes = Convert.FromBase64String(base64);

                                    var originalFileName = Path.GetFileName(item.file_name ?? "file");

                                    foreach (char c in Path.GetInvalidFileNameChars())
                                    {
                                        originalFileName = originalFileName.Replace(c, '_');
                                    }

                                    var extension = Path.GetExtension(originalFileName);

                                    if (string.IsNullOrWhiteSpace(extension))
                                    {
                                        var fileType = item.file_type ?? "";

                                        if (!fileType.StartsWith("."))
                                        {
                                            fileType = "." + fileType;
                                        }

                                        originalFileName += fileType;
                                    }

                                    var finalFileName = $"{Path.GetFileNameWithoutExtension(originalFileName)}_" + $"{DateTime.Now:yyyyMMddHHmmss}" + $"{Path.GetExtension(originalFileName)}";
                                    var fullPath = Path.Combine(rfqFolder, finalFileName);
                                    await File.WriteAllBytesAsync(fullPath, bytes);

                                    var fileUrl = $"{domain}/uploads/rfq/{request.rfq_number}/{finalFileName}";

                                    documents.Add(new TEMP_RFQ_DOCUMENT()
                                    {
                                        nRFQID = result.RFQID?.ToString(),
                                        sFileName = finalFileName,
                                        sFilePath = fileUrl,
                                        sFileSeq = item.file_seq,
                                        CreatedBy = request.created_by,
                                    });
                                }
                                catch (Exception ex)
                                {
                                    Logger.LogError(ex, "CreateRFQ File", $"request: {JsonConvert.SerializeObject(request)}");
                                    continue;
                                }
                            }
                            #endregion 
                            else
                            {
                                documents.Add(new TEMP_RFQ_DOCUMENT()
                                {
                                    nRFQID = result.RFQID?.ToString(),
                                    sFileName = item.file_name,
                                    sFilePath = item.file_path,
                                    sFileSeq = item.file_seq,
                                    CreatedBy = request.created_by,
                                });
                            }
                        }
                        var res = await _wolfApproveRepository.SP_INSERT_NEWRFQ_DOCUMENT(documents);
                        Logger.LogInfo("Insert Document", "CreateRFQ", $"result: {res.Message}");
                    }
                    response = new RFQCreateResponse()
                    {
                        status = new Status()
                        {
                            code = ResponseCode.Success.Text(),
                            message = ResponseCode.Success.Description()
                        },
                        data = new RFQCreateData()
                        {
                            rfq_id = result.RFQID.ToString(),
                            rfq_number = request.rfq_number,
                            company_id = companyData.nCompanyID,
                            company_name = companyData.sCompanyName,
                            created_date = DateTime.Now,
                        }
                    };

                    //ส่งเมลแจ้งเตือน New RFQ
                    #region send mail new RFQ
                    var sqlParameter = new SqlParameter[] {
                                new SqlParameter("@sChannel", _appConfigHelper.GetConfiguration("KubbossChannel"))
                            };

                    var configToken = await _dbContext.ExcuteStoreQuerySingleAsync<SP_GET_SYSENDPOINT>("SP_GET_SYSENDPOINT", sqlParameter);

                    var endPoint = _appConfigHelper.GetConfiguration("EndPoint:Kubboss");

                    var client = HttpClientHelper.CreateClient(endPoint, configToken?.sToken);

                    var defaultLang = "TH";
                    await SendEmailNotify(client, request.supplier_id, result.RFQID?.ToString(), defaultLang);

                    #endregion
                }
                else
                {
                    Logger.LogError(new Exception(result.Message), "CreateRFQ", $"request: {JsonConvert.SerializeObject(request)}");
                    response = new RFQCreateResponse()
                    {
                        status = new Status()
                        {
                            code = ResponseCode.Unprocessable.Text(),
                            message = result.Message
                        },
                        data = null
                    };

                }

            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "CreateRFQ", $"request: {JsonConvert.SerializeObject(request)}");
                response = new RFQCreateResponse()
                {
                    status = new Status()
                    {
                        code = ResponseCode.InternalServerError.Text(),
                        message = ResponseCode.InternalServerError.Description()
                    },
                    data = null
                };
            }
            return response;
        }

        public async Task<RFQUpdateResponse> UpdateRFQ(RFQUpdateRequest request)
        {
            RFQUpdateResponse response = new();
            try
            {
                var verify = await _wolfApproveRepository.SP_GET_RFQ_DETAIL(request.rfq_id);
                if (verify == null)
                {
                    response = new RFQUpdateResponse()
                    {
                        status = new Status()
                        {
                            code = ResponseCode.NotFound.Text(),
                            message = ResponseCode.NotFound.Description()
                        },
                        data = null
                    };
                    return response;
                }
                else
                {
                    var doc = new List<TEMP_RFQ_DOCUMENT>();
                    foreach (var item in request.documents)
                    {
                        doc.Add(new TEMP_RFQ_DOCUMENT
                        {
                            nRFQID = request.rfq_id,
                            sFileName = item.file_name,
                            sFilePath = item.file_path,
                            sFileSeq = item.file_seq
                        });
                    }

                    var result = await _wolfApproveRepository.SP_UPDATE_RFQ(doc, request.rfq_id, request.start_date, request.end_date, request.modified_by);
                    if (result.result)
                    {
                        response = new RFQUpdateResponse()
                        {
                            status = new Status()
                            {
                                code = ResponseCode.Success.Text(),
                                message = ResponseCode.Success.Description()
                            },
                            data = new RFQUpdateData()
                            {
                                rfq_id = request.rfq_id,
                                update_response = "Successful"
                            }
                        };
                    }
                    else
                    {
                        response = new RFQUpdateResponse()
                        {
                            status = new Status()
                            {
                                code = ResponseCode.Unprocessable.Text(),
                                message = ResponseCode.Unprocessable.Description(),
                            },
                            data = null
                        };
                    }
                }
            }
            catch (System.Exception ex)
            {
                response = new RFQUpdateResponse()
                {
                    status = new Status()
                    {
                        code = ResponseCode.InternalServerError.Text(),
                        message = ResponseCode.InternalServerError.Description()
                    },
                    data = null
                };
                Logger.LogError(ex, "UpdateRFQ");
            }
            return response;
        }

        public async Task<BaseResponse> PutQuotation(string rfq_id, PutQuotationRequest request)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                var verify = await _wolfApproveRepository.SP_GET_RFQ_DETAIL(rfq_id);
                if (verify == null || !verify.Any())
                {
                    response = new BaseResponse()
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
                    if (request.status.ToLower() == "create")
                    {
                        var result = await _wolfApproveRepository.SP_PUT_QUOTATION_CREATE(rfq_id, request.quo_number, request.quo_id, request.supplier_id, request.status, request.reason);
                        if (result.result)
                        {
                            response = new BaseResponse()
                            {
                                status = new Status()
                                {
                                    code = ResponseCode.Success.Text(),
                                    message = ResponseCode.Success.Description()
                                }
                            };

                            var routes = await _wolfApproveRepository.SP_GET_Buyer_Code(request.buyerCode);
                            var route = routes.FirstOrDefault(x => x.ActionType == "CREATE_QUOTATION");
                            Logger.LogInfo("PutQuotation", $"RouteFound:{(route != null)} | Buyer:{request.buyerCode}");


                            var sqlParameter = new SqlParameter[] {
                                new SqlParameter("@sChannel", _appConfigHelper.GetConfiguration("KubbossChannel"))
                            };

                            var configToken = await _dbContext.ExcuteStoreQuerySingleAsync<SP_GET_SYSENDPOINT>("SP_GET_SYSENDPOINT", sqlParameter);

                            var endPoint = _appConfigHelper.GetConfiguration("EndPoint:Kubboss");

                            var client = HttpClientHelper.CreateClient(endPoint, configToken?.sToken);

                            var resQuotation = await _kubBossService.GetQuotationDetail(client, request.quo_id);

                            var resSuppliers = await _kubBossService.GetSuppliersDetail(client, request.supplier_id);
                            var name = resSuppliers["data"]?["name"]?.ToString();
                            var supplier_email = resSuppliers["data"]?["supplier_email"]?.ToString();

                            var data = resQuotation["data"];

                            if (data != null)
                            {
                                data["created_by"] = new JObject
                                {
                                    ["email"] = supplier_email,
                                    ["name"] = name,
                                    ["surname"] = "",

                                };

                                var currency = string.IsNullOrWhiteSpace(data["currency"]?.ToString()) ? "THB" : data["currency"].ToString();

                                var revision = verify.FirstOrDefault()?.Revision;
                                data["revision"] = revision;


                                var lines = data["lines"] as JArray;

                                if (lines != null)
                                {
                                    foreach (JObject line in lines)
                                    {
                                        line["currency"] = currency;
                                    }
                                }
                            }

                            var payload = new JObject
                            {
                                ["data"] = data
                            };

                            Logger.LogInfo("PutQuotation", "CreateRFQ", $"result: {JsonConvert.SerializeObject(payload)}");


                            if (data == null)
                                throw new Exception("Quotation data not found");

                            if (route != null)
                            {
                                await _buyerApiService.SendToBuyer(route, JsonConvert.SerializeObject(payload));

                            }

                        }
                        else
                        {
                            response = new BaseResponse()
                            {
                                status = new Status()
                                {
                                    code = ResponseCode.Unprocessable.Text(),
                                    message = result.message
                                }
                            };
                        }
                    }
                    else if (request.status.ToLower() == "decline")
                    {
                        response = new BaseResponse()
                        {
                            status = new Status()
                            {
                                code = ResponseCode.Success.Text(),
                                message = ResponseCode.Success.Description()
                            }
                        };
                        try
                        {

                            var routes = await _wolfApproveRepository.SP_GET_Buyer_Code(request.buyerCode);
                            var route = routes.FirstOrDefault(x => x.ActionType == "UPDATE_QUOTATION");

                            var sqlParameter = new SqlParameter[] {
                                new SqlParameter("@sChannel", _appConfigHelper.GetConfiguration("KubbossChannel"))
                            };

                            var configToken = await _dbContext.ExcuteStoreQuerySingleAsync<SP_GET_SYSENDPOINT>("SP_GET_SYSENDPOINT", sqlParameter);

                            var endPoint = _appConfigHelper.GetConfiguration("EndPoint:Kubboss");

                            var client = HttpClientHelper.CreateClient(endPoint, configToken?.sToken);

                            var resSuppliers = await _kubBossService.GetSuppliersDetail(client, request.supplier_id);
                            var name = resSuppliers["data"]?["name"]?.ToString();
                            var supplier_email = resSuppliers["data"]?["supplier_email"]?.ToString();

                            if (route != null)
                            {

                                var rfq_data = verify.FirstOrDefault();
                                var payload = new RFQUpdateStatus
                                {
                                    doc_number = rfq_data.sRFQNumber,
                                    supplier_name = name,
                                    supplier_email = supplier_email,
                                    status = "ไม่เข้าร่วม",
                                    reason = request.reason
                                };

                                Logger.LogInfo("SendToBuyer", "CreateRFP", $"result: {JsonConvert.SerializeObject(payload)}");

                                await _buyerApiService.SendToBuyer(route, JsonConvert.SerializeObject(payload));

                            }
                            else
                            {
                                response = new BaseResponse()
                                {
                                    status = new Status()
                                    {
                                        code = ResponseCode.Unprocessable.Text(),
                                        message = "Update Failed"
                                    }
                                };
                            }
                        }
                        catch (System.Exception ex)
                        {
                            response = new BaseResponse()
                            {
                                status = new Status()
                                {
                                    code = ResponseCode.InternalServerError.Text(),
                                    message = ResponseCode.InternalServerError.Description()
                                }
                            };
                            Logger.LogError(ex, "PutQuotation", $"rfq_id: {rfq_id}");
                        }
                    }
                    else
                    {
                        var result = await _wolfApproveRepository.SP_PUT_QUOTATION(rfq_id, request.quo_number, request.quo_id, request.status, request.reason);
                        if (result.result)
                        {
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
                                    code = ResponseCode.NotImplement.Text(),
                                    message = result.message
                                }
                            };
                        }

                        if (request.status.ToLower() == "cancel")
                        {
                            try
                            {
                                var routes = await _wolfApproveRepository.SP_GET_Buyer_Code(request.buyerCode);
                                var route = routes.FirstOrDefault(x => x.ActionType == "UPDATE_QUOTATION");

                                var sqlParameter = new SqlParameter[] {
                                new SqlParameter("@sChannel", _appConfigHelper.GetConfiguration("KubbossChannel"))};

                                var configToken = await _dbContext.ExcuteStoreQuerySingleAsync<SP_GET_SYSENDPOINT>("SP_GET_SYSENDPOINT", sqlParameter);

                                var endPoint = _appConfigHelper.GetConfiguration("EndPoint:Kubboss");

                                var client = HttpClientHelper.CreateClient(endPoint, configToken?.sToken);

                                var resSuppliers = await _kubBossService.GetSuppliersDetail(client, request.supplier_id);
                                var name = resSuppliers["data"]?["name"]?.ToString();
                                var supplier_email = resSuppliers["data"]?["supplier_email"]?.ToString();

                                if (route != null)
                                {

                                    var rfq_data = verify.FirstOrDefault();
                                    var payload = new RFQUpdateStatus
                                    {
                                        doc_number = rfq_data.sRFQNumber,
                                        supplier_name = name,
                                        supplier_email = supplier_email,
                                        status = "ไม่เข้าร่วม",
                                        reason = request.reason,
                                        cancel = true
                                    };

                                    Logger.LogInfo("SendToBuyer", "CreatRFP", $"result: {JsonConvert.SerializeObject(payload)}");

                                    await _buyerApiService.SendToBuyer(route, JsonConvert.SerializeObject(payload));
                                }
                            }
                            catch (System.Exception ex)
                            {
                                response = new BaseResponse()
                                {
                                    status = new Status()
                                    {
                                        code = ResponseCode.Unprocessable.Text(),
                                        message = ex.Message
                                    }
                                };

                                Logger.LogError(ex, "PutQuotation", $"rfq_id: {rfq_id}");

                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                response = new BaseResponse()
                {
                    status = new Status()
                    {
                        code = ResponseCode.InternalServerError.Text(),
                        message = ResponseCode.InternalServerError.Description()
                    }
                };
                Logger.LogError(ex, "PutQuotation", $"rfq_id: {rfq_id}");
            }
            return response;
        }
        public async Task<QuotationResponse> GetQuotation(string supplier_id, string rfq_id)
        {
            QuotationResponse response = new QuotationResponse();
            try
            {
                var _client = await _wolfApproveRepository.SP_GET_RFQ_DETAIL(rfq_id);
            }
            catch (System.Exception ex)
            {
                response = new QuotationResponse()
                {
                    status = new Status()
                    {
                        code = ResponseCode.InternalServerError.Text(),
                        message = ResponseCode.InternalServerError.Description()
                    }
                };
                Logger.LogError(ex, "GetRFQ_Show");
            }
            return response;
        }

        public async Task<RFQShowResponse> GetBuyerCode(string BuyerCode)
        {
            RFQShowResponse response = new RFQShowResponse();

            try
            {
                var sp_result = await _wolfApproveRepository.SP_GET_Buyer_Code(BuyerCode);

            }
            catch (System.Exception ex)
            {
                response = new RFQShowResponse()
                {
                    status = new Status()
                    {
                        code = ResponseCode.InternalServerError.Text(),
                        message = ResponseCode.InternalServerError.Description()
                    }
                };
                Logger.LogError(ex, "GetBuyerCode");
            }

            return response;
        }

        public async Task<VendorRegisterResponse> CreateSupplierRegistration(string companyID, SupplierRegistrationResponse responseSuppliers)
        {
            VendorRegisterResponse response = new VendorRegisterResponse();

            try
            {
                var _companyAPI = await _wolfApproveRepository.SP_GET_COMPANY_API(companyID);
                var companyInfo = _companyAPI.FirstOrDefault();
                if (companyInfo == null)
                {
                    response.status = new Status()
                    {
                        code = ResponseCode.NotFound.Text(),
                        message = "Company API not found"
                    };
                    return response;
                }

                var vendorData = new VendorRegisterData
                {
                    CompanyID = companyInfo.CompanyID,
                    CompanyName = companyInfo.CompanyName,
                    EndpointAPI = companyInfo.EndpointAPI,
                    KB_API_PATH = companyInfo.KB_API_PATH,
                    URLCreatedDate = companyInfo.URLCreatedDate,
                    WOLF_API_PATH = companyInfo.ApiPath,
                    PathCreatedDate = companyInfo.PathCreatedDate,
                    Model = companyInfo.Model,
                };

                string apiPath = companyInfo.ApiPath;
                string url = $"{companyInfo.EndpointAPI}{apiPath}";
                string AccessToken = "";
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(companyInfo.EndpointAPI);
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", AccessToken);

                    var jsonContent = JsonConvert.SerializeObject(responseSuppliers);
                    var httpContent = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

                    var result = await client.PostAsync(url, httpContent);

                    if (!result.IsSuccessStatusCode)
                    {
                        response.status = new Status()
                        {
                            code = ResponseCode.BadRequest.Text(),
                            message = $"Failed to forward supplier data to {url}"
                        };
                        response.data = vendorData;
                        return response;
                    }
                }

                response.status = new Status()
                {
                    code = ResponseCode.Success.Text(),
                    message = ResponseCode.Success.Description()
                };

                response.data = vendorData;
            }
            catch (System.Exception ex)
            {
                response = new VendorRegisterResponse()
                {
                    status = new Status()
                    {
                        code = ResponseCode.InternalServerError.Text(),
                        message = ResponseCode.InternalServerError.Description()
                    }
                };

                Logger.LogError(ex, "Vendor Register");
            }

            return response;
        }

        #region RFP
        public async Task<RFPCreateResponse> CreateRFP(string documentNo, RFPCreateRequest request, int company_id, List<IFormFile> files, string domain)
        {
            RFPCreateResponse response = new();

            try
            {
                DateTime today = DateTime.Now;
                DateTime endDate = DateTime.Now.AddMonths(6);
                var _companyList = await _masterDataRepository.SP_GET_MASTER_COMPANY(isShowAll: true);
                var companyData = _companyList.Where(e => e.nCompanyID == company_id).FirstOrDefault();

                if (request != null)
                {
                    string sup_id = string.Empty;
                    if (request.supplier_id.Count != 0)
                        sup_id = string.Join(",", request.supplier_id);

                    var result = await _wolfApproveRepository.SP_INSERT_NEWRFQ(
                        documentNo,
                        company_id: companyData.nCompanyID,
                        company_name: companyData.sCompanyName,
                        rfq_status: "",
                        sub_total: 0,
                        discount: 0,
                        total_amount: 0,
                        net_amount: 0,
                        payment_condition: "",
                        project_name: "",
                        project_description: "",
                        procurement_tyepe_id: 0,
                        procurement_type_name: "",
                        catagory_id: 0,
                        category_name: "",
                        today,
                        endDate,
                        required_date: today,
                        status_id: 2,
                        status_name: "Open",
                        contract_value: 0,
                        remark: "",
                        requesterName: string.IsNullOrWhiteSpace(request.RequesterName) ? null : request.RequesterName,
                        requesterLastName: request.RequesterLastName,
                        requesterEmail: request.RequesterEmail,
                        requesterTel: request.RequesterTel,
                        "System",
                        "Y",
                        string.IsNullOrEmpty(sup_id) ? "" : sup_id,
                        requestForType: "RFP",
                        revision: 0
                    );


                    string rootPath = _config["FileUpload:RootPath"];
                    string publicPath = _config["FileUpload:PublicPath"];
                    string folderPath = Path.Combine(rootPath, "rfp", documentNo);

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    List<RFQUpdateDocument> document = new List<RFQUpdateDocument>();
                    List<TEMP_RFQ_DOCUMENT> documents = new();
                    int Fileseq = 1;
                    var savedFiles = new List<string>();
                    foreach (var f in files)
                    {
                        if (f.Length == 0) continue;
                        string shortGuid = Guid.NewGuid().ToString("N").Substring(0, 12);

                        string fileName = Path.GetFileName(f.FileName);
                        string formattedDate = today.ToString("yyyyMMdd");
                        string newFileName = $"{shortGuid}_{formattedDate}_{fileName}";
                        string fullPath = Path.Combine(folderPath, newFileName);

                        using (var stream = new FileStream(fullPath, FileMode.Create))
                        {
                            await f.CopyToAsync(stream);
                        }

                        savedFiles.Add(fullPath);
                        string fileURL = $"{domain}/uploads/rfp/{documentNo}/{newFileName}";

                        documents.Add(new TEMP_RFQ_DOCUMENT()
                        {
                            nRFQID = result.RFQID?.ToString(),
                            sFileName = newFileName,
                            sFilePath = fileURL,
                            sFileSeq = Fileseq,
                            CreatedBy = "SYSTEM",
                        });

                        Fileseq++;

                    }

                    var res = await _wolfApproveRepository.SP_INSERT_NEWRFQ_DOCUMENT(documents);
                    Logger.LogInfo("Insert Document", "CreateRFP", $"result: {res.Message}");

                    if (!res.Result)
                    {
                        foreach (var path in savedFiles)
                        {
                            try
                            {
                                if (System.IO.File.Exists(path))
                                    System.IO.File.Delete(path);
                            }
                            catch (Exception ex)
                            {
                                Logger.LogError(ex, "DeleteFileRollback");
                            }
                        }
                        throw new Exception($"Insert DB failed: {res.Message}");
                    }

                }

                response.data = new RFPCreateData()
                {
                    documentNo = documentNo
                };

                response.status = new Status()
                {
                    code = ResponseCode.Success.Text(),
                    message = "Upload Success"
                };

            }
            catch (Exception ex)
            {
                response = new RFPCreateResponse()
                {
                    status = new Status()
                    {
                        code = ResponseCode.InternalServerError.Text(),
                        message = ex.Message
                    },
                    data = null
                };

                Logger.LogError(ex, "CreateRFP");
            }

            return response;
        }

        public async Task<AcknowledgeDocumentResponse> AcknowledgeDocument(AcknowledgeDocumentRequest request)
        {
            AcknowledgeDocumentResponse response = new();

            try
            {
                var rfqInfo = await _wolfApproveRepository.SP_GET_RFQ_DETAIL(request.rfq_id);

                if (rfqInfo != null && rfqInfo.Any())
                {
                    var _dataInfo = rfqInfo.FirstOrDefault();

                    response.data = new AcknowledgeDocumentData()
                    {
                        rfq_id = _dataInfo.nRFQID.ToString(),
                        documentNo = _dataInfo.sRFQNumber,
                        name = request.name,
                        supplier_id = request.supplier_id,
                        supplier_email = request.supplier_email,
                        acknowledgeDate = request.acknowledgeDate,
                    };

                    var sqlParameter = new SqlParameter[]
                    {
                        new SqlParameter("@sChannel", _appConfigHelper.GetConfiguration("KubbossChannel").ToString()),
                    };
                    var configToken = await _dbContext.ExcuteStoreQuerySingleAsync<SP_GET_SYSENDPOINT>("SP_GET_SYSENDPOINT", sqlParameter);
                    var endPoint = _appConfigHelper.GetConfiguration("EndPoint:Mahidol").ToString();
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri(endPoint);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {configToken.sToken}");
                    var data = new List<QuotationData>();

                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(response.data);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var result = await client.PostAsync("/apir3/api/addon/acknowledge-send-mail", content);
                    if (!result.IsSuccessStatusCode)
                    {
                        var err = await result.Content.ReadAsStringAsync();
                        var errObj = Newtonsoft.Json.Linq.JObject.Parse(err);
                        var errMsg = errObj["message"]?.ToString();

                        response.status = new Status()
                        {
                            code = ResponseCode.NotFound.Text(),
                            message = errMsg ?? err
                        };
                    }
                    else
                    {
                        var raw = await result.Content.ReadAsStringAsync();

                        Logger.LogInfo("acknowledge-send-mail", $"result: {raw}");
                        response.status = new Status()
                        {
                            code = ResponseCode.Success.Text(),
                            message = "Send Acknowledge Document Success"
                        };
                    }
                }
            }
            catch (System.Exception ex)
            {
                response = new AcknowledgeDocumentResponse()
                {
                    status = new Status()
                    {
                        code = ResponseCode.InternalServerError.Text(),
                        message = ResponseCode.InternalServerError.Description()
                    },
                    data = null
                };
                Logger.LogError(ex, "Acknowledge Document");
            }
            return response;
        }
        #endregion

        #region Send Maill New Docunent
        public async Task SendEmailNotify(HttpClient client, List<string> supplierIds, string rfqId, string language)
        {
            var delayMs = Random.Shared.Next(5000, 10001);
            await Task.Delay(delayMs);

            foreach (var supId in supplierIds)
            {
                try
                {
                    var resSuppliers = await _kubBossService.GetSuppliersDetail(client, supId);

                    var email = resSuppliers["data"]?["supplier_email"]?.ToString();

                    if (string.IsNullOrWhiteSpace(email))
                    {
                        Logger.LogInfo("NotifyRFQ", $"Skip supplier {supId} (no email)");
                        continue;
                    }

                    var payload = new JObject
                    {
                        ["email"] = email,
                        ["rfq_id"] = rfqId,
                        ["lang"] = language ?? "TH",
                    };

                    var success = false;

                    for (int i = 1; i <= 3; i++)
                    {
                        var response = await client.PostAsync(
                            "/api/notify/user/rfq",
                            new StringContent(payload.ToString(), Encoding.UTF8, "application/json")
                        );

                        var resBody = await response.Content.ReadAsStringAsync();

                        if (response.IsSuccessStatusCode)
                        {
                            Logger.LogInfo("NotifyRFQ",
                                $"SUCCESS | supplier:{supId} | email:{email}");

                            success = true;
                            break;
                        }

                        Logger.LogInfo("NotifyRFQ",
                            $"Retry {i} | supplier:{supId} | response:{resBody}");

                        if (i < 3)
                            await Task.Delay(6000);
                    }

                    if (!success)
                    {
                        Logger.LogError(
                            new Exception("NotifyRFQ failed"),
                            "NotifyRFQ",
                            $"supplier:{supId} | email:{email}"
                        );
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, $"NotifyRFQ ERROR supplier:{supId}");
                }
            }
        }
        #endregion

        public async Task<RFQCancelResponse> RFQCancelDocument(RFQCancelRequest request)
        {
            RFQCancelResponse response = new();

            try
            {
                var rfqInfo = await _wolfApproveRepository.SP_GET_RFQ_DETAIL(request.rfq_id);

                if (rfqInfo != null && rfqInfo.Any())
                {
                    var _dataInfo = rfqInfo.FirstOrDefault();

                    var req = new RFQCancelRequest()
                    {
                        rfq_id = _dataInfo.nRFQID.ToString(),
                        rfq_number = _dataInfo.sRFQNumber,
                        remark = request.remark
                    };

                    var sqlParameter = new SqlParameter[] {
                                new SqlParameter("@sChannel", _appConfigHelper.GetConfiguration("KubbossChannel"))
                            };

                    var configToken = await _dbContext.ExcuteStoreQuerySingleAsync<SP_GET_SYSENDPOINT>("SP_GET_SYSENDPOINT", sqlParameter);

                    var endPoint = _appConfigHelper.GetConfiguration("EndPoint:Kubboss");

                    var client = HttpClientHelper.CreateClient(endPoint, configToken?.sToken);

                    var resCancel = await _kubBossService.CancelRFQKubboss(client, req);

                    if (resCancel?.status?.code == "200")
                    {
                        await _wolfApproveRepository.SP_UPDATE_RFQ_CANCEL(request.rfq_number, request.remark);
                    }

                    return resCancel;
                }
            }
            catch (System.Exception ex)
            {
                response = new RFQCancelResponse()
                {
                    status = new Status()
                    {
                        code = ResponseCode.InternalServerError.Text(),
                        message = ResponseCode.InternalServerError.Description()
                    },
                    data = null
                };
                Logger.LogError(ex, "Cancel RFQ Document");
            }
            return response;
        }

        private async Task UploadMedia(string modelId, IFormFile file = null, byte[] fileBytes = null, string fileName = null, string contentType = null, string uploadForm = null)
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
                form.Add(new StringContent(uploadForm), "type");
                form.Add(new StringContent(Guid.NewGuid().ToString()), "file_uuid");

                HttpContent uploadContent;
                string uploadFileName;
                string uploadContentType;

                if (file != null)
                {
                    uploadContent = new StreamContent(file.OpenReadStream());
                    uploadFileName = file.FileName;
                    uploadContentType = file.ContentType;
                }
                else if (fileBytes != null && fileBytes.Length > 0)
                {
                    uploadContent = new ByteArrayContent(fileBytes);
                    uploadFileName = fileName;
                    uploadContentType = contentType;
                }
                else
                {
                    throw new Exception("File is required.");
                }

                uploadContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(uploadContentType);

                form.Add(uploadContent, "file", uploadFileName);

                var response = await client.PostAsync("/api/media", form);

                response.EnsureSuccessStatusCode();


            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "UploadMedia");

            }
        }
    }

}