using System.Collections.Generic;
using System.Threading.Tasks;
using VendorPortal.Domain.Models.KubBoss.v1.Response;
using VendorPortal.Domain.Models.WolfApprove.StoreModel;

namespace VendorPortal.Domain.Interfaces.v1
{
    public interface IVendorPortalRepository
    {
        Task<GetPurchaseOrderExResponse> GetPurchaseOrder(string PONumber);
    }
}