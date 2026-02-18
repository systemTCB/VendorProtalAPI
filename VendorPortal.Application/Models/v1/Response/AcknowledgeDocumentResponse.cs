using System;
using System.Collections.Generic;
using VendorPortal.Application.Models.Common;
using static VendorPortal.Application.Models.Common.KubbossCommonModel;

namespace VendorPortal.Application.Models.v1.Response
{
    public class AcknowledgeDocumentResponse : BaseResponse
    {
        public AcknowledgeDocumentData data { get; set; } = new AcknowledgeDocumentData();
    }

    public class AcknowledgeDocumentData
    {
        public string rfq_id { get; set; }
        public string documentNo { get; set; }
        public string name { get; set; }
        public string supplier_id { get; set; }
        public string supplier_email { get; set; }
        public DateTime acknowledgeDate { get; set; }
    }

}




