using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
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

        public Task<ClaimConfirmResponse> ConfirmClaimStatus(string claim_id, ClaimConfirmRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<PurchaseOrderConfirmResponse> ConfirmPurchaseOrderStatus(string purchase_order_id, PurchaseOrderConfirmRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<CompaniesConnectResponse> ConnectCompanies(string supplier_id)
        {
            throw new NotImplementedException();
        }

        public Task<ClaimResponse> GetClaimList()
        {
            throw new NotImplementedException();
        }

        public Task<ClaimDetailResponse> GetClaimShow(string claim_id, string supplier_id)
        {
            throw new NotImplementedException();
        }

        public Task<CompaniesDetailResponse> GetCompaniesById(string company_id)
        {
            throw new NotImplementedException();
        }

        public Task<CompaniesResponse> GetCompaniesList(string supplier_id)
        {
            throw new NotImplementedException();
        }

        public Task<CountResponse> GetCount(string supplier_id)
        {
            throw new NotImplementedException();
        }

        public Task<PurchaseOrderResponse> GetPurchaseOrderList()
        {
            throw new NotImplementedException();
        }

        public async Task<PurchaseOrderDetailResponse> GetPurchaseOrderShow(string order_id, string supplier_id)
        {
            PurchaseOrderDetailResponse result = new();
            try
            {
                if (!string.IsNullOrEmpty(order_id) && !string.IsNullOrEmpty(supplier_id))
                {
                    var store = await _wolfApproveRepository.SP_GET_PURCHASE_ORDER_SHOW(order_id, supplier_id);
                    if (store != null)
                    {
                        result.Data = new PurchaseOrderDetailData()
                        {
                            Id = store.Id,
                                   
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
                var sp_result = await _wolfApproveRepository.SP_GETRFQ_LIST();
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
                            First_Name = s.Company_Contract.First_Name,
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
                        Code = ResponseCode.InternalError.Text(),
                        Message = ResponseCode.InternalError.Description(),
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
                var sp_result = await _wolfApproveRepository.SP_GETRFQ_SHOW(rfq_id);
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
                            Category_Name = sp_result.CategoryName,
                            Company_Address = new CompanyAddress
                            {
                                Address_1 = sp_result.CompanyAddress.Address1,
                                Address_2 = sp_result.CompanyAddress.Address2,
                                Branch = sp_result.CompanyAddress.Branch,
                                District = sp_result.CompanyAddress.District,
                                Province = sp_result.CompanyAddress.Province,
                                Sub_District = sp_result.CompanyAddress.SubDistrict,
                                Tax_Number = sp_result.CompanyAddress.TaxNumber,
                                Zip_Code = sp_result.CompanyAddress.ZipCode
                            },
                            Company_Contract = new CompanyContract
                            {
                                Email = sp_result.CompanyContract.Email,
                                First_Name = sp_result.CompanyContract.FirstName,
                                Last_Name = sp_result.CompanyContract.LastName,
                                Phone = sp_result.CompanyContract.Phone,
                            },
                            Company_Name = sp_result.CompanyName,
                            ContractValue = Decimal.Parse(string.IsNullOrEmpty(sp_result.ContractValue) ? "0" : sp_result.ContractValue),
                            Description = sp_result.Description,
                            Discount = Decimal.Parse(string.IsNullOrEmpty(sp_result.Discount) ? "0" : sp_result.Discount),
                            Documents = sp_result.Documents.Select(s => new Document
                            {
                                FileUrl = s.FileUrl,
                                Name = s.Name,
                            }).ToList(),
                            EndDate = sp_result.EndDate,
                            Id = sp_result.Id,
                            Lines = sp_result.Lines.Select(s => new Line
                            {
                                Description = s.Description,
                                Id = s.Id,
                                Item_code = s.ItemCode,
                                Item_name = s.ItemName,
                                Line_number = s.LineNumber,
                                Quantity = Decimal.Parse(string.IsNullOrEmpty(s.Quantity) ? "0" : s.Quantity),
                                Uom_name = s.UomName,
                                Unit_price = Decimal.Parse(string.IsNullOrEmpty(s.UnitPrice) ? "0" : s.UnitPrice)
                            }).ToList(),
                            NetAmount = Decimal.Parse(string.IsNullOrEmpty(sp_result.NetAmount) ? "0" : sp_result.NetAmount),
                            Number = sp_result.Number,
                            PaymentCondition = sp_result.PaymentCondition,
                            ProjectName = sp_result.ProjectName,
                            PurchaseTypeName = sp_result.PurchaseTypeName,
                            Remark = sp_result.Remark,
                            RequireDate = sp_result.RequireDate,
                            StartDate = sp_result.StartDate,
                            Status = sp_result.Status.Select(s => new StatusName
                            {
                                Name = s.Name
                            }).ToList(),
                            SubTotal = Decimal.Parse(string.IsNullOrEmpty(sp_result.SubTotal) ? "0" : sp_result.SubTotal),
                            TotalAmount = Decimal.Parse(string.IsNullOrEmpty(sp_result.TotalAmount) ? "0" : sp_result.TotalAmount),
                            VatAmount = Decimal.Parse(string.IsNullOrEmpty(sp_result.VatAmount) ? "0" : sp_result.VatAmount)
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
                        Code = ResponseCode.InternalError.Text(),
                        Message = ResponseCode.InternalError.Description()
                    }
                };
                Logger.LogError(ex, "GetRFQ_Show");
            }
            return response;
        }
    }

}