using System;
using System.Collections.Generic;

namespace VendorPortal.Application.Models.v1.Request
{
    public class VendorRegisterRequest
    {
        public string supplier_id { get; set; }
        public string buyerCode { get; set; }
        public string docNo { get; set; }
        //public string buyer_code { get; set; }
    }

    public class SupplierRegisterRequest
    {
        public string email { get; set; }
        public string docNo { get; set; }
        public string supplier_name { get; set; }
        public string key_contact_name { get; set; }
        public string coupon_code { get; set; }
        public string lang { get; set; }
    }
}