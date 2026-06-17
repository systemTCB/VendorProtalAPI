using System;
using System.Collections.Generic;
using VendorPortal.Application.Models.Common;

namespace VendorPortal.Application.Models.v1.Response
{
    public class POCancelResponse : BaseResponse
    {
        public POCancelData data { get; set; }
    }

    public class POCancelData
    {
        public string id { get; set; }
        public string company_id { get; set; }
        public string company_contact_id { get; set; }
        public string document_type { get; set; }
        public string purchase_order_number { get; set; }
        public string quotation_header_id { get; set; }
        public int supplier_id { get; set; }
        public int created_by { get; set; }
        public int updated_by { get; set; }
        public string order_date { get; set; }
        public string require_date { get; set; }
        public string purchase_type_name { get; set; }
        public string procurement_type_name { get; set; }
        public string status { get; set; }
        public int is_mockup { get; set; }
        public string ship_to { get; set; }
        public string payment_condition { get; set; }
        public string remark { get; set; }
        public string cancel_reason { get; set; }
        public string cancel_description { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string discount { get; set; }
        public string sub_total { get; set; }
        public string total_amount { get; set; }
        public string vat_rate { get; set; }
        public string vat_amount { get; set; }
        public string net_amount { get; set; }
        public string currency { get; set; }
        public int include_withholding_tax { get; set; }
        public string wht_rate { get; set; }
        public string wht_amount { get; set; }
        public string sys_wht_amount { get; set; }
        public string final_amount { get; set; }
    }
}