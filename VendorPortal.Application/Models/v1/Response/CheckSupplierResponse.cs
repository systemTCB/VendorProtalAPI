using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using VendorPortal.Application.Models.Common;

namespace VendorPortal.Application.Models.v1.Response
{
    public class CheckSupplierResponse : BaseResponse
    {
        public CheckSupplierData? data { get; set; }
    }

    public class ActivePackage
    {
        public string id { get; set; }
        public string start_date { get; set; }
        public string expire_date { get; set; }
        public Package package { get; set; }
        public string status { get; set; }
        public string doc_no { get; set; }
        public string coupon_code { get; set; }
        public object last_terminated_at { get; set; }
        public object last_reactivated_at { get; set; }
        public int termination_count { get; set; }
    }

    public class AddressSupplier
    {
        public string name { get; set; }
        public string branch { get; set; }
        public string address_1 { get; set; }
        public object address_2 { get; set; }
        public object address_3 { get; set; }
        public string province_name { get; set; }
        public string district_name { get; set; }
        public string sub_district_name { get; set; }
        public string postal_code { get; set; }
        public string country_code { get; set; }
        public string country_name { get; set; }
        public object city { get; set; }
        public object state_region { get; set; }
    }

    public class CheckSupplierData
    {
        public int id { get; set; }
        public string name { get; set; }
        public string supplier_type { get; set; }
        public object supplier_code { get; set; }
        public string supplier_status { get; set; }
        public string tax_id { get; set; }
        public string branch { get; set; }
        public string supplier_email { get; set; }
        public string type { get; set; }
        public string type_code { get; set; }
        public string logo_url { get; set; }
        public object juristical_person_certificate { get; set; }
        public object vat_registration { get; set; }
        public object financial_document { get; set; }
        public bool is_juristical_person { get; set; }
        public bool is_verify_complete { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public DateTime? last_login_at { get; set; }
        public string login_status { get; set; }
        public KeyContactSupplier key_contact { get; set; }
        public List<PendingSubscription> pending_subscription { get; set; }
        public List<ActivePackage> active_packages { get; set; }
        public List<object> other_documents { get; set; }
        public List<QuotationSupplier> quotations { get; set; }
        public List<UserSupplier> users { get; set; }
        public List<AddressSupplier> addresses { get; set; }
        public object business_type { get; set; }
    }

    public class KeyContactSupplier
    {
        public string name { get; set; }
        public string surname { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string position { get; set; }
    }

    public class Package
    {
        public string id { get; set; }
        public string name { get; set; }
        public object name_en { get; set; }
        public string type { get; set; }
    }

    public class PendingSubscription
    {
        public string id { get; set; }
        public string start_date { get; set; }
        public string expire_date { get; set; }
        public Package package { get; set; }
        public string status { get; set; }
        public string doc_no { get; set; }
        public string coupon_code { get; set; }
        public object last_terminated_at { get; set; }
        public object last_reactivated_at { get; set; }
        public int termination_count { get; set; }
    }

    public class QuotationSupplier
    {
        public string id { get; set; }
        public string quotation_number { get; set; }
        public string status { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
    }

    public class UserSupplier
    {
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string position { get; set; }
        public string role { get; set; }
        public DateTime? last_login_at { get; set; }
    }
}
