using System;
using System.Threading.Tasks;

namespace VendorPortal.Application.Interfaces.SyncExternalData
{
    public interface IKubbossService
    {
        Task<bool> SyncVendorFromKubboss(DateTime dateTime);
    }
}