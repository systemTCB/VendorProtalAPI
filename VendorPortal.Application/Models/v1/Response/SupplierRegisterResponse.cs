using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VendorPortal.Application.Models.Common;
using VendorPortal.Application.Models.ExtenalModel;

namespace VendorPortal.Application.Models.v1.Response
{
    public class SupplierRegisterResponseData
    {
        public int id { get; set; }
        public string name { get; set; }
        public string tax_id { get; set; }
        public string type { get; set; }
        public object logo_url { get; set; }
        public object juristical_person_certificate { get; set; }
        public object vat_registration { get; set; }
        public object financial_document { get; set; }
        public bool is_juristical_person { get; set; }
        public bool is_verify_complete { get; set; }
        public List<object> other_documents { get; set; }
        public List<Quotation> quotations { get; set; }
        public KeyContact key_contact { get; set; }
        public List<User> users { get; set; }
        public List<Address> addresses { get; set; }
        public object business_type { get; set; }
        public string buyerCode { get; set; }

    }

    public class SupplierRegisterResponse : BaseResponse
    {
        public SupplierRegisterResponseData data { get; set; }
    }

    public class KeyContact
    {
        public string name { get; set; }
        public object surname { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public object position { get; set; }
    }

    public class Quotation
    {
        public string id { get; set; }
        public string quotation_number { get; set; }
        public string status { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }

    public class User
    {
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
    }

}