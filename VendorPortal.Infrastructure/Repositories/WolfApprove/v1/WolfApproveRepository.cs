using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.Server;
using VendorPortal.Domain.Interfaces.v1;
using VendorPortal.Domain.Models.WolfApprove.StoreModel;
using VendorPortal.Infrastructure.Extensions;
using VendorPortal.Logging;

namespace VendorPortal.Infrastructure.Repositories.WolfApprove.v1
{
    public class WolfApproveRepository : IWolfApproveRepository
    {
        private readonly DbContext _context;
        public WolfApproveRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<List<SP_GET_RFQ_DETAIL>> SP_GET_RFQ_DETAIL(string id)
        {
            List<SP_GET_RFQ_DETAIL> result = new List<SP_GET_RFQ_DETAIL>();
            try
            {
                using (var connection = _context.CreateConnectionRead())
                {
                    connection.Open();
                    var sql = "SP_GET_RFQ_DETAIL";
                    var param = new SqlParameter[] { new SqlParameter("@nRFQID", id) };
                    result = await _context.ExcuteStoreQueryListAsync<SP_GET_RFQ_DETAIL>(sql, param);
                }
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "WolfApproveRepository");
                return result;
            }
            return result;
        }

        public async Task<List<SP_GET_RFQ_LIST>> SP_GET_RFQ_LIST()
        {
            List<SP_GET_RFQ_LIST> result = new List<SP_GET_RFQ_LIST>();
            try
            {
                using (var connection = _context.CreateConnectionRead())
                {
                    connection.Open();
                    var sql = "SP_GET_RFQ_LIST";
                    var param = new SqlParameter[] { };
                    result = await _context.ExcuteStoreQueryListAsync<SP_GET_RFQ_LIST>(sql, param);
                }
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "WolfApproveRepository");
                return result;
            }
            return result;
        }

        public async Task<List<SP_GET_RFQ_DOCUMENT>> SP_GET_RFQ_DOCUMENT(string id)
        {
            List<SP_GET_RFQ_DOCUMENT> result = new List<SP_GET_RFQ_DOCUMENT>();
            try
            {
                using (var connection = _context.CreateConnectionRead())
                {
                    connection.Open();
                    var sql = "SP_GET_RFQ_DOCUMENT";
                    var param = new SqlParameter[] { new SqlParameter("@nRFQID", id) };
                    result = await _context.ExcuteStoreQueryListAsync<SP_GET_RFQ_DOCUMENT>(sql, param);
                }
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "SP_GET_RFQ_DOCUMENT");
                return result;
            }
            return result;
        }

        public async Task<List<SP_GET_PURCHASE_ORDER>> SP_GET_PURCHASE_ORDER_LIST()
        {
            List<SP_GET_PURCHASE_ORDER> result = new List<SP_GET_PURCHASE_ORDER>();
            try
            {
                using (var connection = _context.CreateConnectionRead())
                {
                    connection.Open();
                    var sql = "SP_GET_PURCHASE_ORDER_LIST";
                    var param = new SqlParameter[] { };
                    result = await _context.ExcuteStoreQueryListAsync<SP_GET_PURCHASE_ORDER>(sql, param);
                }
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "SP_GET_PURCHASE_ORDER_LIST");
                return result;
            }
            return result;
        }

        public async Task<List<SP_GET_PURCHASE_ORDER_DETAIL>> SP_GET_PURCHASE_ORDER_DETAIL(string id, string supplier_id)
        {
            List<SP_GET_PURCHASE_ORDER_DETAIL> result = new List<SP_GET_PURCHASE_ORDER_DETAIL>();
            try
            {
                using (var connection = _context.CreateConnectionRead())
                {
                    connection.Open();
                    var sql = "SP_GET_PURCHASE_ORDER_DETAIL";
                    var param = new SqlParameter[]
                    {
                        new SqlParameter("@POCode", id),
                        new SqlParameter("@CompanyID", supplier_id) // xx จะ where supplier จากไหนหว้า ????
                    };
                    result = await _context.ExcuteStoreQueryListAsync<SP_GET_PURCHASE_ORDER_DETAIL>(sql, param);
                }
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "SP_GET_PURCHASE_ORDER_DETAIL");
                return result;
            }
            return result;
        }

        public Task<SP_PUT_PURCHASE_ORDER_CONFIRM> SP_PUT_PURCHASE_ORDER_CONFIRM(string id, string status, string reason, string description)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<SP_GET_CLAIM_LIST>> SP_GET_CLAIM_LIST(string supplier_id, string company_id, string from_date, string to_date)
        {
            throw new System.NotImplementedException();
        }

        public Task<SP_GET_CLAIM_DETAIL> SP_GET_CLAIM_DETAIL(string id, string supplier_id)
        {
            throw new System.NotImplementedException();
        }

        public Task<SP_POST_CLAIM_ORDER_CONFIRM> SP_UPDATE_CLAIM_ORDER_CONFIRM(string id, string status, string reason, string description)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<SP_GET_COMPANIES_LIST>> SP_GET_COMPANIES_LIST(string supplier_id)
        {
            throw new System.NotImplementedException();
        }

        public Task<SP_GET_COMPANIES_DETAIL> SP_GET_COMPANIES_DETAIL(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<SP_PUT_CONNECT_COMPANIES_REQUEST> SP_PUT_CONNECT_COMPANIES_REQUEST(string id, string company_request_code)
        {
            throw new System.NotImplementedException();
        }

        public Task<SP_GET_COUNT_PO_CLAIM> SP_GET_COUNT_PO_CLAIM()
        {
            throw new System.NotImplementedException();
        }


    }
}