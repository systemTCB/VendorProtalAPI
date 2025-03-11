using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using VendorPortal.Domain.Interfaces.v1;
using VendorPortal.Domain.Models.WolfApprove.StoreModel;
using VendorPortal.Infrastructure.Extensions;
using VendorPortal.Logging;

namespace VendorPortal.Infrastructure.Repositories.WolfApprove.v1
{
    public class MasterDataRepository : IMasterDataRepository
    {
        // Add your methods and properties here
        private readonly DbContext _dbContext;
        public MasterDataRepository(DbContext dbContext)
        {
            // Add your constructor code here
            _dbContext = dbContext;
        }

        public async Task<List<SP_GET_MASTER_COMPANY>> SP_GET_MASTER_COMPANY(bool isShowAll)
        {
            List<SP_GET_MASTER_COMPANY> data = new List<SP_GET_MASTER_COMPANY>();
            try
            {
                using (var connection = _dbContext.CreateConnectionRead())
                {
                    connection.Open();
                    var sql = "SP_GET_MASTER_COMPANY";
                    var param = new SqlParameter[] { new SqlParameter("@isShowAll", isShowAll) };
                    data = await _dbContext.ExcuteStoreQueryListAsync<SP_GET_MASTER_COMPANY>(sql, param);
                }
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "SP_GET_MASTER_COMPANY");
            }
            return data;
        }
    }
}