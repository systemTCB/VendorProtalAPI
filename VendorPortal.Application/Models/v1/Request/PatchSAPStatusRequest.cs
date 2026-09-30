using System;
using VendorPortal.Application.Models.Common;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VendorPortal.Application.Models.v1.Request
{
    public class PatchSAPStatusRequest
    {
        public string id { get; set; } = string.Empty;
        public string is_send_sap { get; set; } = string.Empty;
    }

    public class PatchSAPStatusResponse : BaseResponse
    {
        public PatchSAPStatusData data { get; set; }
    }

    public class PatchSAPStatusData
    {
        public string id { get; set; }
        public string document_type { get; set; }
        public string type { get; set; }
        public string delivery_order_number { get; set; }
        public string purchase_order_number { get; set; }
        public string rfq_number { get; set; }
        public QuotationPatchSAP quotation { get; set; }
        public string currency { get; set; }
        public object invoice_id { get; set; }
        public string delivery_address { get; set; }
        public string origin_address { get; set; }
        public string delivery_person { get; set; }
        public DateTime delivery_date { get; set; }
        public string receive_by { get; set; }
        public DateTime receive_date { get; set; }
        public DateTime estimated_delivery_date { get; set; }
        public object cancel_reason { get; set; }
        public object remark { get; set; }
        public string supplier_id { get; set; }
        public string company_id { get; set; }
        public string company_contact_id { get; set; }
        public string net_amount { get; set; }
        public string discount { get; set; }
        public string sub_total { get; set; }
        public bool include_vat { get; set; }
        public string vat_rate { get; set; }
        public string vat_amount { get; set; }
        public string total_amount { get; set; }
        public bool include_withholding_tax { get; set; }
        public object wht_rate { get; set; }
        public string wht_amount { get; set; }
        public string final_amount { get; set; }
        public string status { get; set; }
        public string is_send_sap { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }

    public class QuotationPatchSAP
    {
        public string quotation_id { get; set; }
        public string quotation_number { get; set; }
    }

}