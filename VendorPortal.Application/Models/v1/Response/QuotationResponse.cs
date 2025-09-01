using System;
using System.Collections.Generic;
using VendorPortal.Application.Models.Common;

namespace VendorPortal.Application.Models.v1.Response
{
    public class QuotationResponse : BaseResponse
    {
        public List<QuotationData> data { get; set; }
    }
    public class QuotationData
    {
        public string id { get; set; }
        public string quotation_number { get; set; }
        public string rfq_number { get; set; }
        public int supplier_id { get; set; }
        public string company_id { get; set; }
        public string status { get; set; }
        public DateTime transfer_date { get; set; }
        public decimal net_amount { get; set; }
        public decimal discount { get; set; }
        public decimal sub_total { get; set; }
        public decimal total_amount { get; set; }
        public int vat_rate { get; set; }
        public decimal vat_amount { get; set; }
        public SupplierData supplier { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public List<QuotationLineData> lines { get; set; }
        public List<QuotationDocumentData> documents { get; set; }
        public List<QuotationQuestionData> questions { get; set; }
        public QuotationAddressData address { get; set; }
        public string payment_condition { get; set; }
        public string remark { get; set; }
    }

    public class SupplierData
    {
        public int id { get; set; }
        public string name { get; set; }
        public string tax_id { get; set; }
    }
    public class QuotationLineData
    {
        public string id { get; set; }
        public string quantity { get; set; }
        public string unit_price { get; set; }
        public int rfq_line_number { get; set; }
        public string rfq_item_code { get; set; }
        public string rfq_item_name { get; set; }
        public string rfq_uom_name { get; set; }
        public string rfq_description { get; set; }
    }

    public class QuotationDocumentData
    {
        public string uuid { get; set; }
        public string file_name { get; set; }
        public string file_url { get; set; }
    }

    public class QuotationQuestionData
    {
        public string id { get; set; }
        public string question_id { get; set; }
        public string question_number { get; set; }
        public string question { get; set; }
        public string answer { get; set; }
        public string description { get; set; }
        public DateTime created_at { get; set; }
    }

    public class QuotationAddressData
    {
        public string name { get; set; }
        public string address_1 { get; set; }
        public string address_2 { get; set; }
        public string province_name { get; set; }
        public string district_name { get; set; }
        public string sub_district_name { get; set; }
        public string postal_code { get; set; }
    }
}