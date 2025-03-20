using System;
using System.Collections.Generic;
using VendorPortal.Application.Models.Common;
using static VendorPortal.Application.Models.Common.KubbossCommonModel;

namespace VendorPortal.Application.Models.v1.Response
{
    public class PurchaseOrderResponse
    {
        public string id { get; set; }
        public string code { get; set; }
        public QuotationData quotation { get; set; }
        public int company_id { get; set; }
        public string company_name { get; set; }
        public string project_name { get; set; }
        public string description { get; set; }
        public string purchase_type_name { get; set; }
        public int catagory_id { get; set; }
        public string category_name { get; set; }
        public DateTime order_date { get; set; }
        public DateTime require_date { get; set; }
        public int status_id { get; set; }
        public string status { get; set; }
        public CompanyContract company_contract { get; set; }
        public decimal net_amount { get; set; }
        public string ship_to { get; set; }
        public string payment_condition { get; set; }
        public string remark { get; set; }
        public string cancel_reason { get; set; }
        public string cancel_description { get; set; }
        public int rfq_status_id { get; set; }
        public string rfq_status_name { get; set; }
    }
}