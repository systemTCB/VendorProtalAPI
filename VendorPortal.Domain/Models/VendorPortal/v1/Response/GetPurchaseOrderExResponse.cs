using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using VendorPortal.Domain.Models.KubBoss;

namespace VendorPortal.Domain.Models.KubBoss.v1.Response
{
    public class GetPurchaseOrderExResponse : ExBaseResponseStatus
    {
        public List<GetPurchaseOrderExData> data { get; set; }
    }
    public class GetPurchaseOrderExData
    {
        public int orderNo { get; set; }
        public string vender { get; set; }
        public List<GetPurchaseOrderExItem> orderItem { get; set; }
        public DateTime orderSendDate { get; set; }
    }
    public class GetPurchaseOrderExItem
    {
        public int orderItem { get; set; }
        public string itemName { get; set; }
        public int itemValue { get; set; }
        public string itemUnit { get; set; }

    }

}