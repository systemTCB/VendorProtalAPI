using System;

namespace VendorPortal.Domain.Models.WolfApprove.StoreModel
{
    public class SP_GET_PROCUREMENT_TYPE
    {
        public int nProcurementTypeID { get; set; }
        public string sProcurementTypeName { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifieyBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

    }
}