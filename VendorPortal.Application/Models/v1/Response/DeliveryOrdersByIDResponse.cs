using System;
using System.Collections.Generic;
using VendorPortal.Application.Models.Common;
using VendorPortal.Application.Models.v1.Request;

namespace VendorPortal.Application.Models.v1.Response
{
    public class DeliveryOrdersResponse : BaseResponse
    {
        public DeliveryOrdersData data { get; set; }
    }
    public class DeliveryOrdersData
    {
        public string id { get; set; }
        public string document_type { get; set; }
        public string type { get; set; }
        public string delivery_order_number { get; set; }
        public string purchase_order_number { get; set; }
        public string rfq_number { get; set; }
        public QuotationDO quotation { get; set; }
        public string currency { get; set; }
        public string invoice_id { get; set; }
        public string delivery_address { get; set; }
        public string origin_address { get; set; }
        public string delivery_person { get; set; }
        public DateTime delivery_date { get; set; }
        public string receive_by { get; set; }
        public DateTime? receive_date { get; set; }
        public DateTime? estimated_delivery_date { get; set; }
        public string cancel_reason { get; set; }
        public string supplier_id { get; set; }
        public string company_id { get; set; }
        public string company_contact_id { get; set; }
        public string net_amount { get; set; }
        public string discount { get; set; }
        public string sub_total { get; set; }
        public string vat_rate { get; set; }
        public string vat_amount { get; set; }
        public string total_amount { get; set; }
        public string line_amount_without_vat { get; set; }
        public string line_amount_with_vat { get; set; }
        public bool include_withholding_tax { get; set; }
        public string wht_rate { get; set; }
        public string wht_amount { get; set; }
        public string sys_wht_amount { get; set; }
        public string final_amount { get; set; }
        public string status { get; set; }
        public CreatedBy created_by { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public List<LineDO> lines { get; set; }
        public List<DocumentDO> documents { get; set; }

    }

    public class QuotationDO
    {
        public string quotation_id { get; set; }
        public string quotation_number { get; set; }
    }

    public class LineDO
    {
        public string id { get; set; }
        public int line_number { get; set; }
        public string item_code { get; set; }
        public string item_name { get; set; }
        public string uom_name { get; set; }
        public object description { get; set; }
        public string quantity { get; set; }
        public string unit_price { get; set; }
        public string discount { get; set; }
        public string sub_total { get; set; }
        public string vat_rate { get; set; }
        public string vat_amount { get; set; }
        public string total_amount { get; set; }
        public object wht_rate { get; set; }
        public string wht_amount { get; set; }
    }

    public class DocumentDO
    {
        public string uuid { get; set; }
        public string file_name { get; set; }
        public string file_url { get; set; }
    }

}