using System.Collections.Generic;
using System.Threading.Tasks;
using VendorPortal.Domain.Interfaces.v1;
using VendorPortal.Domain.Models.WolfApprove.StoreModel;
using VendorPortal.Infrastructure.Extensions;

namespace VendorPortal.Infrastructure.Repositories.WolfApprove.v1
{
    public class WolfApproveRepository : IWolfApproveRepository
    {
        private readonly DbContext _context;
        public WolfApproveRepository()
        {
            
        }
        // Add your repository methods here
        public Task<SP_GET_RFQ_DETAIL> SP_GETRFQ_SHOW(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<SP_GET_RFQ>> SP_GETRFQ_LIST()
        {
            throw new System.NotImplementedException();
        }

        public Task<List<SP_GET_PURCHASE_ORDER>> SP_GET_PURCHASE_ORDER_LIST()
        {
            throw new System.NotImplementedException();
        }

        public Task<SP_GET_PURCHASE_ORDER_DETAIL> SP_GET_PURCHASE_ORDER_SHOW(string id , string supplier_id)
        {
            throw new System.NotImplementedException();
        }

        public Task<SP_PUT_PURCHASE_ORDER_CONFIRM> SP_PUT_PURCHASE_ORDER_CONFIRM(string id, string status, string reason, string description)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<SP_GET_CLAIM_LIST>> SP_GET_CLAIM_LIST()
        {
            throw new System.NotImplementedException();
        }

        public Task<SP_GET_CLAIM_DETAIL> SP_GET_CLAIM_SHOW(string id, string supplier_id)
        {
            throw new System.NotImplementedException();
        }

        public Task<SP_POST_CLAIM_ORDER_CONFIRM> SP_PUT_CLAIM_ORDER_CONFIRM(string id, string status, string reason, string description)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<SP_GET_COMPANIES_LIST>> SP_GET_COMPANY_LIST()
        {
            throw new System.NotImplementedException();
        }

        public Task<SP_GET_COMPANIES_BY_ID> SP_GET_COMPANIES_BY_ID(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<SP_PUT_CONNECT_COMPANIES_REQUEST> SP_PUT_CONNECT_COMPANIES_REQUEST(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<SP_GET_COUNT_PO_CLAIM> SP_GET_COUNT_PO_CLAIM()
        {
            throw new System.NotImplementedException();
        }
    }
}