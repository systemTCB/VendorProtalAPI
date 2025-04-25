using System;

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
        public int? procurement_tyepe_id { get; set; }
        public int? procurement_category_id { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public DateTime required_date { get; set; }
        public decimal contract_value { get; set; }
        public string remark { get; set; }
        public string rfq_status { get; set; }
        public string created_by { get; set; }

    }
}