using System;
using System.Collections.Generic;
using VendorPortal.Application.Models.Common;
using static VendorPortal.Application.Models.Common.KubbossCommonModel;

namespace VendorPortal.Application.Models.v1.Response
{
    public class RFQDataItem
    {
        public RFQDataItem()
        {
            company_contract = new CompanyContract();
        }
        public string id { get; set; }
        public string number { get; set; }
        public string company_name { get; set; }
        public string project_name { get; set; }
        public string description { get; set; }
        public string procurement_type_name { get; set; }
        public string category_name { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public DateTime require_date { get; set; }
        public string status { get; set; }
        public decimal contract_value { get; set; }
        public CompanyContract company_contract { get; set; }
        public decimal net_amount { get; set; }
        public string payment_condition { get; set; }
        public string remark { get; set; }
    }

}




