using System;
using System.Collections.Generic;
using VendorPortal.Application.Models.Common;
using VendorPortal.Application.Models.ExtenalModel;

namespace VendorPortal.Application.Models.v1.Response
{
    public class SuppliersResponse : BaseResponse
    {
        public SuppliersData data { get; set; }
    }

    public class Address
    {
        public string name { get; set; }
        public string branch { get; set; }
        public string address_1 { get; set; }
        public string address_2 { get; set; }
        public string province_name { get; set; }
        public string district_name { get; set; }
        public string sub_district_name { get; set; }
        public string postal_code { get; set; }
    }

    public class SuppliersData
    {
        public int id { get; set; }
        public string name { get; set; }
        public string supplier_type { get; set; }
        public string supplier_code { get; set; }
        public string tax_id { get; set; }
        public string supplier_email { get; set; }
        public string type { get; set; }
        public string logo_url { get; set; }
        public string juristical_person_certificate { get; set; }
        public string vat_registration { get; set; }
        public string financial_document { get; set; }
        public bool is_juristical_person { get; set; }
        public bool is_verify_complete { get; set; }
        public DateTime last_login_at { get; set; }
        public string login_status { get; set; }
        public List<object> other_documents { get; set; }
        public List<Quotation> quotations { get; set; }
        public KeyContact key_contact { get; set; }
        public List<User> users { get; set; }
        public List<Address> addresses { get; set; }
        public string business_type { get; set; }
    }
   
}