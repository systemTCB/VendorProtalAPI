using System;
using System.Collections.Generic;
using VendorPortal.Application.Models.Common;

namespace VendorPortal.Application.Models.ExtenalModel
{
    public class SyncVendorResponse
    {
        public BaseResponse status { get; set; }
        public DateTime last_updated { get; set; }
        public List<SyncVendorData> data { get; set; } = new List<SyncVendorData>();
    }

    public class SyncVendorData
    {
        public string id { get; set; }
        public string name { get; set; }
        public string tax_id { get; set; }
        public string @type { get; set; }
        public string logo_url { get; set; }
        public string juristical_person_certificate { get; set; }
        public string vat_registration { get; set; }
        public string financial_document { get; set; }
        public bool is_juristical_person { get; set; }
        public bool is_verify_complete { get; set; }
         public List<Other_documents> other_documents { get; set; } = new List<Other_documents>();
         public List<Quotations> quotations { get; set; } = new List<Quotations>();
         public KeyContact key_contact { get; set; }
         public List<Address> addresses { get; set; } = new List<Address>();
         public BusinessType business_type { get; set; } 
    }

    public class Other_documents
    {
        public string file_url { get; set; }
        public string file_name { get; set; }
    }
    public class Quotations
    {
        public string id { get; set; }
        public string quotations_number { get; set; }
        public string status { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
    }
    public class KeyContact
    {
        public string name { get; set; }
        public string surname { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string position { get; set; }
    }
    public class Address
    {
        public string name { get; set; }
        public string address_1 { get; set; }
        public string address_2 { get; set; }
        public string province_name { get; set; }
        public string district_name { get; set; }
        public string sub_district_name { get; set; }
        public string postal_code { get; set; }
    }
    public class BusinessType
    {
        public string registered_business_type { get; set; }
        public string business_objective { get; set; }
    }
}