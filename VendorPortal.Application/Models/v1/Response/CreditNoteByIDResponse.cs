using System;
using System.Collections.Generic;
using VendorPortal.Application.Models.Common;
using VendorPortal.Application.Models.ExtenalModel;
using VendorPortal.Application.Models.v1.Request;

namespace VendorPortal.Application.Models.v1.Response
{
    public class CreditNoteByIDResponse : BaseResponse
    {
        public CreditNoteByData data { get; set; }
    }
    public class CreditNoteByData
    {
        public string id { get; set; }
        public string document_type { get; set; }
        public string invoice_number { get; set; }
        public string credit_note_number { get; set; }
        public int supplier_id { get; set; }
        public string company_id { get; set; }
        public string company_contact_id { get; set; }
        public DateTime credit_note_date { get; set; }
        public string credit_note_cause { get; set; }
        public string currency { get; set; }
        public string remark { get; set; }
        public string reject_reason { get; set; }
        public string discount { get; set; }
        public string sub_total { get; set; }
        public bool include_vat { get; set; }
        public string vat_rate { get; set; }
        public string vat_amount { get; set; }
        public string sys_vat_amount { get; set; }
        public string total_amount { get; set; }
        public string net_amount { get; set; }
        public string line_amount_without_vat { get; set; }
        public string line_amount_with_vat { get; set; }
        public bool include_withholding_tax { get; set; }
        public string wht_rate { get; set; }
        public string wht_amount { get; set; }
        public string sys_wht_amount { get; set; }
        public string final_amount { get; set; }
        public string difference_value { get; set; }
        public string status { get; set; }
        public CreatedBy created_by { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        public Supplier supplier { get; set; }
        public InvoiceCreditNote invoice { get; set; }
        public List<LineCreditNote> lines { get; set; }
        public List<DocumentCreditNote> documents { get; set; }

    }

    public class DocumentCreditNote
    {
        public string uuid { get; set; }
        public string file_name { get; set; }
        public string file_url { get; set; }
    }

    public class InvoiceCreditNote
    {
        public string id { get; set; }
        public string invoice_number { get; set; }
        public string status { get; set; }
    }

    public class LineCreditNote
    {
        public string id { get; set; }
        public string line_number { get; set; }
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
        public string invoice_line_id { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }


}