using System;
using System.Collections.Generic;
using System.Security.Cryptography.Xml;
using Microsoft.AspNetCore.Http.HttpResults;
using VendorPortal.Application.Models.Common;
using VendorPortal.Application.Models.ExtenalModel;

namespace VendorPortal.Application.Models.v1.Response
{
    public class SyncQuotationResponse : BaseResponse
    {
        public SyncQuotationData data { get; set; }
    }

    public class SyncQuotationData
    {
        public string id { get; set; }
        public string quotation_number { get; set; }
        public string rfq_number { get; set; }
        public int supplier_id { get; set; }
        public string company_id { get; set; }
        public string status { get; set; }
        public DateTime transfer_date { get; set; }
        public string net_amount { get; set; }
        public string discount { get; set; }
        public string sub_total { get; set; }
        public string total_amount { get; set; }
        public string vat_rate { get; set; }
        public string vat_amount { get; set; }
        public ExSyncSupplier supplier { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public List<ExSyncQuotationLine> lines { get; set; }
        public List<ExSyncQuotationDocument> documents { get; set; }
        public List<ExSyncQuotationQuestion> questions { get; set; }
        public ExSyncQuotationAddress address { get; set; }
        public string payment_condition { get; set; }
        public string remark { get; set; }
        public bool is_supplier_signature_attached { get; set; }
        public bool is_require_signature { get; set; }
        public bool signed_completed { get; set; }
        public CreatedBy created_by { get; set; }
        public List<Signature> signatures { get; set; }
        public string document_type { get; set; }
        public bool include_withholding_tax { get; set; }
        public string wht_rate { get; set; }
        public string wht_amount { get; set; }
        public string sys_wht_amount { get; set; }
        public string final_amount { get; set; }
    }
    public class ExSyncSupplier
    {
        public int id { get; set; }
        public string name { get; set; }
        public string tax_id { get; set; }
    }
    public class ExSyncQuotationLine
    {
        public string id { get; set; }
        public string quantity { get; set; }
        public string unit_price { get; set; }
        public int rfq_line_number { get; set; }
        public string rfq_item_code { get; set; }
        public string rfq_item_name { get; set; }
        public string rfq_uom_name { get; set; }
        public string rfq_description { get; set; }
        public string total_amount { get; set; }
        public string vat_rate { get; set; }
        public string vat_amount { get; set; }
        public string discount { get; set; }
        public string sub_total { get; set; }
        public string wht_rate { get; set; }
        public string wht_amount { get; set; }
    }

    public class ExSyncQuotationDocument
    {
        public string uuid { get; set; }
        public string file_name { get; set; }
        public string file_url { get; set; }
    }

    public class ExSyncQuotationQuestion
    {
        public string id { get; set; }
        public string question_id { get; set; }
        public string question_number { get; set; }
        public string question { get; set; }
        public string answer { get; set; }
        public string description { get; set; }
        public DateTime created_at { get; set; }
    }

    public class ExSyncQuotationAddress
    {
        public string name { get; set; }
        public string address_1 { get; set; }
        public string address_2 { get; set; }
        public string province_name { get; set; }
        public string district_name { get; set; }
        public string sub_district_name { get; set; }
        public string postal_code { get; set; }
        public string branch { get; set; }
    }
    public class Signature
    {
        public string name { get; set; }
        public string surname { get; set; }
        public string signature_url { get; set; }
        public DateTime? signed_at { get; set; }
    }

    public class CreatedBy
    {
        public string name { get; set; }
        public string surname { get; set; }
        public string email { get; set; }
    }

}