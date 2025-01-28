using System;
using System.Collections.Generic;

namespace VendorPortal.Domain.Models.WolfApprove.StoreModel
{
    public class SP_GET_RFQ_DETAIL
    {
        // Add properties and methods here
        public string Id { get; set; }
        public string Number { get; set; }
        public string CompanyName { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public string PurchaseTypeName { get; set; }
        public string CategoryName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime RequireDate { get; set; }
        public List<StatusData> Status { get; set; }
        public string ContractValue { get; set; }
        public CompanyAddressData CompanyAddress { get; set; }
        public CompanyContractData CompanyContract { get; set; }
        public string SubTotal { get; set; }
        public string Discount { get; set; }
        public string TotalAmount { get; set; }
        public string VatAmount { get; set; }
        public string NetAmount { get; set; }
        public List<Line> Lines { get; set; }
        public string PaymentCondition { get; set; }
        public string Remark { get; set; }
        public List<Document> Documents { get; set; }

        public class StatusData
        {
            public string Name { get; set; }
        }

        public class CompanyAddressData
        {
            public string Address1 { get; set; }
            public string Address2 { get; set; }
            public string SubDistrict { get; set; }
            public string District { get; set; }
            public string Province { get; set; }
            public string ZipCode { get; set; }
            public string Branch { get; set; }
            public string TaxNumber { get; set; }
        }

        public class CompanyContractData
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
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