using System.Collections.Generic;
using System.Threading.Tasks;
using VendorPortal.Domain.Models.WolfApprove;
using VendorPortal.Domain.Models.WolfApprove.Store;

namespace VendorPortal.Domain.Interfaces.v1
{
    public interface IWolfApproveRepository
    {
        // Define your method signatures here
        Task<List<SP_GETRFQ>> SP_GETRFQ_LIST();
        Task<SP_GETRFQ_DETAIL> SP_GETRFQ_SHOW(string id);
        Task<List<SP_GETPURCHASE_ORDER>> SP_GETPURCHASE_ORDER_LIST();
    }
}