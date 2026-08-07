using System;
using System.Collections.Generic;
using VendorPortal.Application.Models.Common;
using VendorPortal.Application.Models.v1.Request;

namespace VendorPortal.Application.Models.v1.Response
{
    public class DebitNoteUpdateResponse : BaseResponse
    {
        public DebitNoteData data { get; set; }
    }

    public class DebitNoteData
    {
        public DebitNote debit_note { get; set; }
    }


    public class DebitNote
    {
        public string id { get; set; }
        public string debit_note_number { get; set; }
        public string status { get; set; }
        public object reject_reason { get; set; }
    }


}