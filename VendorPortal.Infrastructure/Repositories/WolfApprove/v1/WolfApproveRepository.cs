using System.Collections.Generic;
using System.Threading.Tasks;
using VendorPortal.Domain.Interfaces.v1;
using VendorPortal.Domain.Models.WolfApprove;
using VendorPortal.Domain.Models.WolfApprove.Store;
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
    }
}