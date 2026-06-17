using System;
using System.Collections.Generic;
using VendorPortal.Application.Models.Common;
using static VendorPortal.Application.Models.Common.KubbossCommonModel;

namespace VendorPortal.Application.Models.v1.Response
{
    public class RFQCancelResponse : BaseResponse
    {
        public RFQCancelData data { get; set; } = new RFQCancelData();
    }

    public class RFQCancelData
    {
        public string id { get; set; }
        public string remark { get; set; }
        public string rfq_id { get; set; }
        public string status { get; set; }
        public string category { get; set; }
        public string discount { get; set; }
        public string end_date { get; set; }
        public int is_mockup { get; set; }
        public string company_id { get; set; }
        public DateTime created_at { get; set; }
        public string net_amount { get; set; }
        public string rfq_number { get; set; }
        public string start_date { get; set; }
        public DateTime updated_at { get; set; }
        public string company_name { get; set; }
        public string project_name { get; set; }
        public string require_date { get; set; }
        public string cancel_remark { get; set; }
        public string contact_email { get; set; }
        public string contact_phone { get; set; }
        public string company_branch { get; set; }
        public string company_tax_id { get; set; }
        public string contract_email { get; set; }
        public string contract_phone { get; set; }
        public string contract_value { get; set; }
        public string company_address1 { get; set; }
        public string company_address2 { get; set; }
        public string request_for_type { get; set; }
        public string contact_last_name { get; set; }
        public string payment_condition { get; set; }
        public string company_contact_id { get; set; }
        public string contact_first_name { get; set; }
        public string contract_last_name { get; set; }
        public string contract_first_name { get; set; }
        public string project_description { get; set; }
        public string procurement_type_name { get; set; }
    }

}




