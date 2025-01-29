using System.Collections.Generic;
using VendorPortal.Application.Models.Common;
using static VendorPortal.Application.Models.Common.KubbossCommonModel;

namespace VendorPortal.Application.Models.v1.Response
{
    public class RFQResponse : BaseResponse
    {
        public RFQResponse()
        {
            Data = new List<RFQData>();
        }
        public int Total { get; set; }
        public int PerPage { get; set; }
        public int CurrentPage { get; set; }
        public int LastPage { get; set; }
        public string FirstPageUrl { get; set; }
        public string LastPageUrl { get; set; }
        public string NextPageUrl { get; set; }
        public string PrevPageUrl { get; set; }
        public string Path { get; set; }
        public int From { get; set; }
        public int To { get; set; }
        public List<RFQData> Data { get; set; }
    }
    public class RFQData
    {
        public RFQData()
        {
            CompanyContract = new CompanyContract();
        }
        public string Id { get; set; }
        public string Code { get; set; }
        public string CompanyName { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public string ProcurementTypeName { get; set; }
        public string CategoryName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string RequireDate { get; set; }
        public string Status { get; set; }
        public string ContractValue { get; set; }
        public CompanyContract CompanyContract { get; set; }
        public string NetAmount { get; set; }
        public string PaymentCondition { get; set; }
        public string Remark { get; set; }
    }

}




