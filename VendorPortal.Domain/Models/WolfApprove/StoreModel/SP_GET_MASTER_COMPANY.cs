using System;

namespace VendorPortal.Domain.Models.WolfApprove.StoreModel
{
    public class SP_GET_MASTER_COMPANY
    {
        // Add properties and methods as needed
        public int nCompanyID { get; set; }
        public string sCompanyName { get; set; }
        public string sAddress1 { get; set; }
        public string sAddress2 { get; set; }
        public string sDistrict { get; set; }
        public string sSubDistrict { get; set; }
        public string sProvince { get; set; }
        public string sZipCode { get; set; }
        public string sBranch { get; set; }
        public string sTaxNumber { get; set; }
        public string sContractFirstName { get; set; }
        public string sContractLastName { get; set; }
        public string sContractPhone { get; set; }
        public string sContractEmail { get; set; }
        public bool isActive { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<DateTime> ModifiedDate { get; set; }
    }
}