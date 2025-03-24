using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;
using VendorPortal.Application.Helpers;
using VendorPortal.Application.Interfaces.v1;
using VendorPortal.Application.Models.Common;
using VendorPortal.Application.Models.v1.Request;
using VendorPortal.Application.Models.v1.Response;
using VendorPortal.Domain.Interfaces.v1;
using VendorPortal.Infrastructure.Mock.WolfApprove.v1.Repository;
using VendorPortal.Logging;
using static VendorPortal.Application.Models.Common.AppEnum;
using static VendorPortal.Application.Models.Common.KubbossCommonModel;

namespace VendorPortal.Application.Services.v1
{
    public class WolfApproveService : IWolfApproveService
    {
        private readonly IWolfApproveRepository _wolfApproveRepository;
        private readonly AppConfigHelper _appConfigHelper;
        private readonly IHttpContextAccessor _httpContext;
        private readonly string _baseUrl = string.Empty;
        public WolfApproveService(IHttpContextAccessor httpContext,
            IWolfApproveRepository wolfApproveRepository,
            AppConfigHelper appConfigHelper
            )
        {
            _httpContext = httpContext;
            _appConfigHelper = appConfigHelper;
            _baseUrl = $"{_httpContext.HttpContext.Request.Scheme}://{_httpContext.HttpContext.Request.Host}{_httpContext.HttpContext.Request.Path}";
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var hop = _httpContext.HttpContext.Request.Headers["hop"].ToString();
            if (hop?.ToLower() == "off" && env?.ToLower() != "production")
            {
                _wolfApproveRepository = new MockWolfApproveRepository();
            }
            else
            {
                _wolfApproveRepository = wolfApproveRepository;
            }
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

        public async Task<BaseResponse<List<ClaimResponse>>> GetClaimList(string supplier_id, string company_id, string status, string from_date, string to_date, string page, string per_page, string order_direction, string order_by)
        {
            BaseResponse<List<ClaimResponse>> response = new();
            try
            {
                var store = await _wolfApproveRepository.SP_GET_CLAIM_LIST(supplier_id, company_id, from_date, to_date);
                if (store.Count != 0)
                {
                    //kick out if company_id is empty
                    if (string.IsNullOrEmpty(company_id))
                    {
                        response = Utility.PagingCalculator<List<ClaimResponse>>(int.Parse(page), int.Parse(per_page), store.Count, _baseUrl);
                        response.status = new Status()
                        {
                            code = ResponseCode.BadRequest.Text(),
                            message = ResponseCode.BadRequest.Description()
                        };
                        response.data = null;
                        return response;
                    }
                    else
                    {
                        store = store.Where(e => e.nCompanyID == Convert.ToInt16(company_id)).ToList();
                        response.data = [.. store.Select(s => new ClaimResponse{
                        id = s.nClaimID,
                        claim_date = s.dClaimDate,
                        claim_description = s.sClaimDescription,
                        claim_option = s.sClaimOption,
                        claim_reason = s.sClaimReason,
                        claim_return_address = s.sClaimReturnAddress,
                        claim_number = s.sClaimCode,
                        company_name = s.sCompanyName,
                        create_date = s.nCreatedDate,
                        purchase_order = new ClaimPurchaseOrderData{
                            code = s.sPOCode,
                            purchase_date = s.dPurchaseDate
                        }
                    })];
                        response.status = new Status()
                        {
                            code = ResponseCode.Success.Text(),
                            message = ResponseCode.Success.Description()
                        };
                    }
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
                        response.Data = new PurchaseOrderDetailData()
                        {
                            id = _temp.nPOID,
                            cancel_description = _temp.sCancelDesc,
                            cancel_reason = _temp.sCancelReason,
                            category_name = _temp.sCategoryName,
                            number = _temp.sPOCode,
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

        public async Task<BaseResponse<List<RFQDataItem>>> GetRFQ_List(int pageSize, int page, string company_id, string number, string start_date, string end_date, string purchase_type_id, string status_id, string category_id, string order_direction, string order_by, string q)
        {
            var result = new BaseResponse<List<RFQDataItem>>();
            try
            {
                var item = await _wolfApproveRepository.SP_GET_RFQ_LIST();
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
                    if (!string.IsNullOrEmpty(start_date))
                    {
                        item = [.. item.Where(s => s.dStartDate >= DateTime.Parse(start_date))];
                    }
                    if (!string.IsNullOrEmpty(end_date))
                    {
                        item = [.. item.Where(s => s.dEndDate <= DateTime.Parse(end_date))];
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
                        item = [.. item.Where(s => s.sRFQNumber.Contains(q) || s.sProjectName.Contains(q) || s.sCompanyName.Contains(q))];
                    }
                    if (!string.IsNullOrEmpty(order_direction))
                    {
                        if (order_direction == "asc")
                        {
                            item = item.OrderBy(s => s.sRFQNumber).ToList();
                        }
                        else
                        {
                            item = item.OrderByDescending(s => s.sRFQNumber).ToList();
                        }
                    }
                    page = page <= 0 ? 1 : page;
                    pageSize = pageSize <= 0 ? 10 : pageSize;
                    var dataList = Utility.ItemPerpageCalculator<Domain.Models.WolfApprove.StoreModel.SP_GET_RFQ_LIST>(item, page, pageSize);
                    var data = dataList.Select(s => new RFQDataItem()
                    {
                        category_name = s.sCategoryName,
                        number = s.sRFQNumber,
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
                        procurement_type_name = s.sProcurementTypeName,
                        project_name = s.sProjectName,
                        remark = s.sRemark,
                        require_date = s.dRequireDate,
                        start_date = s.dStartDate,
                        status = s.sStatusName
                    }).ToList();
                    result = Utility.PagingCalculator<List<RFQDataItem>>(page, pageSize, data.Count, _baseUrl);
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
                            email = _companyInfo.sContractEmail,
                            first_name = _companyInfo.sContractFirstName,
                            last_name = _companyInfo.sContractLastName,
                            phone = _companyInfo.sContractPhone,
                        },
                        category_name = _companyInfo.sCategoryName,
                        company_id = _companyInfo.nCompanyID,
                        company_name = _companyInfo.sCompanyName,
                        contract_value = _companyInfo.nContractValue,
                        description = _companyInfo.sProjectDesc,
                        discount = _companyInfo.dDiscount,
                        end_date = _companyInfo.dEndDate,
                        net_amount = _companyInfo.dNetAmount,
                        number = _companyInfo.sRFQNumber,
                        payment_condition = _companyInfo.sPaymentCondition,
                        project_name = _companyInfo.sProjectName,
                        purchase_type_name = _companyInfo.sProcurementTypeName,
                        remark = _companyInfo.sRemark,
                        require_date = _companyInfo.dRequireDate,
                        start_date = _companyInfo.dStartDate,
                        status = _companyInfo.sStatusName,
                        sub_total = _companyInfo.dSubTotal,
                        total_amount = _companyInfo.dTotalAmount,
                        vat_amount = _companyInfo.dVatAmount,
                        lines = new List<Line>(),
                        documents = new List<Document>()
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
                        foreach (var item in _documnet.Where(e => e.isActive == true))
                        {
                            tempList.documents.Add(new Document
                            {
                                fileUrl = item.sFilePath,
                                name = item.sFileName
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
    }

}