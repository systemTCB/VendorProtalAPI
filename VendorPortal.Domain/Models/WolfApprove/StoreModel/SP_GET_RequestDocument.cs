using System;

namespace VendorPortal.Domain.Models.WolfApprove.StoreModel
{
    public class SP_GET_RequestDocument
    {
        public string docNo { get; set; }
        public int memoId { get; set; }
        public string kubboss_document_id { get; set; }
        public string company_code { get; set; }
        public string email { get; set; }
    }
}