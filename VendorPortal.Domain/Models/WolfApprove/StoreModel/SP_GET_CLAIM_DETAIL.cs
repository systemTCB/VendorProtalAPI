using System;
using System.Collections.Generic;

namespace VendorPortal.Domain.Models.WolfApprove.StoreModel
{
    public class SP_GET_CLAIM_DETAIL
    {
        // Add properties and methods here
        public string Id { get; set; }
        public string Code { get; set; }
        public DateTime Created_date { get; set; }
        public string Company_name { get; set; }
        public PurchaseOrderData Purchase_order { get; set; }
        public List<StatusData> Status { get; set; }
        public DateTime Claim_date { get; set; }
        public List<Line> Lines { get; set; }
        public string Claim_reason { get; set; }
        public string Claim_description { get; set; }
        public string Claim_option { get; set; }
        public string Claim_return_address { get; set; }
        public List<Document> Documents { get; set; }

        public class PurchaseOrderData
        {
            public string Code { get; set; }
            public DateTime Purchase_date { get; set; }
        }

        public class StatusData
        {
            public string Name { get; set; }
        }

        public class Line
        {
            public string Id { get; set; }
            public string Line_number { get; set; }
            public string Item_code { get; set; }
            public string Item_name { get; set; }
            public string Uom_name { get; set; }
            public string Description { get; set; }
            public string Quantity { get; set; }
            public string Unit_price { get; set; }
            public string Vat_rate { get; set; }
            public string Vat_amount { get; set; }
            public string Total_amount { get; set; }
        }

        public class Document
        {
            public string Name { get; set; }
            public string File_url { get; set; }
        }
    }
}