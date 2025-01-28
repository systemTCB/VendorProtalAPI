using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using VendorPortal.Domain.Interfaces.v1;
using VendorPortal.Domain.Models.WolfApprove;
using VendorPortal.Domain.Models.WolfApprove.Store;
using VendorPortal.Domain.Models.WolfApprove.StoreModel;

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

        public async Task<List<SP_GETPURCHASE_ORDER>> SP_GETPURCHASE_ORDER_LIST()
        {
            var mock = new List<SP_GETPURCHASE_ORDER>();
            try
            {
                string filename = "";
                string json = await File.ReadAllTextAsync(basePath + filename);
                mock = JsonConvert.DeserializeObject<List<SP_GETPURCHASE_ORDER>>(json);
            }
            catch (System.Exception ex)
            {
                throw;
            }
            return mock;
        }

        public async Task<SP_GETPURCHASE_ORDER_DETAIL> SP_GETPURCHASE_ORDER_SHOW(string id , string supplier_id)
        {
            var mock = new SP_GETPURCHASE_ORDER_DETAIL();
            try
            {
                string filename = "";
                string json = await File.ReadAllTextAsync(basePath + filename);
                mock = JsonConvert.DeserializeObject<SP_GETPURCHASE_ORDER_DETAIL>(json);
            }
            catch (System.Exception ex)
            {
                throw;
            }
            return mock;
        }

        public async Task<SP_PUTPURCHASE_ORDER_CONFIRM> SP_PUTPURCHASE_ORDER_CONFIRM(string id, string status, string reason, string description)
        {
            var mock = new SP_PUTPURCHASE_ORDER_CONFIRM();
            try
            {
                string filename = "";
                string json = await File.ReadAllTextAsync(basePath + filename);
                mock = JsonConvert.DeserializeObject<SP_PUTPURCHASE_ORDER_CONFIRM>(json);
            }
            catch (System.Exception ex)
            {
                throw;
            }
            return mock;
        }

        public async Task<List<SP_GETCLAIM_LIST>> SP_GETCLAIM_LIST()
        {
            var mock = new List<SP_GETCLAIM_LIST>();
            try
            {
                string filename = "";
                string json = await File.ReadAllTextAsync(basePath + filename);
                mock = JsonConvert.DeserializeObject<List<SP_GETCLAIM_LIST>>(json);
            }
            catch (System.Exception ex)
            {
                throw;
            }
            return mock;
        }

        public async Task<SP_GETCLAIM_DETAIL> SP_GETCLAIM_SHOW(string id, string supplier_id)
        {
            var mock = new SP_GETCLAIM_DETAIL();
            try
            {
                string filename = "";
                string json = await File.ReadAllTextAsync(basePath + filename);
                mock = JsonConvert.DeserializeObject<SP_GETCLAIM_DETAIL>(json);
            }
            catch (System.Exception ex)
            {
                throw;
            }
            return mock;
        }

        public async Task<SP_PUTCLAIM_ORDER_CONFIRM> SP_PUTCLAIM_ORDER_CONFIRM(string id, string status, string reason, string description)
        {
            var mock = new SP_PUTCLAIM_ORDER_CONFIRM();
            try
            {
                string filename = "";
                string json = await File.ReadAllTextAsync(basePath + filename);
                mock = JsonConvert.DeserializeObject<SP_PUTCLAIM_ORDER_CONFIRM>(json);
            }
            catch (System.Exception ex)
            {
                throw;
            }
            return mock;
        }
    }
}