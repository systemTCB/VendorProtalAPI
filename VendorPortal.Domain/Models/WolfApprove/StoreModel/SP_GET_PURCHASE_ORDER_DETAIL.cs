using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace VendorPortal.Domain.Models.WolfApprove.StoreModel
{
    public class SP_GET_PURCHASE_ORDER_DETAIL
    {

        public string nPOID { get; set; }
        public string sPOCode { get; set; }
        public string sQuotationCode { get; set; }
        public string nRFQID { get; set; }
        public string sRFQNumber { get; set; }
        public int nCompanyID { get; set; }
        public string sCompanyName { get; set; }
        public string sProjectName { get; set; }
        public string sProjectDesc { get; set; }
        public int nProcurementTypeID { get; set; }
        public string sProcurementTypeName { get; set; }
        public int nCategoryID { get; set; }
        public string sCategoryName { get; set; }
        public DateTime dOrderDate { get; set; }
        public DateTime dRequireDate { get; set; }
        public string sStatusName { get; set; }

        //Company
        public string sAddress1 { get; set; }
        public string sAddress2 { get; set; }
        public string sSubDistrict { get; set; }
        public string sDistrict { get; set; }
        public string sProvince { get; set; }
        public string sZipCode { get; set; }
        public string sBranch { get; set; }
        public string sTaxNumber { get; set; }
        public string sContractFirstName { get; set; }
        public string sContractLastName { get; set; }
        public string sContractPhone { get; set; }
        public string sContractEmail { get; set; }

        public decimal dSubtotal { get; set; }
        public decimal dDiscount { get; set; }
        //public string dTotalAmount { get; set; }
        //public string dVatAmount { get; set; }
        //public string dNetAmount { get; set; }
        // Line Item
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

        public string sShipTo { get; set; }
        public string sPaymentCondition { get; set; }
        public string sRemark { get; set; }
        public string sCancelReason { get; set; }
        public string sCancelDesc { get; set; }

    }
}