using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using VendorPortal.Domain.Models.KubBoss.v1.Response;
using Newtonsoft.Json;
using System;
using VendorPortal.Domain.Interfaces.v1;
using Microsoft.AspNetCore.Http;
using System.Net.Http;

namespace VendorPortal.Infrastructure.Mock.ThaiRedCross.v1.Repository
{
    public class MockVendorPortalRepository : IVendorPortalRepository
    {
        private readonly string basePath;
        public MockVendorPortalRepository()
        {
            basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/Mock/VendorPortal/v1/Data/";
        }
        public async Task<GetPurchaseOrderExResponse> GetPurchaseOrder(string PONumber)
        {
            GetPurchaseOrderExResponse mock = new();
            try
            {
                string file = "GetPurchaseOrder.json";
                string json = await File.ReadAllTextAsync(basePath + file);
                mock = JsonConvert.DeserializeObject<GetPurchaseOrderExResponse>(json);
            }
            catch(Exception ex)
            {
                string file = "ExInternalError.json";
                string json = await File.ReadAllTextAsync(basePath + file);
                mock.status.code = StatusCodes.Status500InternalServerError.ToString();
                mock.status.message = ex.Message;
                mock = JsonConvert.DeserializeObject<GetPurchaseOrderExResponse>(json);
            }
            return mock;
        }
    
    
    }
}