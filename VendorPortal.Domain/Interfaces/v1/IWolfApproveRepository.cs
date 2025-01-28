using System.Collections.Generic;
using System.Threading.Tasks;
using VendorPortal.Domain.Models.WolfApprove;
using VendorPortal.Domain.Models.WolfApprove.Store;
using VendorPortal.Domain.Models.WolfApprove.StoreModel;

namespace VendorPortal.Domain.Interfaces.v1
{
    public interface IWolfApproveRepository
    {
        // RFQ
        Task<List<SP_GETRFQ>> SP_GETRFQ_LIST();
        Task<SP_GETRFQ_DETAIL> SP_GETRFQ_SHOW(string id);
        
        // Purchase Order
        Task<List<SP_GETPURCHASE_ORDER>> SP_GETPURCHASE_ORDER_LIST();
        Task<SP_GETPURCHASE_ORDER_DETAIL> SP_GETPURCHASE_ORDER_SHOW(string id , string supplier_id);
        Task<SP_PUTPURCHASE_ORDER_CONFIRM> SP_PUTPURCHASE_ORDER_CONFIRM(string id , string status , string reason , string description);

        // Claim
        Task<List<SP_GETCLAIM_LIST>> SP_GETCLAIM_LIST();
        Task<SP_GETCLAIM_DETAIL> SP_GETCLAIM_SHOW(string id , string supplier_id);
        Task<SP_PUTCLAIM_ORDER_CONFIRM> SP_PUTCLAIM_ORDER_CONFIRM(string id , string status , string reason , string description);
    }
}