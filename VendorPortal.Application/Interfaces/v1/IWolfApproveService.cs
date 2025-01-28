using System.Threading.Tasks;
using VendorPortal.Application.Models.v1.Response;
using VendorPortal.Application.Models.v1.Request;

namespace VendorPortal.Application.Interfaces.v1
{
    public interface IWolfApproveService
    {
        // RFQ
        Task<RFQResponse> GetRFQ_List();
        Task<RFQShowResponse> GetRFQ_Show(string rfq_id);
        
        // Purchase Order
        Task<PurchaseOrderResponse> GetPurchaseOrderList();
        Task<PurchaseOrderDetailResponse> GetPurchaseOrderShow(string order_id , string supplier_id);
        Task<PurchaseOrderConfirmResponse> ConfirmPurchaseOrderStatus(string purchase_order_id , PurchaseOrderConfirmRequest request);

        // Claim 
        Task<ClaimResponse> GetClaimList();
        Task<ClaimDetailResponse> GetClaimShow(string claim_id , string supplier_id);
        Task<ClaimConfirmResponse> ConfirmClaimStatus(string claim_id , ClaimConfirmRequest request);
    }
}