using System;
using System.Collections.Generic;
using VendorPortal.Application.Models.Common;
using VendorPortal.Application.Models.ExtenalModel;

namespace VendorPortal.Application.Models.v1.Response
{
    public class POUpdateResponse : BaseResponse
    {
        public POUpdateData data { get; set; }
    }

    public class POUpdateData
    {
        public string id { get; set; }
        public string document_type { get; set; }
        public string purchase_order_number { get; set; }
        public string quotation_number { get; set; }
        public string rfq_number { get; set; }
        public string quotation_header_id { get; set; }
        public int supplier_id { get; set; }
        public string company_id { get; set; }
        public string company_contact_id { get; set; }
        public string currency { get; set; }
        public DateTime order_date { get; set; }
        public DateTime require_date { get; set; }
        public DateTime estimated_delivery_date { get; set; }
        public object purchase_type_name { get; set; }
        public string procurement_type_name { get; set; }
        public string status { get; set; }
        public string delivery_status { get; set; }
        public object ship_to { get; set; }
        public string payment_condition { get; set; }
        public object remark { get; set; }
        public object cancel_reason { get; set; }
        public object cancel_description { get; set; }
        public string net_amount { get; set; }
        public string discount { get; set; }
        public string sub_total { get; set; }
        public bool include_vat { get; set; }
        public string vat_rate { get; set; }
        public string vat_amount { get; set; }
        public string sys_vat_amount { get; set; }
        public string total_amount { get; set; }
        public string line_amount_without_vat { get; set; }
        public string line_amount_with_vat { get; set; }
        public bool include_withholding_tax { get; set; }
        public string wht_rate { get; set; }
        public string wht_amount { get; set; }
        public string sys_wht_amount { get; set; }
        public string final_amount { get; set; }
        public SupplierUpdatePO supplier { get; set; }
        public CompanyUpdatePO company { get; set; }
        public CreatedBy created_by { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public List<LinePOUpdate> lines { get; set; }
        public List<DocumentUpdatePO> documents { get; set; }
        public List<object> delivery_orders { get; set; }
    }

    public class LinePOUpdate
    {
        public string id { get; set; }
        public int line_number { get; set; }
        public string item_code { get; set; }
        public string item_name { get; set; }
        public string uom_name { get; set; }
        public string description { get; set; }
        public string quantity { get; set; }
        public string unit_price { get; set; }
        public string discount { get; set; }
        public string sub_total { get; set; }
        public string vat_rate { get; set; }
        public string vat_amount { get; set; }
        public string total_amount { get; set; }
        public string wht_rate { get; set; }
        public string wht_amount { get; set; }
        public string quantity_remain { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }

    public class SupplierUpdatePO
    {
        public int id { get; set; }
        public string name { get; set; }
        public string tax_id { get; set; }
        public string branch { get; set; }
    }

    public class CompanyUpdatePO
    {
        public string id { get; set; }
        public string name { get; set; }
        public string company_id { get; set; }
    }

    public class DocumentUpdatePO
    {
        public string uuid { get; set; }
        public string collection_name { get; set; }
        public string file_name { get; set; }
        public string file_url { get; set; }
    }
}