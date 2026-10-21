using System;

namespace VendorPortal.Domain.Models.WolfApprove.StoreModel
{
    public class SP_GET_TRNPurchaseOrderKubboss
    {
        public long id { get; set; }
        public string email { get; set; }
        public string purchase_order_id { get; set; }
        public string purchase_order_number { get; set; }
        public string remark { get; set; }
        public string status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}