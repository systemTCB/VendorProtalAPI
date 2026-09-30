using System;
using System.Collections.Generic;
using VendorPortal.Application.Models.Common;
using VendorPortal.Application.Models.ExtenalModel;

namespace VendorPortal.Application.Models.v1.Response
{
    public class BlockStatusByEmailResponse : BaseResponse
    {
        public BlockStatusByEmailData data { get; set; }
    }

    public class BlockStatusByEmailData
    {
        public string id { get; set; }
        public int? supplier_id { get; set; }
        public string package_id { get; set; }
        public int? subscription_order_id { get; set; }
        public string status { get; set; }
        public object purchase_date { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? expire_date { get; set; }
        public DateTime? inactive_at { get; set; }
        public string status_remark { get; set; }
        public DateTime? last_terminated_at { get; set; }
        public object last_reactivated_at { get; set; }
        public int? termination_count { get; set; }
        public object remark { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
    }

}