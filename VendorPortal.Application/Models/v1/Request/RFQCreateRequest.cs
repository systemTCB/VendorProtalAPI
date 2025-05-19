using System;
using System.Collections.Generic;

namespace VendorPortal.Application.Models.v1.Request
{
    public class RFQCreateRequest
    {
        public string rfq_number { get; set; }
        public int company_id { get; set; }
        public decimal sub_total { get; set; }
        public decimal discount { get; set; }
        public decimal total_amount { get; set; }
        public decimal net_amount { get; set; }
        public string payment_condition { get; set; }
        public string project_name { get; set; }
        public string project_description { get; set; }
        public int? procurement_type_id { get; set; }
        public int? procurement_category_id { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public DateTime required_date { get; set; }
        public decimal contract_value { get; set; }
        public List<RFQItemData> items { get; set; }
        public string remark { get; set; }
        public string rfq_status { get; set; }
        public string created_by { get; set; }
        public List<RFQCreateQuestionnaire> questionaires { get; set; } = new List<RFQCreateQuestionnaire>();
        public List<RFQCreateDocument> attachments { get; set; } = new List<RFQCreateDocument>();
    }

    public class RFQItemData
    {
        public int line_id { get; set; }
        public string item_code { get; set; }
        public string item_name { get; set; }
        public string item_uom_name { get; set; }
        public string item_descption { get; set; }
        public int quantity { get; set; }
        public decimal unit_price { get; set; }
        public decimal vat_rate { get; set; }
        public decimal vat_amount { get; set; }
        public decimal total_amount { get; set; }
    }


    public class RFQCreateQuestionnaire
    {
        public int questionnaire_number { get; set; }
        public string questionnaire_detail { get; set; }
    }

    public class RFQCreateDocument
    {
        public string file_name { get; set; }
        public string file_path { get; set; }
        public int file_seq { get; set; }
    }
}