using System;
using System.Collections.Generic;
using VendorPortal.Application.Models.Common;
using VendorPortal.Application.Models.v1.Request;

namespace VendorPortal.Application.Models.v1.Response
{
    public class CreditNoteUpdateResponse : BaseResponse
    {
        public CreditNoteData data { get; set; }
    }

    public class CreditNoteData
    {
        public CreditNote credit_note { get; set; }
    }

    public class CreditNote
    {
        public string id { get; set; }
        public string credit_note_number { get; set; }
        public string status { get; set; }
        public object reject_reason { get; set; }
    }

}