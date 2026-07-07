using System;
using System.Collections.Generic;
using VendorPortal.Application.Models.Common;
using VendorPortal.Application.Models.v1.Request;

namespace VendorPortal.Application.Models.v1.Response
{
    public class DeliveryOrdersUpdateResponse : BaseResponse
    {
        public DeliveryOrdersUpdateData data { get; set; }
    }
    public class DeliveryOrdersUpdateData
    {
        public Invoice invoice { get; set; }
        public DeliveryOrder deliveryOrder { get; set; }
        public PurchaseOrder purchase_order { get; set; }
    }

    public class DeliveryOrder
    {
        public string id { get; set; }
        public string delivery_order_number { get; set; }
    }

    public class Invoice
    {
        public string id { get; set; }
        public string invoice_number { get; set; }
        public string status { get; set; }
        public string cancel_reason { get; set; }
    }
}