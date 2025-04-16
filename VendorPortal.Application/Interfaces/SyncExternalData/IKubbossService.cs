using System;
using System.Threading.Tasks;
using VendorPortal.Application.Models.Common;

namespace VendorPortal.Application.Interfaces.SyncExternalData
{
    public interface IKubbossService
    {
        Task<BaseResponse> SyncVendorFromKubboss(DateTime dateTime);
    }
}