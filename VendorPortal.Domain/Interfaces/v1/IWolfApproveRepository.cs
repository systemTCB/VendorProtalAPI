using System.Collections.Generic;
using System.Threading.Tasks;
using VendorPortal.Domain.Models.WolfApprove.StoreModel;

namespace VendorPortal.Domain.Interfaces.v1
{
    public interface IWolfApproveRepository
    {
        // RFQ
        Task<List<SP_GET_RFQ_LIST>> SP_GET_RFQ_LIST();
        Task<List<SP_GET_RFQ_DETAIL>> SP_GET_RFQ_DETAIL(string id);
        Task<List<SP_GET_RFQ_DOCUMENT>> SP_GET_RFQ_DOCUMENT(string id);

        // Purchase Order
        Task<List<SP_GET_PURCHASE_ORDER>> SP_GET_PURCHASE_ORDER_LIST();
        Task<List<SP_GET_PURCHASE_ORDER_DETAIL>> SP_GET_PURCHASE_ORDER_DETAIL(string id, string supplier_id);
        Task<SP_PUT_PURCHASE_ORDER_CONFIRM> SP_PUT_PURCHASE_ORDER_CONFIRM(string id, string status, string reason, string description);

        // Claim
        Task<List<SP_GET_CLAIM_LIST>> SP_GET_CLAIM_LIST(int? supplier_id, int? company_id, string from_date, string to_date, int? status_id);
        Task<SP_GET_CLAIM_DETAIL> SP_GET_CLAIM_DETAIL(string id, string supplier_id);
        Task<SP_POST_CLAIM_ORDER_CONFIRM> SP_UPDATE_CLAIM_ORDER_CONFIRM(string id, string status, string reason, string description);

        //Companies
        Task<List<SP_GET_COMPANIES_LIST>> SP_GET_COMPANIES_LIST(string supplier_id);
        Task<SP_GET_COMPANIES_DETAIL> SP_GET_COMPANIES_DETAIL(string id);
        Task<SP_PUT_CONNECT_COMPANIES_REQUEST> SP_PUT_CONNECT_COMPANIES_REQUEST(string id, string company_request_code);

        //Count
        Task<SP_GET_COUNT_PO_CLAIM> SP_GET_COUNT_PO_CLAIM();
    }
}