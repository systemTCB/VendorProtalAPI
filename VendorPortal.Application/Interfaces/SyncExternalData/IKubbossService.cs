using System;
using System.Threading.Tasks;
using VendorPortal.Application.Models.Common;
using VendorPortal.Application.Models.v1.Response;

namespace VendorPortal.Application.Interfaces.SyncExternalData
{
    public interface IKubbossService
    {
        Task<BaseResponse> SyncVendorFromKubboss(DateTime dateTime);
        Task<QuotationResponse> SyncQuotationFromKubboss(string supplierId, string rfqId);
    }
}