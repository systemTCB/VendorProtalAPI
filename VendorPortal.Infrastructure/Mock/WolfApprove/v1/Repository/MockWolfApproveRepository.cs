using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using VendorPortal.Domain.Interfaces.v1;
using VendorPortal.Domain.Models.WolfApprove;
using VendorPortal.Domain.Models.WolfApprove.Store;

namespace VendorPortal.Infrastructure.Mock.WolfApprove.v1.Repository
{
    public class MockWolfApproveRepository : IWolfApproveRepository
    {
        private readonly string basePath;
        public MockWolfApproveRepository()
        {
            basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/Mock/WolfApprove/v1/Data/";
            // Constructor logic here
        }

        public async Task<SP_GETRFQ_DETAIL> SP_GETRFQ_SHOW(string id)
        {
            SP_GETRFQ_DETAIL mock = new();
            try
            {
                string filename = "SP_GETRFQ_DETAIL.json";
                string json = await File.ReadAllTextAsync(basePath + filename);
                mock = JsonConvert.DeserializeObject<SP_GETRFQ_DETAIL>(json);
            }
            catch (System.Exception ex)
            {
            }
            return mock;
        }

        public async Task<List<SP_GETRFQ>> SP_GETRFQ_LIST()
        {
            List<SP_GETRFQ> mock = new();
            try
            {
                string filename = "SP_GETRFQ_LIST.json";
                string json = await File.ReadAllTextAsync(basePath + filename);
                mock = JsonConvert.DeserializeObject<List<SP_GETRFQ>>(json);
            }
            catch (System.Exception ex)
            {
            }
            return mock;
        }
    }
}