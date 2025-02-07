using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Azure;
using Microsoft.AspNetCore.Http;
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
        public WolfApproveService(IHttpContextAccessor httpContext,
            IWolfApproveRepository wolfApproveRepository,
            AppConfigHelper appConfigHelper
            )
        {
            IHttpContextAccessor _httpContext = httpContext;
            _appConfigHelper = appConfigHelper;
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
                        Status = new Status()
                        {
                            Code = ResponseCode.Success.Text(),
                            Message = ResponseCode.Success.Description()
                        }
                    };
                }
                else
                {
                    // รอดูก่อนว่าตรงนี้ทำงานยังไง อาจจะต้องมี reason ที่แตกต่างกัน
                    result = new ClaimConfirmResponse()
                    {
                        Status = new Status()
                        {
                            Code = ResponseCode.NotFound.Text(),
                            Message = ResponseCode.NotFound.Description()
                        }
                    };
                }
            }
            catch (ApplicationException ex)
            {
                Logger.LogError(ex, "ConfirmClaimStatus", $"claim_id: {claim_id} , request: {request}");
                result = new ClaimConfirmResponse()
                {
                    Status = new Status()
                    {
                        Code = ResponseCode.NotImplement.Text(),
                        Message = ResponseCode.NotImplement.Description()
                    },
                    Data = null
                };
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "ConfirmClaimStatus", $"claim_id: {claim_id} , request: {request}");
                result = new ClaimConfirmResponse()
                {
                    Status = new Status()
                    {
                        Code = ResponseCode.InternalServerError.Text(),
                        Message = ResponseCode.InternalServerError.Description()
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
                    response.Status = new Status
                    {
                        Code = ResponseCode.Success.Text(),
                        Message = ResponseCode.Success.Description()
                    };
                }
                else
                {
                    response = new PurchaseOrderConfirmResponse()
                    {
                        Status = new Status()
                        {
                            Code = ResponseCode.NotFound.Text(),
                            Message = ResponseCode.NotFound.Description()
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
                    Status = new Status()
                    {
                        Code = ResponseCode.NotImplement.Text(),
                        Message = ResponseCode.NotImplement.Description()
                    },
                    Data = null
                };
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "ConfirmPurchaseOrderStatus", $"purchase_order_id: {purchase_order_id} , request: {request}");
                response = new PurchaseOrderConfirmResponse()
                {
                    Status = new Status()
                    {
                        Code = ResponseCode.InternalServerError.Text(),
                        Message = ResponseCode.InternalServerError.Description()
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
                    Status = new Status()
                    {
                        Code = ResponseCode.NotImplement.Text(),
                        Message = ResponseCode.NotImplement.Description()
                    },
                    Data = null
                };
                Logger.LogError(ex, "ConnectCompanies", $"supplier_id: {supplier_id} , request: {request}");
            }
            catch (System.Exception ex)
            {
                result = new CompaniesConnectResponse()
                {
                    Status = new Status()
                    {
                        Code = ResponseCode.InternalServerError.Text(),
                        Message = ResponseCode.InternalServerError.Description()
                    },
                    Data = null
                };
                Logger.LogError(ex, "ConnectCompanies", $"supplier_id: {supplier_id} , request: {request}");

            }
            return result;
        }

        public async Task<ClaimResponse> GetClaimList(string supplier_id, string company_id, string status, string from_date, string to_date, string page, string per_page, string order_direction, string order_by)
        {
            ClaimResponse result = new();
            try
            {
                var store = await _wolfApproveRepository.SP_GET_CLAIM_LIST(supplier_id, company_id, from_date, to_date);
                if (store.Count != 0)
                {
                    result.Data = [.. store.Select(s => new ClaimData{
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
                            Code = s.Purchase_order.Code,
                            Purchase_date = s.Purchase_order.Purchase_date
                        }
                    })];
                    result.Status = new Status()
                    {
                        Code = ResponseCode.Success.Text(),
                        Message = ResponseCode.Success.Description()
                    };
                }
                else
                {
                    result = new ClaimResponse()
                    {
                        Status = new Status()
                        {
                            Code = ResponseCode.NotFound.Text(),
                            Message = ResponseCode.NotFound.Description()
                        }
                    };
                }
            }
            catch (ApplicationException ex)
            {
                Logger.LogError(ex, "GetClaimList");
                result = new ClaimResponse()
                {
                    Status = new Status()
                    {
                        Code = ResponseCode.NotImplement.Text(),
                        Message = ResponseCode.NotImplement.Description()
                    }
                };
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "GetClaimList");
                result = new ClaimResponse()
                {
                    Status = new Status()
                    {
                        Code = ResponseCode.InternalServerError.Text(),
                        Message = ResponseCode.InternalServerError.Description()
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
                    result.Data = new ClaimDetailData()
                    {
                        Claim_date = store.ClaimDate.ToString(),
                        Claim_description = store.ClaimDescription,
                        Claim_option = store.ClaimOption,
                        Claim_reason = store.ClaimReason,
                        Code = store.Code,
                        Company_name = store.CompanyName,
                        Create_date = store.CreatedDate.ToString(),
                        Documents = [.. store.Documents.Select(s => new Document()
                        {
                            FileUrl = s.FileUrl,
                            Name = s.Name
                        })],
                        Id = store.Id,
                        Lines = [.. store.Lines.Select(s => new Line()
                        {
                            Description = s.Description,
                            Id = s.Id,
                            Item_code = s.ItemCode,
                            Item_name = s.ItemName,
                            Line_number = s.LineNumber,
                            Quantity = s.Quantity,
                            Uom_name = s.UomName,
                            Unit_price = s.UnitPrice
                        })],
                        Status = new ClaimStatus()
                        {
                            Name = store.Status.FirstOrDefault().Name
                        },
                        Claim_return_address = store.ClaimReturnAddress,
                        Purchase_order = new ClaimPurchaseOrderData()
                        {
                            Code = store.PurchaseOrder.Code,
                            Purchase_date = store.PurchaseOrder.PurchaseDate.ToString()
                        }
                    };
                    result = new ClaimDetailResponse()
                    {
                        Status = new Status()
                        {
                            Code = ResponseCode.Success.Text(),
                            Message = ResponseCode.Success.Description()
                        }
                    };
                }
                else
                {
                    result = new ClaimDetailResponse()
                    {
                        Status = new Status()
                        {
                            Code = ResponseCode.NotFound.Text(),
                            Message = ResponseCode.NotFound.Description()
                        }
                    };
                }
            }
            catch (ApplicationException ex)
            {
                result = new ClaimDetailResponse()
                {
                    Status = new Status()
                    {
                        Code = ResponseCode.NotImplement.Text(),
                        Message = ResponseCode.NotImplement.Description()
                    }
                };
                Logger.LogError(ex, "GetClaimDetail", $"claim_id: {claim_id} , supplier_id: {supplier_id}");
            }
            catch (System.Exception ex)
            {
                result = new ClaimDetailResponse()
                {
                    Status = new Status()
                    {
                        Code = ResponseCode.InternalServerError.Text(),
                        Message = ResponseCode.InternalServerError.Description()
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
                            Email = store.Company_contact.Email,
                            First_name = store.Company_contact.First_name,
                            Last_Name = store.Company_contact.Last_name,
                            Phone = store.Company_contact.Phone
                        },
                        Id = store.Id,
                        Name = store.Name,
                        Request_date = store.Request_date.ToString(),
                        Request_status = store.Request_status,
                        Website = store.Website
                    };
                    result.Status = new Status()
                    {
                        Code = ResponseCode.Success.Text(),
                        Message = ResponseCode.Success.Description()
                    };
                }
                else
                {
                    result = new CompaniesDetailResponse()
                    {
                        Status = new Status()
                        {
                            Code = ResponseCode.NotFound.Text(),
                            Message = ResponseCode.NotFound.Description()
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

        public async Task<CompaniesResponse> GetCompaniesList(string supplier_id)
        {
            CompaniesResponse response = new();
            try
            {
                var store = await _wolfApproveRepository.SP_GET_COMPANIES_LIST(supplier_id);
                if (store.Count != 0)
                {
                    response.Data = [.. store.Select(s=> new CompainesData{
                        Id = s.Id,
                        Company_contact =  new CompanyContract(){
                            First_name = s.Company_contact.First_name,
                            Email = s.Company_contact.Email,
                            Last_Name = s.Company_contact.Last_Name,
                            Phone = s.Company_contact.Phone
                        },
                        Name = s.Name,
                        Request_date = s.Request_date,
                        Request_status = s.Request_status,

                    })];
                    response.Status = new Status
                    {
                        Code = ResponseCode.Success.Text(),
                        Message = ResponseCode.Success.Description()
                    };
                }
                else
                {
                    response = new CompaniesResponse()
                    {
                        Status = new Status()
                        {
                            Code = ResponseCode.NotFound.Text(),
                            Message = ResponseCode.NotFound.Description()
                        },
                        Data = null
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
                    Status = new Status()
                    {
                        Code = ResponseCode.Success.Text(),
                        Message = ResponseCode.Success.Description()
                    }
                };
            }
            catch (ApplicationException ex)
            {
                result = new CountResponse()
                {
                    Status = new Status()
                    {
                        Code = ResponseCode.NotImplement.Text(),
                        Message = ResponseCode.NotImplement.Description()
                    }
                };
                Logger.LogError(ex, "GetCount", $"supplier_id: {supplier_id}");
            }
            catch (System.Exception ex)
            {
                result = new CountResponse()
                {
                    Status = new Status()
                    {
                        Code = ResponseCode.InternalServerError.Text(),
                        Message = ResponseCode.InternalServerError.Description()
                    }
                };
                Logger.LogError(ex, "GetCount", $"supplier_id: {supplier_id}");
            }
            return result;
        }

        public async Task<PurchaseOrderListResponse> GetPurchaseOrderList(string q, string supplier_id, string number, string start_date, string end_date, string purchase_type_id, string status_id, string category_id, string page, string per_page, string order_direction, string order_by)
        {
            PurchaseOrderListResponse result = new();
            try
            {
                var store = await _wolfApproveRepository.SP_GET_PURCHASE_ORDER_LIST();
                if (store.Count != 0)
                {
                    result.Data = [.. store.Select(s => new PurchaseOrderData()
                    {
                        Id = s.Id,
                        Cancel_description = s.Cancel_description,
                        Cancel_reason = s.Cancel_reason,
                        Category_name = s.Category_name,
                        Code = s.Code,
                        Company_contract = new CompanyContract{
                            First_name = s.Company_contract.First_name,
                            Last_Name = s.Company_contract.Last_name,
                            Email = s.Company_contract.Email,
                            Phone = s.Company_contract.Phone
                        },
                        Company_Name = s.Company_Name,
                        Description = s.Description,
                        Net_Amount = s.Net_Amount,
                        Order_date = s.Order_date,
                        Payment_condition = s.Payment_condition,
                        Purchase_type_name = s.Purchase_type_name,
                        Quotation = new QuotationData{
                            Code = s.Quotation.Code
                        },
                        Remark = s.Remark,
                        Require_date = s.Require_date,
                        Ship_to = s.Ship_to
                    })];
                    result.Status = new Status()
                    {
                        Code = ResponseCode.Success.Text(),
                        Message = ResponseCode.Success.Description()
                    };
                }
                else
                {
                    result = new PurchaseOrderListResponse()
                    {
                        Status = new Status()
                        {
                            Code = ResponseCode.NotFound.Text(),
                            Message = ResponseCode.NotFound.Description()
                        }
                    };
                }
            }
            catch (ApplicationException ex)
            {
                Logger.LogError(ex, "GetPurchaseOrderList");
                result = new PurchaseOrderListResponse()
                {
                    Status = new Status()
                    {
                        Code = ResponseCode.NotImplement.Text(),
                        Message = ResponseCode.NotImplement.Description()
                    }
                };
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "GetPurchaseOrderList");
                result = new PurchaseOrderListResponse()
                {
                    Status = new Status()
                    {
                        Code = ResponseCode.InternalServerError.Text(),
                        Message = ResponseCode.InternalServerError.Description()
                    }
                };
            }
            return result;
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
                                Email = store.Company_contract.Email,
                                First_name = store.Company_contract.First_name,
                                Last_Name = store.Company_contract.Last_name,
                                Phone = store.Company_contract.Phone
                            },
                            Company_name = store.Company_name,
                            Description = store.Description,
                            Discount = store.Discount,
                            Documents = store.Documents.Select(s => new Document()
                            {
                                FileUrl = s.File_url,
                                Name = s.Name
                            }).ToList(),
                            Lines = store.Lines.Select(s => new Line()
                            {
                                Description = s.Description,
                                Id = s.Id,
                                Item_code = s.Item_code,
                                Item_name = s.Item_name,
                                Line_number = s.Line_number,
                                Quantity = s.Quantity,
                                Uom_name = s.Uom_name,
                                Unit_price = s.Unit_price
                            }).ToList(),
                            Net_amount = store.Net_amount,
                            Order_date = store.Order_date,
                            Request_date = store.Request_Date,
                            Payment_condition = store.Payment_condition,
                            Quotation = new QuotationData()
                            {
                                Code = store.Quotation.Code
                            },
                            Remark = store.Remark,
                            Status = store.Status,
                            Sub_totoal = store.Sub_totoal,
                            Total_amount = store.Total_amount,
                            Vat_amount = store.Vat_amount

                        };
                        result.Status = new Status()
                        {
                            Code = ResponseCode.Success.Text(),
                            Message = ResponseCode.Success.Description()
                        };
                    }
                    else
                    {
                        result = new PurchaseOrderDetailResponse()
                        {
                            Status = new Status()
                            {
                                Code = ResponseCode.NotFound.Text(),
                                Message = ResponseCode.NotFound.Description()
                            },
                            Data = null
                        };
                    }
                }
                else
                {
                    result = new PurchaseOrderDetailResponse()
                    {
                        Status = new Status()
                        {
                            Code = ResponseCode.BadRequest.Text(),
                            Message = "กรุณากรอกข้อมูล order_id และ supplier_id"
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

        public async Task<RFQResponse> GetRFQ_List()
        {
            var result = new RFQResponse();
            try
            {
                var sp_result = await _wolfApproveRepository.SP_GET_RFQ_LIST();
                if (sp_result != null)
                {
                    var data = sp_result.Select(s => new RFQData()
                    {
                        CategoryName = s.Category_Name,
                        Code = s.Code,
                        Description = s.Description,
                        CompanyContract = new CompanyContract
                        {
                            Email = s.Company_Contract.Email,
                            First_name = s.Company_Contract.First_name,
                            Last_Name = s.Company_Contract.Last_Name,
                            Phone = s.Company_Contract.Phone
                        },
                        CompanyName = s.Company_Name,
                        ContractValue = s.Contract_Value,
                        EndDate = s.End_Date,
                        Id = s.Id,
                        NetAmount = s.Net_Amount,
                        PaymentCondition = s.Payment_Condition,
                        ProcurementTypeName = s.Procurement_Type_Name,
                        ProjectName = s.Project_Name,
                        Remark = s.Remark,
                        RequireDate = s.Require_Date,
                        StartDate = s.Start_Date,
                        Status = s.Status
                    }).ToList();
                    result = new RFQResponse()
                    {
                        Status = new Status()
                        {
                            Code = ResponseCode.Success.Text(),
                            Message = ResponseCode.Success.Description()
                        },
                        Data = data
                    };
                }
                else
                {
                    result = new RFQResponse()
                    {
                        Status = new Status()
                        {
                            Code = ResponseCode.NotFound.Text(),
                            Message = ResponseCode.NotFound.Description()
                        }
                    };
                }
            }
            catch (ApplicationException ex)
            {
                result = new RFQResponse()
                {
                    Status = new Status()
                    {
                        Code = ResponseCode.NotImplement.Text(),
                        Message = ResponseCode.NotImplement.Description()
                    }
                };
                Logger.LogError(ex, "GetRFQ_List");
            }
            catch (System.Exception ex)
            {
                result = new RFQResponse()
                {
                    Status = new Status()
                    {
                        Code = ResponseCode.InternalServerError.Text(),
                        Message = ResponseCode.InternalServerError.Description(),
                    }
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
                if (sp_result != null)
                {
                    response = new RFQShowResponse()
                    {
                        Status = new Status()
                        {
                            Code = ResponseCode.Success.Text(),
                            Message = ResponseCode.Success.Text()
                        },
                        Data = new RFQShowData
                        {
                            Category_Name = sp_result.Category_name,
                            Company_Address = new CompanyAddress
                            {
                                Address_1 = sp_result.Company_address.Address_1,
                                Address_2 = sp_result.Company_address.Address_2,
                                Branch = sp_result.Company_address.Branch,
                                District = sp_result.Company_address.District,
                                Province = sp_result.Company_address.Province,
                                Sub_District = sp_result.Company_address.Sub_district,
                                Tax_Number = sp_result.Company_address.Tax_number,
                                Zip_Code = sp_result.Company_address.Zip_code
                            },
                            Company_Contract = new CompanyContract
                            {
                                Email = sp_result.Company_contract.Email,
                                First_name = sp_result.Company_contract.First_name,
                                Last_Name = sp_result.Company_contract.Last_name,
                                Phone = sp_result.Company_contract.Phone,
                            },
                            Company_Name = sp_result.Company_name,
                            ContractValue = Decimal.Parse(string.IsNullOrEmpty(sp_result.Contract_value) ? "0" : sp_result.Contract_value),
                            Description = sp_result.Description,
                            Discount = Decimal.Parse(string.IsNullOrEmpty(sp_result.Discount) ? "0" : sp_result.Discount),
                            Documents = [.. sp_result.Documents.Select(s => new Document
                            {
                                FileUrl = s.File_url,
                                Name = s.Name,
                            })],
                            EndDate = sp_result.End_date,
                            Id = sp_result.Id,
                            Lines = [.. sp_result.Lines.Select(s => new Line
                            {
                                Description = s.Description,
                                Id = s.Id,
                                Item_code = s.Item_code,
                                Item_name = s.Item_name,
                                Line_number = s.Line_number,
                                Quantity = s.Quantity,
                                Uom_name = s.Uom_name,
                                Unit_price = s.Unit_price
                            })],
                            NetAmount = Decimal.Parse(string.IsNullOrEmpty(sp_result.Net_amount) ? "0" : sp_result.Net_amount),
                            Number = sp_result.Number,
                            PaymentCondition = sp_result.Payment_condition,
                            ProjectName = sp_result.Project_name,
                            PurchaseTypeName = sp_result.Purchase_type_name,
                            Remark = sp_result.Remark,
                            RequireDate = sp_result.Require_date,
                            StartDate = sp_result.Start_date,
                            Status = [.. sp_result.Status.Select(s => new StatusName
                            {
                                Name = s.Name
                            })],
                            SubTotal = Decimal.Parse(string.IsNullOrEmpty(sp_result.Sub_total) ? "0" : sp_result.Sub_total),
                            TotalAmount = Decimal.Parse(string.IsNullOrEmpty(sp_result.Total_amount) ? "0" : sp_result.Total_amount),
                            VatAmount = Decimal.Parse(string.IsNullOrEmpty(sp_result.Vat_amount) ? "0" : sp_result.Vat_amount)
                        }
                    };
                }
                else
                {
                    response = new RFQShowResponse()
                    {
                        Status = new Status()
                        {
                            Code = ResponseCode.NotFound.Text(),
                            Message = ResponseCode.NotFound.Text()
                        }
                    };
                }
            }
            catch (System.Exception ex)
            {
                response = new RFQShowResponse()
                {
                    Status = new Status()
                    {
                        Code = ResponseCode.InternalServerError.Text(),
                        Message = ResponseCode.InternalServerError.Description()
                    }
                };
                Logger.LogError(ex, "GetRFQ_Show");
            }
            return response;
        }
    }

}