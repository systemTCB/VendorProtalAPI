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
        Task<PurchaseOrderListResponse> GetPurchaseOrderList(string q, string supplier_id, string number, string start_date, string end_date, string purchase_type_id, string status_id, string category_id, string page, string per_page, string order_direction, string order_by);
        Task<PurchaseOrderDetailResponse> GetPurchaseOrderDetail(string order_id, string supplier_id);
        Task<PurchaseOrderConfirmResponse> ConfirmPurchaseOrderStatus(string purchase_order_id, PurchaseOrderConfirmRequest request);

        // Claim 
        Task<ClaimResponse> GetClaimList(string supplier_id, string company_id, string status, string from_date, string to_date, string page, string per_page, string order_direction, string order_by);
        Task<ClaimDetailResponse> GetClaimDetail(string claim_id, string supplier_id);
        Task<ClaimConfirmResponse> ConfirmClaimStatus(string claim_id, ClaimConfirmRequest request);

        // Companies
        Task<CompaniesResponse> GetCompaniesList(string supplier_id);
        Task<CompaniesDetailResponse> GetCompaniesDetail(string company_id);
        Task<CompaniesConnectResponse> ConnectCompanies(string supplier_id, CompaniesConnectRequest request);

        //Count
        Task<CountResponse> GetCountClaimPo(string supplier_id);

    }
}