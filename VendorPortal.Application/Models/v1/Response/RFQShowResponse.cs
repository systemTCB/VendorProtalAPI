using System;
using System.Collections.Generic;
using VendorPortal.Application.Models.Common;
using static VendorPortal.Application.Models.Common.KubbossCommonModel;

namespace VendorPortal.Application.Models.v1.Response
{
    public class RFQShowResponse : BaseResponse
    {
        public RFQShowData data { get; set; }
    }

    public class RFQShowData
    {
        public string id { get; set; }
        public string number { get; set; }
        public int company_id { get; set; }
        public string company_name { get; set; }
        public string project_name { get; set; }
        public string description { get; set; }
        public string purchase_type_name { get; set; }
        public string category_name { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public DateTime require_date { get; set; }
        public string status { get; set; }
        public decimal contract_value { get; set; }
        public CompanyAddress company_address { get; set; }
        public CompanyContract company_contract { get; set; }
        public decimal sub_total { get; set; }
        public decimal discount { get; set; }
        public decimal total_amount { get; set; }
        public decimal vat_amount { get; set; }
        public decimal net_amount { get; set; }
        public List<Line> lines { get; set; }
        public string payment_condition { get; set; }
        public string remark { get; set; }
        public List<Document> documents { get; set; }
    }
}