
using System;
using System.Security;

namespace VendorPortal.Domain.Models.WolfApprove.StoreModel
{
    public class SP_GET_MASTER_CATAGORY
    {
        public int nCatagoryID { get; set; }
        public string sCatagotyName { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}