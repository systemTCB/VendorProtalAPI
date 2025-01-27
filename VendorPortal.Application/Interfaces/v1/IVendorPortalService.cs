using System.Threading.Tasks;
using VendorPortal.Application.Models.v1.Request;
using VendorPortal.Application.Models.v1.Response;

namespace VendorPortal.Application.Interfaces.v1
{
    public interface IVendorPortalService
    {
        Task<PurchaseOrderResponse> GetPurchaseOrder(PurchaseOrderRequest request);
    }
}