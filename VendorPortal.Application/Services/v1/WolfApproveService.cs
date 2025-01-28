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
using static VendorPortal.Application.Models.Common.AppEnum;

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

        public Task<PurchaseOrderConfirmResponse> ConfirmPurchaseOrderStatus(string purchase_order_id, PurchaseOrderConfirmRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ClaimResponse> GetClaimList()
        {
            throw new NotImplementedException();
        }

        public Task<PurchaseOrderResponse> GetPurchaseOrderList()
        {
            throw new NotImplementedException();
        }

        public Task<PurchaseOrderDetailResponse> GetPurchaseOrderShow(string order_id , string supplier_id)
        {
            throw new NotImplementedException();
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
                            FirstName = s.Company_Contract.First_Name,
                            LastName = s.Company_Contract.Last_Name,
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
                            Message = ResponseCode.Success.Text()
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
                            Message = ResponseCode.NotFound.Text()
                        }
                    };
                }
            }
            catch (System.Exception ex)
            {
                result = new RFQResponse()
                {
                    Status = new Status()
                    {
                        Code = ResponseCode.InternalError.Text(),
                        Message = ex.Message
                    }
                };
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
                            CategoryName = sp_result.CategoryName,
                            CompanyAddress = new CompanyAddress
                            {
                                Address1 = sp_result.CompanyAddress.Address1,
                                Address2 = sp_result.CompanyAddress.Address2,
                                Branch = sp_result.CompanyAddress.Branch,
                                District = sp_result.CompanyAddress.District,
                                Province = sp_result.CompanyAddress.Province,
                                SubDistrict = sp_result.CompanyAddress.SubDistrict,
                                TaxNumber = sp_result.CompanyAddress.TaxNumber
                            },
                            CompanyContract = new CompanyContract
                            {
                                Email = sp_result.CompanyContract.Email,
                                FirstName = sp_result.CompanyContract.FirstName,
                                LastName = sp_result.CompanyContract.LastName,
                                Phone = sp_result.CompanyContract.Phone,
                            },
                            CompanyName = sp_result.CompanyName,
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
                                ItemCode = s.ItemCode,
                                ItemName = s.ItemName,
                                LineNumber = s.LineNumber,
                                Quantity = Decimal.Parse(string.IsNullOrEmpty(s.Quantity) ? "0" : s.Quantity),
                                UomName = s.UomName,
                                UnitPrice = Decimal.Parse(string.IsNullOrEmpty(s.UnitPrice) ? "0" : s.UnitPrice)
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
            }
            return response;
        }
    }

}