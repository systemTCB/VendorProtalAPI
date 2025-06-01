using System;
using System.Collections.Generic;
using VendorPortal.Application.Models.Common;

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
        public int vat_rate { get; set; }
        public string vat_amount { get; set; }
        public ExSyncSupplier supplier { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public List<ExSyncQuotationLine> lines { get; set; }
        public List<ExSyncQuotationDocument> documents { get; set; }
        public List<ExSyncQuotationQuestion> questions { get; set; }
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
        public int answer { get; set; }
        public string description { get; set; }
        public DateTime created_at { get; set; }
    }
}