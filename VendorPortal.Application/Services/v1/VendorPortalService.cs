using System.Threading.Tasks;
using VendorPortal.Application.Interfaces.v1;
using VendorPortal.Application.Models.v1.Request;
using VendorPortal.Application.Models.v1.Response;
using VendorPortal.Application.Models.Common;
using static VendorPortal.Application.Models.Common.AppEnum;
using Microsoft.AspNetCore.Http;
using VendorPortal.Application.Helpers;
using System;
using VendorPortal.Infrastructure.Mock.ThaiRedCross.v1.Repository;
using System.ComponentModel.Design;
using Azure;
using System.Linq;
using VendorPortal.Domain.Interfaces.v1;
namespace VendorPortal.Application.Services.v1
{
    public class VendorPortalService : IVendorPortalService
    {
        public IVendorPortalRepository _vendotPortalRepository;
        public IWolfApproveRepository _wolfApproveRepository;
        private readonly AppConfigHelper _appConfigHelper;
        public VendorPortalService(IHttpContextAccessor httpContext,
                                    IVendorPortalRepository thaiRedCrossRepository,
                                    IWolfApproveRepository wolfApproveRepository,
                                    AppConfigHelper appConfigHelper)
        {
            _vendotPortalRepository = thaiRedCrossRepository;
            _wolfApproveRepository = wolfApproveRepository;
            _appConfigHelper = appConfigHelper;
            IHttpContextAccessor _httpContext = httpContext;
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var hop = _httpContext.HttpContext.Request.Headers["hop"].ToString();

            if (hop?.ToLower() == "off" && env?.ToLower() != "production")
            {
                _vendotPortalRepository = new MockVendorPortalRepository();
            }

        }
        public async Task<PurchaseOrderResponse> GetPurchaseOrder(PurchaseOrderRequest request)
        {
            var response = new PurchaseOrderResponse();
            try
            {
                var result = await _vendotPortalRepository.GetPurchaseOrder(request.orderNo);
                if (result.status.code == ResponseCode.Success.Text())
                {
                    response.data = result.data.Select(e => new PurchaseOrderData
                    {
                        OrderNo = e.orderNo,
                        OrderSendDate = e.orderSendDate,
                        OrderItem = e.orderItem.Select(s => new PurchaseItem
                        {
                            OrderItem = s.orderItem,
                            ItemName = s.itemName,
                            ItemValue = s.itemValue,
                            ItemUnit = s.itemUnit,
                        }).ToList()
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                response.Status = new Status()
                {
                    Code = ResponseCode.InternalError.Text(),
                    Message = ex.Message,
                };
            }

            return response;
        }

        
    }
}