using System.Collections.Generic;
using System.Threading.Tasks;
using VendorPortal.Domain.Interfaces.v1;
using VendorPortal.Domain.Models.WolfApprove.StoreModel;
using VendorPortal.Infrastructure.Extensions;

namespace VendorPortal.Infrastructure.Repositories.WolfApprove.v1
{
    public class WolfApproveRepository : IWolfApproveRepository
    {
        //private readonly DbContext _context;
        public WolfApproveRepository()
        {
        }
        public Task<SP_GET_RFQ_DETAIL> SP_GET_RFQ_DETAIL(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<SP_GET_RFQ>> SP_GET_RFQ_LIST()
        {
            throw new System.NotImplementedException();
        }

        public Task<List<SP_GET_PURCHASE_ORDER>> SP_GET_PURCHASE_ORDER_LIST()
        {
            throw new System.NotImplementedException();
        }

        public Task<SP_GET_PURCHASE_ORDER_DETAIL> SP_GET_PURCHASE_ORDER_DETAIL(string id , string supplier_id)
        {
            throw new System.NotImplementedException();
        }

        public Task<SP_PUT_PURCHASE_ORDER_CONFIRM> SP_PUT_PURCHASE_ORDER_CONFIRM(string id, string status, string reason, string description)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<SP_GET_CLAIM_LIST>> SP_GET_CLAIM_LIST(string supplier_id , string company_id , string from_date , string to_date)
        {
            throw new System.NotImplementedException();
        }

        public Task<SP_GET_CLAIM_DETAIL> SP_GET_CLAIM_DETAIL(string id, string supplier_id)
        {
            throw new System.NotImplementedException();
        }

        public Task<SP_POST_CLAIM_ORDER_CONFIRM> SP_UPDATE_CLAIM_ORDER_CONFIRM(string id, string status, string reason, string description)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<SP_GET_COMPANIES_LIST>> SP_GET_COMPANIES_LIST( string supplier_id)
        {
            throw new System.NotImplementedException();
        }

        public Task<SP_GET_COMPANIES_DETAIL> SP_GET_COMPANIES_DETAIL(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<SP_PUT_CONNECT_COMPANIES_REQUEST> SP_PUT_CONNECT_COMPANIES_REQUEST(string id , string company_request_code)
        {
            throw new System.NotImplementedException();
        }

        public Task<SP_GET_COUNT_PO_CLAIM> SP_GET_COUNT_PO_CLAIM()
        {
            throw new System.NotImplementedException();
        }
    }
}