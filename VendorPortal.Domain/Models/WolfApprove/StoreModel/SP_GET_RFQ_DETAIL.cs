using System;
using System.Collections.Generic;
using System.Drawing;
using System.Security;

namespace VendorPortal.Domain.Models.WolfApprove.StoreModel
{
    public class SP_GET_RFQ_DETAIL
    {
        // Add properties and methods here
        public Guid nRFQID { get; set; }
        public string sRFQNumber { get; set; }
        public int nCompanyID { get; set; }
        public string sCompanyName { get; set; }
        public string sProjectName { get; set; }
        public string sProjectDesc { get; set; }
        public int nProcurementTypeID { get; set; }
        public string sProcurementTypeName { get; set; }
        public int nCategoryID { get; set; }
        public string sCategoryName { get; set; }
        public DateTime dStartDate { get; set; }
        public DateTime dEndDate { get; set; }
        public DateTime dRequireDate { get; set; }
        public int nStatusID { get; set; }
        public string sStatusName { get; set; }
        public decimal nContractValue { get; set; }
        public decimal dDiscount { get; set; }
        public decimal dNetAmount { get; set; }
        public string sAddress1 { get; set; }
        public string sAddress2 { get; set; }
        public string sSubDistrict { get; set; }
        public string sDistrict { get; set; }
        public string sProvince { get; set; }
        public string sZipCode { get; set; }
        public string sBranch { get; set; }
        public string sTaxNumber { get; set; }
        public string sRequesterName { get; set; }
        public string sRequesterLastName { get; set; }
        public string sRequesterEmail { get; set; }
        public string sRequesterTel { get; set; }
        public int nLineID { get; set; }
        public string sItemCode { get; set; }
        public string sItemName { get; set; }
        public string sItemUomName { get; set; }
        public string sItemDescption { get; set; }
        public int nQuantity { get; set; }
        public decimal dUnitPrice { get; set; }
        public decimal dVatRate { get; set; }
        public decimal dVatAmount { get; set; }
        public decimal dTotalAmount { get; set; }
        public decimal dSubTotal { get; set; }
        public string sPaymentCondition { get; set; }
        public string sRemark { get; set; }
    }
}
