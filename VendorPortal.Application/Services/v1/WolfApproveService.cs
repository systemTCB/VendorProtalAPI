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
            BaseResponse<List<ClaimResponse>> result = new();
            try
            {
                var store = await _wolfApproveRepository.SP_GET_CLAIM_LIST(supplier_id, company_id, from_date, to_date);
                if (store.Count != 0)
                {
                    result.data = [.. store.Select(s => new ClaimResponse{
                        Id = s.Id,
                        Claim_date = s.Claim_date,
                        Claim_description = s.Claim_description,
                        Claim_option = s.Claim_option,
                        Claim_reason = s.Claim_reason,
                        Claim_return_address = s.Claim_return_address,
                        Code = s.Code,
                        Company_name = s.Company_name,
                        Create_date = s.Create_date,
                        Purchase_order = new ClaimPurchaseOrderData{
                            code = s.Purchase_order.Code,
                            purchase_date = s.Purchase_order.Purchase_date
                        }
                    })];
                    result.status = new Status()
                    {
                        code = ResponseCode.Success.Text(),
                        message = ResponseCode.Success.Description()
                    };
                }
                else
                {
                    result = new BaseResponse<List<ClaimResponse>>()
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
                result = new BaseResponse<List<ClaimResponse>>()
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
                result = new BaseResponse<List<ClaimResponse>>()
                {
                    status = new Status()
                    {
                        code = ResponseCode.InternalServerError.Text(),
                        message = ResponseCode.InternalServerError.Description()
                    }
                };
            }
            return result;
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
                        Claim_date = store.Claim_date.ToString(),
                        Claim_description = store.Claim_description,
                        Claim_option = store.Claim_option,
                        Claim_reason = store.Claim_reason,
                        Code = store.Code,
                        Company_name = store.Company_name,
                        Create_date = store.Created_date.ToString(),
                        Documents = [.. store.Documents.Select(s => new Document()
                        {
                            fileUrl = s.File_url,
                            name = s.Name
                        })],
                        Id = store.Id,
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
                        Status = new ClaimStatus()
                        {
                            Name = [.. store.Status.Select(s => s.Name)]
                        },
                        Claim_return_address = store.Claim_return_address,
                        Purchase_order = new ClaimPurchaseOrderData()
                        {
                            code = store.Purchase_order.Code,
                            purchase_date = store.Purchase_order.Purchase_date.ToString()
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
                        Company_address = new CompanyAddress()
                        {
                            Address_1 = store.Company_address.Address_1,
                            Address_2 = store.Company_address.Address_2,
                            Branch = store.Company_address.Branch,
                            District = store.Company_address.District,
                            Province = store.Company_address.Province,
                            Sub_District = store.Company_address.Sub_district,
                            Tax_Number = store.Company_address.Tax_id,
                            Zip_Code = store.Company_address.Zip_code
                        },
                        Company_contract = new CompanyContract()
                        {
                            email = store.Company_contact.Email,
                            first_name = store.Company_contact.First_name,
                            last_name = store.Company_contact.Last_name,
                            phone = store.Company_contact.Phone
                        },
                        Id = store.Id,
                        Name = store.Name,
                        Request_date = store.Request_date.ToString(),
                        Request_status = store.Request_status,
                        Website = store.Website
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
                        Id = s.Id,
                        Company_contact =  new CompanyContract(){
                            first_name = s.sContractFirstName,
                            email = s.sContractEmail,
                            last_name = s.sContractLastName,
                            phone = s.sContractPhone
                        },
                        Name = s.Name,
                        Request_date = s.Request_date,
                        Request_status = s.Request_status,

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
                        Count_claim = store.Count_claim.GetValueOrDefault(),
                        Count_po = store.Count_po.GetValueOrDefault()
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

        public async Task<BaseResponse<List<PurchaseOrderResponse>>> GetPurchaseOrderList(string q, string supplier_id, string number, string start_date, string end_date, string purchase_type_id, string status_id, string category_id, string page, string per_page, string order_direction, string order_by)
        {
            BaseResponse<List<PurchaseOrderResponse>> response = new();
            try
            {
                var store = await _wolfApproveRepository.SP_GET_PURCHASE_ORDER_LIST();
                if (store.Count != 0)
                {
                    response.data = [.. store.Select(s => new PurchaseOrderResponse()
                    {
                        id = s.Id,
                        cancel_description = s.Cancel_description,
                        cancel_reason = s.Cancel_reason,
                        category_name = s.Category_name,
                        code = s.Code,
                        company_contract = new CompanyContract{
                            first_name = s.Company_contract.First_name,
                            last_name = s.Company_contract.Last_name,
                            email = s.Company_contract.Email,
                            phone = s.Company_contract.Phone
                        },
                        company_name = s.Company_Name,
                        description = s.Description,
                        net_amount = s.Net_Amount,
                        order_date = s.Order_date,
                        payment_condition = s.Payment_condition,
                        purchase_type_name = s.Purchase_type_name,
                        quotation = new QuotationData{
                            code = s.Quotation.Code
                        },
                        remark = s.Remark,
                        require_date = s.Require_date,
                        ship_to = s.Ship_to
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
            PurchaseOrderDetailResponse result = new();
            try
            {
                if (!string.IsNullOrEmpty(order_id) && !string.IsNullOrEmpty(supplier_id))
                {
                    var store = await _wolfApproveRepository.SP_GET_PURCHASE_ORDER_DETAIL(order_id, supplier_id);
                    if (store != null)
                    {
                        result.Data = new PurchaseOrderDetailData()
                        {
                            Id = store.Id,
                            Cancel_description = store.Cancel_description,
                            Cancel_reason = store.Cancel_reason,
                            Category_name = store.Category_Name,
                            Code = store.Code,
                            Company_address = new CompanyAddress()
                            {
                                Address_1 = store.Company_address.Address_1,
                                Address_2 = store.Company_address.Address_2,
                                Branch = store.Company_address.Branch,
                                District = store.Company_address.District,
                                Province = store.Company_address.Province,
                                Sub_District = store.Company_address.Sub_District,
                                Tax_Number = store.Company_address.Tax_number,
                                Zip_Code = store.Company_address.Zip_code
                            },
                            Company_contract = new CompanyContract()
                            {
                                email = store.Company_contract.Email,
                                first_name = store.Company_contract.First_name,
                                last_name = store.Company_contract.Last_name,
                                phone = store.Company_contract.Phone
                            },
                            Company_name = store.Company_name,
                            Description = store.Description,
                            Discount = store.Discount,
                            Documents = store.Documents.Select(s => new Document()
                            {
                                fileUrl = s.File_url,
                                name = s.Name
                            }).ToList(),
                            // Lines = store.Lines.Select(s => new Line()
                            // {
                            //     Description = s.Description,
                            //     Id = s.Id,
                            //     Item_code = s.Item_code,
                            //     Item_name = s.Item_name,
                            //     Line_number = s.Line_number,
                            //     Quantity = s.Quantity,
                            //     Uom_name = s.Uom_name,
                            //     Unit_price = s.Unit_price
                            // }).ToList(),
                            Net_amount = store.Net_amount,
                            Order_date = store.Order_date,
                            Request_date = store.Request_Date,
                            Payment_condition = store.Payment_condition,
                            Quotation = new QuotationData()
                            {
                                code = store.Quotation.Code
                            },
                            Remark = store.Remark,
                            Status = store.Status,
                            Sub_totoal = store.Sub_totoal,
                            Total_amount = store.Total_amount,
                            Vat_amount = store.Vat_amount

                        };
                        result.status = new Status()
                        {
                            code = ResponseCode.Success.Text(),
                            message = ResponseCode.Success.Description()
                        };
                    }
                    else
                    {
                        result = new PurchaseOrderDetailResponse()
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
                    result = new PurchaseOrderDetailResponse()
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
            return result;
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
                        code = s.sRFQNumber,
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
                    result = Utility.Paging<List<RFQDataItem>>(page, pageSize, data.Count, _baseUrl);
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
                            Address_1 = _companyInfo.sAddress1,
                            Address_2 = _companyInfo.sAddress2,
                            Branch = _companyInfo.sBranch,
                            District = _companyInfo.sDistrict,
                            Province = _companyInfo.sProvince,
                            Sub_District = _companyInfo.sSubDistrict,
                            Tax_Number = _companyInfo.sTaxNumber,
                            Zip_Code = _companyInfo.sZipCode
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