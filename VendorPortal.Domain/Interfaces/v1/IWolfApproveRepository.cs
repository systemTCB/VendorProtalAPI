using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VendorPortal.Domain.Models.WolfApprove.StoreModel;

namespace VendorPortal.Domain.Interfaces.v1
{
    public interface IWolfApproveRepository
    {
        // RFQ
        Task<List<SP_GET_RFQ_LIST>> SP_GET_RFQ_LIST();
        Task<List<SP_GET_RFQ_DETAIL>> SP_GET_RFQ_DETAIL(string id);
        Task<List<SP_GET_RFQ_DOCUMENT>> SP_GET_RFQ_DOCUMENT(string id);

        Task<SP_CREATE_RFQ> SP_INSERT_NEWRFQ(
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
            int catagory_id,
            string category_name,
            DateTime start_date,
            DateTime end_date,
            DateTime required_date,
            int status_id,
            string status_name,
            decimal contract_value,
            string remark,
            string created_by,
            string is_specific
        );

        Task<SP_CREATE_ITEM> SP_INSERT_NEWREQ_ITEMLINES(List<TEMP_RFQ_ITEM> rfq_items);
        Task<SP_CREATE_QUESTIONNAIRE> SP_INSERT_NEWRFQ_QUESTIONNAIRE(List<TEMP_RFQ_QUESTIONNAIRE> rfq_questionnaires);
        Task<SP_CREATE_DOCUMENT> SP_INSERT_NEWRFQ_DOCUMENT(List<TEMP_RFQ_DOCUMENT> rfq_documents);
        Task<List<SP_GET_RFQ_QUESTIONNAIRE_BY_RFQID>> SP_GET_RFQ_QUESTIONNAIRE_BY_RFQID(string id);

        // Purchase Order
        Task<List<SP_GET_PURCHASE_ORDER>> SP_GET_PURCHASE_ORDER_LIST();
        Task<List<SP_GET_PURCHASE_ORDER_DETAIL>> SP_GET_PURCHASE_ORDER_DETAIL(string id, string supplier_id);
        Task<SP_PUT_PURCHASE_ORDER_CONFIRM> SP_PUT_PURCHASE_ORDER_CONFIRM(string id, string status, string reason, string description);

        // Claim
        Task<List<SP_GET_CLAIM_LIST>> SP_GET_CLAIM_LIST(int? supplier_id, int? company_id, string from_date, string to_date, int? status_id);
        Task<SP_GET_CLAIM_DETAIL> SP_GET_CLAIM_DETAIL(string id, string supplier_id);
        Task<SP_POST_CLAIM_ORDER_CONFIRM> SP_UPDATE_CLAIM_ORDER_CONFIRM(string id, string status, string reason, string description);

        //Companies
        Task<List<SP_GET_COMPANIES_LIST>> SP_GET_COMPANIES_LIST(string supplier_id);
        Task<SP_GET_COMPANIES_DETAIL> SP_GET_COMPANIES_DETAIL(string id);
        Task<SP_PUT_CONNECT_COMPANIES_REQUEST> SP_PUT_CONNECT_COMPANIES_REQUEST(string id, string company_request_code);

        //Count
        Task<SP_GET_COUNT_PO_CLAIM> SP_GET_COUNT_PO_CLAIM();

        //Quotation
        Task<SP_PUT_QUOTATION> SP_PUT_QUOTATION(string rfq_id, string quo_number, string quo_id, string status, string reason);
        Task<SP_PUT_QUOTATION> SP_PUT_QUOTATION_CREATE(string rfq_id, string quo_number, string quo_id, string status, string reason);
    }
}