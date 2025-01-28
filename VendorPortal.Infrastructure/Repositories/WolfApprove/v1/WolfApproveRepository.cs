using System.Collections.Generic;
using System.Threading.Tasks;
using VendorPortal.Domain.Interfaces.v1;
using VendorPortal.Domain.Models.WolfApprove;
using VendorPortal.Domain.Models.WolfApprove.Store;
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
        public Task<SP_GETRFQ_DETAIL> SP_GETRFQ_SHOW(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<SP_GETRFQ>> SP_GETRFQ_LIST()
        {
            throw new System.NotImplementedException();
        }

        public Task<List<SP_GETPURCHASE_ORDER>> SP_GETPURCHASE_ORDER_LIST()
        {
            throw new System.NotImplementedException();
        }

        public Task<SP_GETPURCHASE_ORDER_DETAIL> SP_GETPURCHASE_ORDER_SHOW(string id , string supplier_id)
        {
            throw new System.NotImplementedException();
        }

        public Task<SP_PUTPURCHASE_ORDER_CONFIRM> SP_PUTPURCHASE_ORDER_CONFIRM(string id, string status, string reason, string description)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<SP_GETCLAIM_LIST>> SP_GETCLAIM_LIST()
        {
            throw new System.NotImplementedException();
        }

        public Task<SP_GETCLAIM_DETAIL> SP_GETCLAIM_SHOW(string id, string supplier_id)
        {
            throw new System.NotImplementedException();
        }

        public Task<SP_PUTCLAIM_ORDER_CONFIRM> SP_PUTCLAIM_ORDER_CONFIRM(string id, string status, string reason, string description)
        {
            throw new System.NotImplementedException();
        }
    }
}