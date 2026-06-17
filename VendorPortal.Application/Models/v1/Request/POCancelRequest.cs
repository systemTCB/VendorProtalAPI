using System;
using System.Collections.Generic;

namespace VendorPortal.Application.Models.v1.Request
{
    public class POCancelRequest
    {
        public string purchase_order_number { get; set; }
        public string cancel_reason { get; set; }

    }
}