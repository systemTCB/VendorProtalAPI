using System;
using System.Collections.Generic;
using Azure.Core;
using VendorPortal.Application.Models.v1.Response;
using static VendorPortal.Application.Models.v1.Request.CompanyPOV2;

namespace VendorPortal.Application.Models.v1.Request
{

    public class POStandaloneRequest
    {
        public DocumentData document_data { get; set; }
        public List<DocumentLine> document_lines { get; set; }
        public string lang { get; set; }
        public List<RFQCreateDocument> attachments { get; set; } = new List<RFQCreateDocument>();

    }
    public class DocumentData
    {
        public string purchase_order_number { get; set; }
        public string document_type { get; set; }
        public string order_date { get; set; }
        public string require_date { get; set; }
        public string payment_condition { get; set; }
        public string remark { get; set; }
        public string currency { get; set; }
        public decimal? discount { get; set; }
        public bool include_vat { get; set; }
        public decimal? vat_rate { get; set; }
        public decimal? vat_amount { get; set; }
        public bool include_withholding_tax { get; set; }
        public decimal? wht_rate { get; set; }
        public decimal? wht_amount { get; set; }
        public bool auto_cal_vat { get; set; }
        public bool auto_cal_wht { get; set; }
    }

    public class DocumentLine
    {
        public string item_code { get; set; }
        public string item_name { get; set; }
        public string uom_name { get; set; }
        public string description { get; set; }
        public decimal? quantity { get; set; }
        public decimal? unit_price { get; set; }
        public decimal? discount { get; set; }
        public decimal? vat_rate { get; set; }
        public decimal? wht_rate { get; set; }
    }
}