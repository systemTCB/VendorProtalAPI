using System;

namespace VendorPortal.Domain.Models.WolfApprove.StoreModel
{
    public class SP_CREATE_RFQ
    {
        public bool Result { get; set; }
        public string Message { get; set; }
        public Guid? RFQID { get; set; }
    }
}