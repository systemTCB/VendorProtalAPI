using System;

namespace VendorPortal.Domain.Models.WolfApprove.StoreModel
{
    public class SP_GET_JobDocuments
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