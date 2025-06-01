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

        public async Task<List<SP_GET_RFQ_QUESTIONNAIRE_BY_RFQID>> SP_GET_RFQ_QUESTIONNAIRE_BY_RFQID(string id)
        {
            List<SP_GET_RFQ_QUESTIONNAIRE_BY_RFQID> result = new List<SP_GET_RFQ_QUESTIONNAIRE_BY_RFQID>();
            try
            {
                using (var connection = _context.CreateConnectionRead())
                {
                    connection.Open();
                    var sql = "SP_GET_RFQ_QUESTIONNAIRE_BY_RFQID";
                    var param = new SqlParameter[] { new SqlParameter("@RFQID", id) };
                    result = await _context.ExcuteStoreQueryListAsync<SP_GET_RFQ_QUESTIONNAIRE_BY_RFQID>(sql, param);
                }
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "SP_GET_RFQ_QUESTIONNAIRE_BY_RFQID");
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

        public async Task<List<SP_GET_CLAIM_LIST>> SP_GET_CLAIM_LIST(int? supplier_id, int? company_id, string from_date, string to_date, int? status_id)
        {
            List<SP_GET_CLAIM_LIST> result = new List<SP_GET_CLAIM_LIST>();
            try
            {
                using (var connection = _context.CreateConnectionRead())
                {
                    connection.Open();
                    var sql = "SP_GET_CLAIM_LIST";
                    var param = new SqlParameter[]
                    {
                        new SqlParameter("@supplier_id", supplier_id),
                        new SqlParameter("@company_id", company_id),
                        new SqlParameter("@status", status_id ),
                        new SqlParameter("@from_date", from_date),
                        new SqlParameter("@to_date", to_date)
                    };
                    result = await _context.ExcuteStoreQueryListAsync<SP_GET_CLAIM_LIST>(sql, param);
                }
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "SP_GET_CLAIM_LIST");
            }
            return result;
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

        public async Task<SP_PUT_QUOTATION> SP_PUT_QUOTATION(string rfq_id, string quo_number, string quo_id, string status, string reason)
        {
            try
            {
                using (var connection = _context.CreateConnectionRead())
                {
                    connection.Open();
                    var sql = "SP_PUT_QUOTATION";
                    var param = new SqlParameter[]
                    {
                        new("@RFQID", rfq_id),
                        new("@QuotationNumber", quo_number),
                        new("@QuotationID", quo_id),
                        new("@Status", status),
                        new("@Reason", reason)
                    };
                    var sp_response = await _context.ExecuteStoreNonQueryAsync(sql, param);
                    return new SP_PUT_QUOTATION
                    {
                        result = sp_response.isSuccess,
                        message = sp_response.message,
                    };
                }
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "WolfApproveRepository");
                return new SP_PUT_QUOTATION
                {
                    result = false,
                    message = ex.Message,
                };
            }
        }

        public async Task<SP_PUT_QUOTATION> SP_PUT_QUOTATION_CREATE(string rfq_id, string quo_number, string quo_id, string status, string reason)
        {
            try
            {
                using (var connection = _context.CreateConnectionRead())
                {
                    connection.Open();
                    var sql = "SP_PUT_QUOTATION_CREATE";
                    var param = new SqlParameter[]
                    {
                        new("@RFQID", rfq_id),
                        new("@QuotationNumber", quo_number),
                        new("@QuotationId", quo_id)
                    };
                    var sp_response = await _context.ExecuteStoreNonQueryAsync(sql, param);
                    return new SP_PUT_QUOTATION
                    {
                        result = sp_response.isSuccess,
                        message = sp_response.message,
                    };
                }
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "WolfApproveRepository");
                return new SP_PUT_QUOTATION
                {
                    result = false,
                    message = ex.Message,
                };
            }
        }
        public async Task<SP_CREATE_RFQ> SP_INSERT_NEWRFQ(
            string rfq_number,
            int company_id,
            string company_name,
            string rfq_status,
            decimal sub_total,
            decimal discount,
            decimal total_amount,
            decimal net_amount,
            string payment_condition,
            string project_name,
            string project_description,
            int procurement_tyepe_id,
            string procurement_type_name,
            int category_id,
            string procurement_category_name,
            DateTime start_date,
            DateTime end_date,
            DateTime required_date,
            int status_id,
            string status_name,
            decimal contract_value,
            string remark,
            string created_by
        )
        {
            try
            {
                using (var connection = _context.CreateConnectionRead())
                {
                    connection.Open();
                    var sql = "SP_INSERT_NEWRFQ";
                    var param = new SqlParameter[]
                    {
                        new SqlParameter("@sRFQNUMBER", rfq_number),
                        new SqlParameter("@nCompanyID", company_id),
                        new SqlParameter("@sCompanyName", company_name),
                        new SqlParameter("@sRFQStatus", rfq_status),
                        new SqlParameter("@dSubTotal", sub_total),
                        new SqlParameter("@dDiscount", discount),
                        new SqlParameter("@dTotalAmount", total_amount),
                        new SqlParameter("@dNetAmount", net_amount),
                        new SqlParameter("@sPaymentCondition", payment_condition),
                        new SqlParameter("@sProjectName", project_name),
                        new SqlParameter("@sProjectDescption", project_description),
                        new SqlParameter("@nProcurementTypeID", procurement_tyepe_id),
                        new SqlParameter("@sProcurementTypeName", procurement_type_name),
                        new SqlParameter("@nCategoryID", category_id),
                        new SqlParameter("@sCategoryName", procurement_category_name),
                        new SqlParameter("@dStartDate", start_date),
                        new SqlParameter("@dEndDate", end_date),
                        new SqlParameter("@dRequireDate", required_date),
                        new SqlParameter("@nStatusID", status_id),
                        new SqlParameter("@sStatusName", status_name),
                        new SqlParameter("@sContractValue", contract_value),
                        new SqlParameter("@sRemark", remark),
                        new SqlParameter("@sCreatedBy", created_by),
                    };
                    var sp_response = await _context.ExcuteStoreQuerySingleAsync<SP_CREATE_RFQ>(sql, param);
                    return new SP_CREATE_RFQ
                    {
                        Result = sp_response.Result,
                        Message = sp_response.Message,
                        RFQID = sp_response.RFQID,
                    };
                }
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "WolfApproveRepository");
                return new SP_CREATE_RFQ
                {
                    Result = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<SP_CREATE_ITEM> SP_INSERT_NEWREQ_ITEMLINES(List<TEMP_RFQ_ITEM> rfq_items)
        {
            try
            {
                using (var connection = _context.CreateConnectionRead())
                {
                    connection.Open();
                    var sql = "SP_INSERT_NEWREQ_ITEMLINES";

                    var param = new SqlParameter[]
                    {
                        new SqlParameter("@RFQItems", rfq_items.ConvertToDataTable())
                    };
                    var sp_response = await _context.ExecuteStoreNonQueryAsync(sql, param);

                    return new SP_CREATE_ITEM
                    {
                        Result = sp_response.isSuccess,
                        Message = sp_response.message,
                    };
                }
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "WolfApproveRepository");
                return new SP_CREATE_ITEM
                {
                    Result = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<SP_CREATE_QUESTIONNAIRE> SP_INSERT_NEWRFQ_QUESTIONNAIRE(List<TEMP_RFQ_QUESTIONNAIRE> rfq_questionnaires)
        {
            try
            {
                using (var connection = _context.CreateConnectionRead())
                {
                    connection.Open();
                    var sql = "SP_INSERT_NEWRFQ_QUESTIONNAIRE";

                    var param = new SqlParameter[]
                    {
                        new SqlParameter("@RFQQuestions", rfq_questionnaires.ConvertToDataTable())
                    };
                    var sp_response = await _context.ExecuteStoreNonQueryAsync(sql, param);

                    return new SP_CREATE_QUESTIONNAIRE
                    {
                        Result = sp_response.isSuccess,
                        Message = sp_response.message,
                    };
                }
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "WolfApproveRepository");
                return new SP_CREATE_QUESTIONNAIRE
                {
                    Result = false,
                    Message = ex.Message,
                };
            }
        }

        public async Task<SP_CREATE_DOCUMENT> SP_INSERT_NEWRFQ_DOCUMENT(List<TEMP_RFQ_DOCUMENT> rfq_documents)
        {
            try
            {
                using (var connection = _context.CreateConnectionRead())
                {
                    connection.Open();
                    var sql = "SP_INSERT_NEWRFQ_DOCUMENT";

                    var param = new SqlParameter[]
                    {
                        new SqlParameter("@Documents", rfq_documents.ConvertToDataTable())
                    };
                    var sp_response = await _context.ExecuteStoreNonQueryAsync(sql, param);

                    return new SP_CREATE_DOCUMENT
                    {
                        Result = sp_response.isSuccess,
                        Message = sp_response.message,
                    };
                }
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "WolfApproveRepository");
                return new SP_CREATE_DOCUMENT
                {
                    Result = false,
                    Message = ex.Message,
                };
            }
        }


    }
}