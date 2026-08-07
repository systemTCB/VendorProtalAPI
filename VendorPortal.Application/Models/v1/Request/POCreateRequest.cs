using System;
using System.Collections.Generic;

namespace VendorPortal.Application.Models.v1.Request
{

    public class POCreateRequest
    {
        public string email { get; set; }
        public string company_tax_id { get; set; }
        public CompanyPO company { get; set; }
        public RfqDataPO rfq_data { get; set; }
        public List<RfqLinePO> rfq_lines { get; set; }
        public DocumentDataPO document_data { get; set; }
        public List<DocumentLinePO> document_lines { get; set; }
        public List<RFQCreateDocument> attachments { get; set; } = new List<RFQCreateDocument>();
    }

    public class CompanyPO
    {
        public string name { get; set; }
        public string company_id { get; set; }
        public string branch { get; set; }
        public string address_1 { get; set; }
        public string address_2 { get; set; }
        public string sub_district { get; set; }
        public string district { get; set; }
        public string province { get; set; }
        public string zipcode { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public ContactPO contact { get; set; }
    }

    public class ContactPO
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
    }

    public class DocumentDataPO
    {
        public int net_amount { get; set; }
        public int discount { get; set; }
        public string payment_condition { get; set; }
        public string remark { get; set; }
        public string issue_date { get; set; }
        public string purchase_order_number { get; set; }
        public string order_date { get; set; }
    }

    public class DocumentLinePO
    {
        public string item_code { get; set; }
        public string item_name { get; set; }
        public string uom_name { get; set; }
        public string description { get; set; }
        public int quantity { get; set; }
        public int unit_price { get; set; }
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
        public int unit_price { get; set; }
        public int vat_rate { get; set; }
    }

   
}