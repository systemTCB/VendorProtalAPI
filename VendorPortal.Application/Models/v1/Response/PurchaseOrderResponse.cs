using System;
using System.Collections.Generic;
using VendorPortal.Application.Models.Common;
using static VendorPortal.Application.Models.Common.KubbossCommonModel;

namespace VendorPortal.Application.Models.v1.Response
{
    public class PurchaseOrderListResponse : ResponseBasePage
    {
        public List<PurchaseOrderData> Data { get; set; }
    }
    public class PurchaseOrderData
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public QuotationData Quotation { get; set; }

        public string Company_Name { get; set; }
        public string Description { get; set; }
        public string Purchase_type_name { get; set; }
        public string Category_name { get; set; }
        public string Order_date { get; set; }
        public string Require_date { get; set; }
        public string Status { get; set; }
        public CompanyContract Company_contract { get; set; }
        public string Net_Amount { get; set; }
        public string Ship_to { get; set; }
        public string Payment_condition { get; set; }
        public string Remark { get; set; }
        public string Cancel_reason { get; set; }
        public string Cancel_description { get; set; }
    }
}