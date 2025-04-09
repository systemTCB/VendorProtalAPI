using System;
using System.Threading.Tasks;
namespace VendorPortal.Domain.Interfaces.SyncExternalData;

public interface IKubbossRepository
{
    Task<bool> SyncVendorFromKubboss(DateTime dateTime);
}
