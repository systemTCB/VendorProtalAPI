using System;

namespace VendorPortal.Domain.Models.WolfApprove.StoreModel
{
    public class SP_GET_CLAIM_LIST
    {
        // Class properties and methods go here
        public string nClaimID { get; set; }
        public string sClaimCode { get; set; }
        public string nCreatedDate { get; set; }
        public int nCompanyID { get; set; }
        public string sCompanyName { get; set; }
        public string sPOCode { get; set; }
        public DateTime dPurchaseDate { get; set; }
        public int nStatusID { get; set; }
        public string sStatusName { get; set; }
        public string dClaimDate { get; set; }
        public string sClaimReason { get; set; }
        public string sClaimDescription { get; set; }
        public string sClaimOption { get; set; }
        public string sClaimReturnAddress { get; set; }
    }
}