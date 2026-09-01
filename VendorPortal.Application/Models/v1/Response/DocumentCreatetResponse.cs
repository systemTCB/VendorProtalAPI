using System;
using System.Collections.Generic;
using VendorPortal.Application.Models.Common;
using VendorPortal.Application.Models.v1.Request;

namespace VendorPortal.Application.Models.v1.Response
{
    public class DocumentCreatetResponse : BaseResponse
    {
        public DocumentCreatetData data { get; set; }
    }
    public class DocumentCreatetData
    {
        public string id { get; set; }
        public string docNo { get; set; }
        public int memoId { get; set; }
        public int supplier_id { get; set; }
        public string company_id { get; set; }
        public string company_name { get; set; }
        public string document_name { get; set; }
        public string status { get; set; }
        public string reason { get; set; }
        public string remark { get; set; }
        public int user_id { get; set; }
        public bool is_require_signature { get; set; }
        public bool signed_completed { get; set; }
        public DateTime? created_at { get; set; }
        public DateTime? updated_at { get; set; }
        public List<DocumentCreate> documents { get; set; }
        public List<DocumentSignature> signatures { get; set; }
        public string WolfVendorCode { get; set; }
    }

    public class DocumentCreate
    {
        public string uuid { get; set; }
        public string file_name { get; set; }
        public string file_url { get; set; }
    }

    public class DocumentSignature
    {
        public int? user_id { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string email { get; set; }
        public string status { get; set; }
        public DateTime? signed_at { get; set; }
        public string signature_url { get; set; }
        public string position { get; set; }
    }

    public class DocumentUpdateResponse : BaseResponse
    {
        public DocumentUpdateData data { get; set; }
    }

    public class DocumentUpdateData
    {
        public string id { get; set; }
        public string status { get; set; }
        public object reason { get; set; }
    }
}