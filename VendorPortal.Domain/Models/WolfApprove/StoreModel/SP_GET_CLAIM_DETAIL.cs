using System;
using System.Collections.Generic;

namespace VendorPortal.Domain.Models.WolfApprove.StoreModel
{
    public class SP_GET_CLAIM_DETAIL
    {
        // Add properties and methods here
        public string Id { get; set; }
        public string Code { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CompanyName { get; set; }
        public PurchaseOrderData PurchaseOrder { get; set; }
        public List<StatusData> Status { get; set; }
        public DateTime ClaimDate { get; set; }
        public List<Line> Lines { get; set; }
        public string ClaimReason { get; set; }
        public string ClaimDescription { get; set; }
        public string ClaimOption { get; set; }
        public string ClaimReturnAddress { get; set; }
        public List<Document> Documents { get; set; }

        public class PurchaseOrderData
        {
            public string Code { get; set; }
            public DateTime PurchaseDate { get; set; }
        }

        public class StatusData
        {
            public string Name { get; set; }
        }

        public class Line
        {
            public string Id { get; set; }
            public string LineNumber { get; set; }
            public string ItemCode { get; set; }
            public string ItemName { get; set; }
            public string UomName { get; set; }
            public string Description { get; set; }
            public string Quantity { get; set; }
            public string UnitPrice { get; set; }
            public string VatRate { get; set; }
            public string VatAmount { get; set; }
            public string TotalAmount { get; set; }
        }

        public class Document
        {
            public string Name { get; set; }
            public string FileUrl { get; set; }
        }
    }
}