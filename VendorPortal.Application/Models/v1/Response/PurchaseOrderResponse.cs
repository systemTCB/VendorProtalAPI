using System;
using System.Collections.Generic;
using VendorPortal.Application.Models.Common;

namespace VendorPortal.Application.Models.v1.Response
{
    public class PurchaseOrderResponse : BaseResponse
    {
        public List<PurchaseOrderData> data { get; set; }
    }
    public class PurchaseOrderData
    {
        public int OrderNo { get; set; }
        public string Vender { get; set; }
        public DateTime OrderSendDate { get; set; }
        public List<PurchaseItem> OrderItem { get; set; }

    }
    public class PurchaseItem
    {
        public int orderItem { get; set; }
        public string itemName { get; set; }
        public int itemValue { get; set; }

        public string itemUnit { get; set; }
    }
}