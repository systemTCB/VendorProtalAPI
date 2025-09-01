using System;

namespace VendorPortal.Domain.Models.WolfApprove.StoreModel
{
    public class SP_GET_RFQ_DOCUMENT
    {
        // Add properties and methods here
        public string nRFQID { get; set; }
        public string sFileName { get; set; }
        public string sFilePath { get; set; }
        public int nFileSeq { get; set; }
        public bool isActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}