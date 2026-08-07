using System;
using System.Collections.Generic;
using VendorPortal.Application.Models.Common;

namespace VendorPortal.Application.Models.v1.Response
{
    public class POCreateV2Response : BaseResponse
    {
        public POCreateV2Data data { get; set; }
    }

    public class POCreateV2Data
    {
        public string purchase_order_number { get; set; }
        public string quotation_header_id { get; set; }
        public int supplier_id { get; set; }
        public string document_type { get; set; }
        public string order_date { get; set; }
        public string require_date { get; set; }
        public string procurement_type_name { get; set; }
        public string status { get; set; }
        public object ship_to { get; set; }
        public string payment_condition { get; set; }
        public string remark { get; set; }
        public string discount { get; set; }
        public string sub_total { get; set; }
        public string total_amount { get; set; }
        public bool include_vat { get; set; }
        public string vat_rate { get; set; }
        public string vat_amount { get; set; }
        public string net_amount { get; set; }
        public string wht_rate { get; set; }
        public double wht_amount { get; set; }
        public double sys_wht_amount { get; set; }
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
        public double sys_vat_amount { get; set; }
        public POCrete_QuotationV2 quotation { get; set; }
        public List<LinePOV2> lines { get; set; }
    }

    public class LinePOV2
    {
        public string id { get; set; }
        public string purchase_order_id { get; set; }
        public object delivery_order_id { get; set; }
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

    public class POCrete_QuotationV2
    {
        public string id { get; set; }
        public string document_type { get; set; }
        public string quotation_number { get; set; }
        public string rfq_header_id { get; set; }
        public int supplier_id { get; set; }
        public object supplier_address_id { get; set; }
        public string status { get; set; }
        public int is_supplier_signature_attached { get; set; }
        public int is_e_signature_attached { get; set; }
        public int is_mockup { get; set; }
        public string notification_status { get; set; }
        public object notification_dispatched_at { get; set; }
        public object notification_processed_at { get; set; }
        public object notification_error { get; set; }
        public object api_response { get; set; }
        public object api_status_code { get; set; }
        public object api_response_at { get; set; }
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
        public int include_vat { get; set; }
        public string vat_amount { get; set; }
        public string sys_vat_amount { get; set; }
        public string remark { get; set; }
        public object note { get; set; }
        public string payment_condition { get; set; }
        public string company_id { get; set; }
        public string company_contact_id { get; set; }
    }
}