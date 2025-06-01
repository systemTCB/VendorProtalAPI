using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using VendorPortal.Domain.Interfaces.v1;
using VendorPortal.Domain.Models.WolfApprove.StoreModel;
using VendorPortal.Logging;

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

        public async Task<List<SP_GET_RFQ_DETAIL>> SP_GET_RFQ_DETAIL(string id)
        {
            List<SP_GET_RFQ_DETAIL> mock = new();
            try
            {
                string filename = "SP_GET_RFQ_DETAIL.json";
                string json = await File.ReadAllTextAsync(basePath + filename);
                mock = JsonConvert.DeserializeObject<List<SP_GET_RFQ_DETAIL>>(json);
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "MockWolfApproveRepository");
            }
            return mock;
        }

        public async Task<List<SP_GET_RFQ_LIST>> SP_GET_RFQ_LIST()
        {
            List<SP_GET_RFQ_LIST> mock = new();
            try
            {
                string filename = "SP_GET_RFQ_LIST.json";
                string json = await File.ReadAllTextAsync(basePath + filename);
                mock = JsonConvert.DeserializeObject<List<SP_GET_RFQ_LIST>>(json);
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "MockWolfApproveRepository");
            }
            return mock;
        }

        public async Task<List<SP_GET_PURCHASE_ORDER>> SP_GET_PURCHASE_ORDER_LIST()
        {
            var mock = new List<SP_GET_PURCHASE_ORDER>();
            try
            {
                string filename = "SP_GET_PURCHASE_ORDER_LIST.json";
                string json = await File.ReadAllTextAsync(basePath + filename);
                mock = JsonConvert.DeserializeObject<List<SP_GET_PURCHASE_ORDER>>(json);
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "MockWolfApproveRepository");
            }
            return mock;
        }

        public async Task<List<SP_GET_PURCHASE_ORDER_DETAIL>> SP_GET_PURCHASE_ORDER_DETAIL(string id, string supplier_id)
        {
            var mock = new List<SP_GET_PURCHASE_ORDER_DETAIL>();
            try
            {
                string filename = "SP_GET_PURCHASE_ORDER_DETAIL.json";
                string json = await File.ReadAllTextAsync(basePath + filename);
                mock = JsonConvert.DeserializeObject<List<SP_GET_PURCHASE_ORDER_DETAIL>>(json);
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "MockWolfApproveRepository");
            }
            return mock;
        }

        public async Task<SP_PUT_PURCHASE_ORDER_CONFIRM> SP_PUT_PURCHASE_ORDER_CONFIRM(string id, string status, string reason, string description)
        {
            var mock = new SP_PUT_PURCHASE_ORDER_CONFIRM();
            try
            {
                string filename = "SP_PUT_PURCHASE_ORDER_CONFIRM.json";
                string json = await File.ReadAllTextAsync(basePath + filename);
                mock = JsonConvert.DeserializeObject<SP_PUT_PURCHASE_ORDER_CONFIRM>(json);
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "MockWolfApproveRepository");
            }
            return mock;
        }

        public async Task<List<SP_GET_CLAIM_LIST>> SP_GET_CLAIM_LIST(int? supplier_id, int? company_id, string from_date, string to_date, int? status_id)
        {
            var mock = new List<SP_GET_CLAIM_LIST>();
            try
            {
                string filename = "SP_GET_CLAIM_LIST.json";
                string json = await File.ReadAllTextAsync(basePath + filename);
                mock = JsonConvert.DeserializeObject<List<SP_GET_CLAIM_LIST>>(json);
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "MockWolfApproveRepository");

            }
            return mock;
        }

        public async Task<SP_GET_CLAIM_DETAIL> SP_GET_CLAIM_DETAIL(string id, string supplier_id)
        {
            var mock = new SP_GET_CLAIM_DETAIL();
            try
            {
                string filename = "SP_GET_CLAIM_DETAIL.json";
                string json = await File.ReadAllTextAsync(basePath + filename);
                mock = JsonConvert.DeserializeObject<SP_GET_CLAIM_DETAIL>(json);
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "MockWolfApproveRepository");
            }
            return mock;
        }

        public async Task<SP_POST_CLAIM_ORDER_CONFIRM> SP_UPDATE_CLAIM_ORDER_CONFIRM(string id, string status, string reason, string description)
        {
            var mock = new SP_POST_CLAIM_ORDER_CONFIRM();
            try
            {
                string filename = "SP_UPDATE_CLAIM_ORDER_CONFIRM.json";
                string json = await File.ReadAllTextAsync(basePath + filename);
                mock = JsonConvert.DeserializeObject<SP_POST_CLAIM_ORDER_CONFIRM>(json);
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "MockWolfApproveRepository");
            }
            return mock;
        }

        public async Task<List<SP_GET_COMPANIES_LIST>> SP_GET_COMPANIES_LIST(string supplier_id)
        {
            var mock = new List<SP_GET_COMPANIES_LIST>();
            try
            {
                string filename = "SP_GET_COMPANIES_LIST.json";
                string json = await File.ReadAllTextAsync(basePath + filename);
                mock = JsonConvert.DeserializeObject<List<SP_GET_COMPANIES_LIST>>(json);
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "MockWolfApproveRepository");
            }
            return mock;
        }

        public async Task<SP_GET_COMPANIES_DETAIL> SP_GET_COMPANIES_DETAIL(string id)
        {
            var mock = new SP_GET_COMPANIES_DETAIL();
            try
            {
                string filename = "SP_GET_COMPANIES_DETAIL.json";
                string json = await File.ReadAllTextAsync(basePath + filename);
                mock = JsonConvert.DeserializeObject<SP_GET_COMPANIES_DETAIL>(json);
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "MockWolfApproveRepository");
            }
            return mock;
        }

        public async Task<SP_PUT_CONNECT_COMPANIES_REQUEST> SP_PUT_CONNECT_COMPANIES_REQUEST(string id, string Company_request_code)
        {
            var mock = new SP_PUT_CONNECT_COMPANIES_REQUEST();
            try
            {
                string filename = "SP_PUT_CONNECT_COMPANIES_REQUEST.json";
                string json = await File.ReadAllTextAsync(basePath + filename);
                mock = JsonConvert.DeserializeObject<SP_PUT_CONNECT_COMPANIES_REQUEST>(json);
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "MockWolfApproveRepository");
            }
            return mock;
        }

        public async Task<SP_GET_COUNT_PO_CLAIM> SP_GET_COUNT_PO_CLAIM()
        {
            var mock = new SP_GET_COUNT_PO_CLAIM();
            try
            {
                string filename = "SP_GET_COUNT_PO_CLAIM.json";
                string json = await File.ReadAllTextAsync(basePath + filename);
                mock = JsonConvert.DeserializeObject<SP_GET_COUNT_PO_CLAIM>(json);
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "MockWolfApproveRepository");
            }
            return mock;
        }

        public async Task<List<SP_GET_RFQ_DOCUMENT>> SP_GET_RFQ_DOCUMENT(string id)
        {
            var mock = new List<SP_GET_RFQ_DOCUMENT>();
            try
            {
                string filename = "SP_GET_RFQ_DOCUMENT.json";
                string json = await File.ReadAllTextAsync(basePath + filename);
                mock = JsonConvert.DeserializeObject<List<SP_GET_RFQ_DOCUMENT>>(json);
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "MockWolfApproveRepository");
            }
            return mock;
        }

        public async Task<List<SP_GET_RFQ_QUESTIONNAIRE_BY_RFQID>> SP_GET_RFQ_QUESTIONNAIRE_BY_RFQID(string id)
        {
            var mock = new List<SP_GET_RFQ_QUESTIONNAIRE_BY_RFQID>();
            try
            {
                string filename = "SP_GET_RFQ_QUESTIONNAIRE_BY_RFQID.json";
                string json = await File.ReadAllTextAsync(basePath + filename);
                mock = JsonConvert.DeserializeObject<List<SP_GET_RFQ_QUESTIONNAIRE_BY_RFQID>>(json);
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "MockWolfApproveRepository");
            }
            return mock;
        }

        public async Task<SP_PUT_QUOTATION> SP_PUT_QUOTATION(string rfq_id, string quo_number,string quo_id, string status, string reason)
        {
            var mock = new SP_PUT_QUOTATION();
            try
            {
                string filename = "SP_PUT_QUOTATION.json";
                string json = await File.ReadAllTextAsync(basePath + filename);
                mock = JsonConvert.DeserializeObject<SP_PUT_QUOTATION>(json);
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "MockWolfApproveRepository");
            }
            return mock;
        }

        public async Task<SP_CREATE_ITEM> SP_INSERT_NEWREQ_ITEMLINES(List<TEMP_RFQ_ITEM> rfq_items)
        {
            var mock = new SP_CREATE_ITEM();
            try
            {
                string filename = "SP_CREATE_RFQ_ITEM.json";
                string json = await File.ReadAllTextAsync(basePath + filename);
                mock = JsonConvert.DeserializeObject<SP_CREATE_ITEM>(json);
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "MockWolfApproveRepository");
            }
            return mock;
        }

        public async Task<SP_CREATE_RFQ> SP_INSERT_NEWRFQ(string rfq_number, int company_id, string company_name, string rfq_status, decimal sub_total, decimal discount, decimal total_amount, decimal net_amount, string payment_condition, string project_name, string project_description, int procurement_tyepe_id, string procurement_type_name, int catagory_id, string category_name, DateTime start_date, DateTime end_date, DateTime required_date, int status_id, string status_name, decimal contract_value, string remark, string created_by)
        {
            var mock = new SP_CREATE_RFQ();
            try
            {
                string filename = "SP_CREATE_RFQ.json";
                string json = await File.ReadAllTextAsync(basePath + filename);
                mock = JsonConvert.DeserializeObject<SP_CREATE_RFQ>(json);
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "MockWolfApproveRepository");
            }
            return mock;
        }

        public async Task<SP_CREATE_QUESTIONNAIRE> SP_INSERT_NEWRFQ_QUESTIONNAIRE(List<TEMP_RFQ_QUESTIONNAIRE> rfq_questionnaires)
        {
            var mock = new SP_CREATE_QUESTIONNAIRE();
            try
            {
                string filename = "SP_INSERT_NEWRFQ_QUESTIONNAIRE.json";
                string json = await File.ReadAllTextAsync(basePath + filename);
                mock = JsonConvert.DeserializeObject<SP_CREATE_QUESTIONNAIRE>(json);
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "MockWolfApproveRepository");
            }
            return mock;
        }

        public async Task<SP_CREATE_DOCUMENT> SP_INSERT_NEWRFQ_DOCUMENT(List<TEMP_RFQ_DOCUMENT> rfq_documents)
        {
            var mock = new SP_CREATE_DOCUMENT();
            try
            {
                string filename = "SP_INSERT_NEWRFQ_QUESTIONNAIRE.json";
                string json = await File.ReadAllTextAsync(basePath + filename);
                mock = JsonConvert.DeserializeObject<SP_CREATE_DOCUMENT>(json);
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "MockWolfApproveRepository");
            }
            return mock;
        }

        public async Task<SP_PUT_QUOTATION> SP_PUT_QUOTATION_CREATE(string rfq_id, string quo_number, string quo_id, string status, string reason)
        {
            var mock = new SP_PUT_QUOTATION();
            try
            {
                string filename = "SP_INSERT_NEWRFQ_QUESTIONNAIRE.json";
                string json = await File.ReadAllTextAsync(basePath + filename);
                mock = JsonConvert.DeserializeObject<SP_PUT_QUOTATION>(json);
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, "MockWolfApproveRepository");
            }
            
            return mock;
        }
    }
}