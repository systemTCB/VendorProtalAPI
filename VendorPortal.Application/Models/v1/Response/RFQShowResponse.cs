using System;
using System.Collections.Generic;
using VendorPortal.Application.Models.Common;
using static VendorPortal.Application.Models.Common.KubbossCommonModel;

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
        public int Company_Id { get; set; }
        public string Company_Name { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public string PurchaseTypeName { get; set; }
        public string Category_Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime RequireDate { get; set; }
        public List<StatusName> Status { get; set; }
        public decimal ContractValue { get; set; }
        public CompanyAddress Company_Address { get; set; }
        public CompanyContract Company_Contract { get; set; }
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
}