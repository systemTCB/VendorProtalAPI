using System;
using System.Collections.Generic;
using VendorPortal.Application.Models.Common;
using VendorPortal.Application.Models.ExtenalModel;
using VendorPortal.Application.Models.v1.Request;
using static VendorPortal.Application.Models.Common.KubbossCommonModel;

namespace VendorPortal.Application.Models.v1.Response
{
    public class InvoicesByIDResponse : BaseResponse
    {
        public InvoicesByIDData data { get; set; }
    }
    public class InvoicesByIDData
    {
        public string id { get; set; }
        public string document_type { get; set; }
        public string invoice_number { get; set; }
        public string delivery_order_number { get; set; }
        public string purchase_order_number { get; set; }
        public string rfq_number { get; set; }
        public string delivery_order_id { get; set; }
        public string purchase_order_id { get; set; }
        public int supplier_id { get; set; }
        public string company_id { get; set; }
        public string company_contact_id { get; set; }
        public string currency { get; set; }
        public DateTime invoice_date { get; set; }
        public DateTime due_date { get; set; }
        public string payment_term { get; set; }
        public string cancel_reason { get; set; }
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
        public bool is_down_payment { get; set; }
        public string down_payment { get; set; }
        public string status { get; set; }
        public CreatedBy created_by { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public Supplier supplier { get; set; }
        public CompanyInvoices company { get; set; }
        public CompanyContact company_contact { get; set; }
        public DeliveryOrderInvoices delivery_order { get; set; }
        public PurchaseOrderInvoices purchase_order { get; set; }
        public List<LineInvoices> lines { get; set; }
        public List<DocumentInvoices> documents { get; set; }

    }

    public class CompanyInvoices
    {
        public string id { get; set; }
        public string name { get; set; }
        public string company_id { get; set; }
    }

    public class CompanyContact
    {
        public string id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
    }


    public class DeliveryOrderInvoices
    {
        public string id { get; set; }
        public string delivery_order_number { get; set; }
        public string status { get; set; }
        public string purchase_order_number { get; set; }
        public string cancel_reason { get; set; }
    }

    public class LineInvoices
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
        public string delivery_order_line_id { get; set; }
        public string purchase_order_line_id { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }
    public class DocumentInvoices
    {
        public string uuid { get; set; }
        public string file_name { get; set; }
        public string file_url { get; set; }
    }

    public class PurchaseOrderInvoices
    {
        public string id { get; set; }
        public string purchase_order_number { get; set; }
        public string status { get; set; }
    }
}