using System;

namespace VendorPortal.Domain.Models.WolfApprove.StoreModel
{
    public class SP_GET_RFQ_LIST
    {
        // Add properties and methods here
        public Guid nRFQID { get; set; }
        public string sRFQNumber { get; set; }
        public short nCategoryID { get; set; }
        public string sCategoryName { get; set; }
        public string sProjectName { get; set; }
        public string sProjectDesc { get; set; }
        public short nProcurementTypeID { get; set; }
        public string sProcurementTypeName { get; set; }
        public DateTime dStartDate { get; set; }
        public DateTime dEndDate { get; set; }
        public DateTime dRequireDate { get; set; }
        public short nStatusID { get; set; }
        public string sStatusName { get; set; }
        public decimal nContractValue { get; set; }
        public decimal dNetAmount { get; set; }
        public string sPaymentCondition { get; set; }
        public string sRemark { get; set; }
        public string sAddress1 { get; set; }
        public string sAddress2 { get; set; }
        public string sBranch { get; set; }
        public string sDistrict { get; set; }
        public string sProvince { get; set; }
        public string sSubDistrict { get; set; }
        public string sTaxNumber { get; set; }
        public string sContractEmail { get; set; }
        public string sContractFirstName { get; set; }
        public string sContractLastName { get; set; }
        public string sContractPhone { get; set; }
        public string sCompanyName { get; set; }
        public int nCompanyID { get; set; }
        public string sQuotationCode { get; set; }
        public bool? bIsSpecific { get; set; }
    }
}