using System;
using System.Collections.Generic;
using Azure.Core;
using VendorPortal.Application.Models.v1.Response;
using static VendorPortal.Application.Models.v1.Request.CompanyPOV2;

namespace VendorPortal.Application.Models.v1.Request
{

    public class POCreateV2Request
    {
        public string email { get; set; }
        public string company_wolf_id { get; set; }
        public string buyerCode { get; set; }
        public CompanyPOV2 company { get; set; }
        public Requester requester { get; set; }
        public RfqData rfq_data { get; set; }
        public List<RfqLine> rfq_lines { get; set; }
        public DocumentDataPOV2 document_data { get; set; }
        public List<DocumentLinePOV2> document_lines { get; set; }
        public List<RFQCreateDocument> attachments { get; set; } = new List<RFQCreateDocument>();
    }
    public class Requester
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
    }

    public class RfqData
    {
        public string project_name { get; set; }
        public string project_description { get; set; }
        public string category { get; set; }
        public string procurement_type_name { get; set; }
        public string payment_condition { get; set; }
        public string remark { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
        public string require_date { get; set; }
    }

    public class RfqLine
    {
        public string item_code { get; set; }
        public string item_name { get; set; }
        public string uom_name { get; set; }
        public string description { get; set; }
        public int quantity { get; set; }
        public decimal unit_price { get; set; }
        public int vat_rate { get; set; }
    }



    public class CompanyPOV2
    {
        public string name { get; set; }
        public string tax_id { get; set; }
        public string branch { get; set; }
        public string address_1 { get; set; }
        public string address_2 { get; set; }
        public string sub_district { get; set; }
        public string district { get; set; }
        public string province { get; set; }
        public string zipcode { get; set; }
        public string email { get; set; }
        public string phone { get; set; }


        public class DocumentDataPOV2
        {
            public string document_type { get; set; }
            public int net_amount { get; set; }
            public int discount { get; set; }
            public bool include_vat { get; set; }
            public int vat_rate { get; set; }
            public int vat_amount { get; set; }
            public bool include_withholding_tax { get; set; }
            public int wht_rate { get; set; }
            public int wht_amount { get; set; }
            public bool auto_cal_vat { get; set; }
            public bool auto_cal_wht { get; set; }
            public string payment_condition { get; set; }
            public string remark { get; set; }
            public string issue_date { get; set; }
            public string purchase_order_number { get; set; }
            public string order_date { get; set; }
        }

        public class DocumentLinePOV2
        {
            public string item_code { get; set; }
            public string item_name { get; set; }
            public string uom_name { get; set; }
            public string description { get; set; }
            public int quantity { get; set; }
            public decimal unit_price { get; set; }
            public int discount { get; set; }
            public int vat_rate { get; set; }
            public int wht_rate { get; set; }
        }

        public class RfqDataPO
        {
            public string project_name { get; set; }
            public string project_description { get; set; }
            public string category { get; set; }
            public string procurement_type_name { get; set; }
            public string payment_condition { get; set; }
            public string remark { get; set; }
            public string start_date { get; set; }
            public string end_date { get; set; }
            public string require_date { get; set; }
        }

        public class RfqLinePO
        {
            public string item_code { get; set; }
            public string item_name { get; set; }
            public string uom_name { get; set; }
            public string description { get; set; }
            public int quantity { get; set; }
            public decimal unit_price { get; set; }
            public int vat_rate { get; set; }
        }

    }
}