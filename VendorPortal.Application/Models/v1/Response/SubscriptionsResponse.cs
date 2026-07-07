using System;
using System.Security.Cryptography;
using VendorPortal.Application.Models.Common;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace VendorPortal.Application.Models.v1.Response
{
    public class SubscriptionsResponse : BaseResponse
    {
        public SubscriptionsData data { get; set; }
    }

    public class SubscriptionsData
    {
        public string id { get; set; }
        public int supplier_id { get; set; }
        public string package_id { get; set; }
        public int subscription_order_id { get; set; }
        public string status { get; set; }
        public DateTime? purchase_date { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? expire_date { get; set; }
        public DateTime? inactive_at { get; set; }
        public string status_remark { get; set; }
        public string remark { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
    }


    public class SubscriptionsIDResponse : BaseResponse
    {
        public SubscriptionsIDData data { get; set; }
    }

    public class SubscriptionsIDData
    {
        public bool is_registered { get; set; }
        public string subscription_status { get; set; }
        public int supplier_id { get; set; }
        public string subscription_id { get; set; }
    }
}