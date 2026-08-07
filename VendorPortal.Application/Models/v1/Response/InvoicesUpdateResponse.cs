using System;
using System.Collections.Generic;
using VendorPortal.Application.Models.Common;
using VendorPortal.Application.Models.v1.Request;

namespace VendorPortal.Application.Models.v1.Response
{
    public class InvoicesUpdateResponse : BaseResponse
    {
        public InvoicesUpdateData data { get; set; }
    }
    public class InvoicesUpdateData
    {
        public InvoiceUpdate invoice { get; set; }
        public DeliveryOrderUpdate delivery_order { get; set; }
    }

    public class DeliveryOrderUpdate
    {
        public string id { get; set; }
        public string delivery_order_number { get; set; }
    }

    public class InvoiceUpdate
    {
        public string id { get; set; }
        public string invoice_number { get; set; }
        public string status { get; set; }
        public string cancel_reason { get; set; }
    }
}