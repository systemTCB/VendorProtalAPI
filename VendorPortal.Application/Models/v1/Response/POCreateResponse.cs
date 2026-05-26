using System;
using System.Collections.Generic;
using VendorPortal.Application.Models.Common;

namespace VendorPortal.Application.Models.v1.Response
{
    public class POCreateResponse : BaseResponse
    {
        public POCreateData data { get; set; }
    }

    public class POCreateData
    {
        public string purchase_order_number { get; set; }
        public string quotation_header_id { get; set; }
        public int supplier_id { get; set; }
        public string document_type { get; set; }
        public DateTime order_date { get; set; }
        public DateTime require_date { get; set; }
        public string procurement_type_name { get; set; }
        public string status { get; set; }
        public string ship_to { get; set; }
        public string payment_condition { get; set; }
        public string remark { get; set; }
        public string discount { get; set; }
        public string sub_total { get; set; }
        public string total_amount { get; set; }
        public string vat_rate { get; set; }
        public string vat_amount { get; set; }
        public string net_amount { get; set; }
        public string wht_rate { get; set; }
        public string wht_amount { get; set; }
        public int sys_wht_amount { get; set; }
        public double final_amount { get; set; }
        public string currency { get; set; }
        public bool include_withholding_tax { get; set; }
        public int created_by { get; set; }
        public int updated_by { get; set; }
        public string company_id { get; set; }
        public string company_contact_id { get; set; }
        public bool is_mockup { get; set; }
        public string id { get; set; }
        public DateTime updated_at { get; set; }
        public DateTime created_at { get; set; }
        public POCrete_Quotation quotation { get; set; }
        public List<LinePO> lines { get; set; }
    }

    public class LinePO
    {
        public string id { get; set; }
        public string purchase_order_id { get; set; }
        public string delivery_order_id { get; set; }
        public int line_number { get; set; }
        public string item_code { get; set; }
        public string item_name { get; set; }
        public string uom_name { get; set; }
        public string description { get; set; }
        public string quantity { get; set; }
        public string unit_price { get; set; }
        public string sub_total { get; set; }
        public string vat_rate { get; set; }
        public string vat_amount { get; set; }
        public string wht_rate { get; set; }
        public string wht_amount { get; set; }
        public string total_amount { get; set; }
        public string discount { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string quantity_remain { get; set; }
    }

    public class POCrete_Quotation
    {
        public string id { get; set; }
        public string document_type { get; set; }
        public string quotation_number { get; set; }
        public string rfq_header_id { get; set; }
        public int supplier_id { get; set; }
        public string supplier_address_id { get; set; }
        public string status { get; set; }
        public int is_supplier_signature_attached { get; set; }
        public int is_e_signature_attached { get; set; }
        public int is_mockup { get; set; }
        public string notification_status { get; set; }
        public string notification_dispatched_at { get; set; }
        public string notification_processed_at { get; set; }
        public string notification_error { get; set; }
        public string api_response { get; set; }
        public string api_status_code { get; set; }
        public string api_response_at { get; set; }
        public int notification_retry_count { get; set; }
        public string net_amount { get; set; }
        public string currency { get; set; }
        public int include_withholding_tax { get; set; }
        public string wht_rate { get; set; }
        public string wht_amount { get; set; }
        public string sys_wht_amount { get; set; }
        public string final_amount { get; set; }
        public string discount { get; set; }
        public string transfer_date { get; set; }
        public string issue_date { get; set; }
        public int created_by { get; set; }
        public int updated_by { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public string sub_total { get; set; }
        public string total_amount { get; set; }
        public string vat_rate { get; set; }
        public string vat_amount { get; set; }
        public string remark { get; set; }
        public string note { get; set; }
        public string payment_condition { get; set; }
        public string company_id { get; set; }
        public string company_contact_id { get; set; }
    }
}