using System;
using System.Security.Cryptography;
using VendorPortal.Application.Models.Common;

namespace VendorPortal.Application.Models.v1.Response
{
    public class JobDocumentResponse : BaseResponse
    {
        public JobDocumentData data { get; set; }
    }

    public class JobDocumentData
    {
        public int id { get; set; }
        public string jobDocumentID { get; set; }
        public string jobDocumentVendorCode { get; set; }
        public string jobDocumentVendorName { get; set; }
        public string jobDocumentEmail { get; set; }
        public string jobDocumentName { get; set; }
        public string jobDocumentType { get; set; }
        public string jobTypeList { get; set; }
        public string jobDocumentDescription { get; set; }
        public string jobDocumentProduct { get; set; }
        public DateTime? jobDocumentStartDate { get; set; }
        public DateTime? jobDocumentEndDate { get; set; }
        public string jobDocumentAmount { get; set; }
        public DateTime? jobDocumentCreateDate { get; set; }
    }
}