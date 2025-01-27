using System;
using System.Collections.Generic;
using VendorPortal.Application.Models.Common;

namespace VendorPortal.Application.Models.v1.Response
{
    public class RFQShowResponse : BaseResponse
    {
        public RFQShowData Data { get; set; }
    }

    public class RFQShowData
    {
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
        public List<StatusName> Status { get; set; }
        public decimal ContractValue { get; set; }
        public CompanyAddress CompanyAddress { get; set; }
        public CompanyContract CompanyContract { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal VatAmount { get; set; }
        public decimal NetAmount { get; set; }
        public List<Line> Lines { get; set; }
        public string PaymentCondition { get; set; }
        public string Remark { get; set; }
        public List<Document> Documents { get; set; }
    }

    public class StatusName
    {
        public string Name { get; set; }
    }

    public class CompanyAddress
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

    public class Line
    {
        public string Id { get; set; }
        public string LineNumber { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string UomName { get; set; }
        public string Description { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal VatRate { get; set; }
        public decimal VatAmount { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class Document
    {
        public string Name { get; set; }
        public string FileUrl { get; set; }
    }
}