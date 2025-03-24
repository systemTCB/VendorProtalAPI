using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.SqlServer.Server;
using VendorPortal.Application.Models.Common;
using static VendorPortal.Application.Models.Common.KubbossCommonModel;

namespace VendorPortal.Application.Models.v1.Response
{
    public class PurchaseOrderDetailResponse : BaseResponse
    {
        // Add properties and methods here
        public PurchaseOrderDetailData Data { get; set; }
    }

    public class PurchaseOrderDetailData
    {
        public string id { get; set; }
        public string number { get; set; }
        public int company_id { get; set; }
        public string company_name { get; set; }
        public string project_name { get; set; }
        public string quotation_number { get; set; }
        public string description { get; set; }
        public string category_name { get; set; }
        public DateTime order_date { get; set; }
        public DateTime request_date { get; set; }
        public string status { get; set; }
        public CompanyAddress company_address { get; set; }
        public CompanyContract company_contract { get; set; }
        public decimal sub_totoal { get; set; }
        public decimal discount { get; set; }
        public string total_amount { get; set; }
        public string vat_amount { get; set; }
        public string net_amount { get; set; }
        public List<Line> lines { get; set; }
        public List<Document> documents { get; set; }
        public string payment_condition { get; set; }
        public string remark { get; set; }
        public string cancel_reason { get; set; }
        public string cancel_description { get; set; }
        public string ship_to { get; set; }

    }
}