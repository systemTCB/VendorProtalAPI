using System;
using Microsoft.Extensions.Configuration;

namespace VendorPortal.Application.Models.Common
{
    public class KubbossCommonModel
    {
        // Add properties and methods here
        public class CompanyAddress
        {
            public string address_1 { get; set; }
            public string address_2 { get; set; }
            public string sub_district { get; set; }
            public string district { get; set; }
            public string province { get; set; }
            public string zip_code { get; set; }
            public string branch { get; set; }
            public string tax_number { get; set; }
        }
        public class CompanyContract
        {
            public string first_name { get; set; }
            public string last_name { get; set; }
            public string phone { get; set; }
            public string email { get; set; }
        }
        public class Line
        {
            public int id { get; set; }
            public string line_number { get; set; }
            public string item_code { get; set; }
            public string item_name { get; set; }
            public string uom_name { get; set; }
            public string description { get; set; }
            public int quantity { get; set; }
            public decimal unit_price { get; set; }
            public decimal vat_rate { get; set; }
            public decimal vat_amount { get; set; }
            public decimal total_amount { get; set; }
        }
        public class Document
        {
            public string name { get; set; }
            public string fileUrl { get; set; }
        }
        public class ClaimPurchaseOrderData
        {
            public string code { get; set; }
            public DateTime purchase_date { get; set; }
        }
        // public class QuotationData
        // {
        //     public string quotation_number { get; set; }
        // }

    }
}