using System.Collections.Generic;
using System.Threading.Tasks;
using VendorPortal.Domain.Models.WolfApprove.StoreModel;

namespace VendorPortal.Domain.Interfaces.v1
{
    public interface IMasterDataRepository
    {
        // Define your method signatures here
        Task<List<SP_GET_MASTER_COMPANY>> SP_GET_MASTER_COMPANY(bool isShowAll);
    }
}