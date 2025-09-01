using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.Marshalling;
using System.Security;

namespace VendorPortal.Domain.Models.WolfApprove.StoreModel
{
    public class SP_GET_PURCHASE_ORDER
    {
        public string nPOID { get; set; }
        public string sPOCode { get; set; }
        public string sQuotationCode { get; set; }
        public string nRFQID { get; set; }
        public int nCompanyID { get; set; }
        public string sCompanyName { get; set; }
        public string sProjectDesc { get; set; }
        public string sProjectName { get; set; }
        public DateTime dOrderDate { get; set; }
        public int nStatusID { get; set; }
        public string sStatusName { get; set; }
        public decimal nContractValue { get; set; }
        public int nCategoryID { get; set; }
        public string sCategoryName { get; set; }
        public int nProcurementTypeID { get; set; }
        public string sProcurementTypeName { get; set; }
        public string sCancelDesc { get; set; }
        public string sCancelReason { get; set; }
        public decimal dNetAmount { get; set; }
        public string sPaymentCondition { get; set; }
        public string sRemark { get; set; }
        public string sShipTo { get; set; }
        // RFQ
        public string sRFQNumber { get; set; }
        public string sRFQStatus { get; set; }
        public DateTime dRequireDate { get; set; }
        public int nRFQStatusID { get; set; }
        public string sRFQStatusName { get; set; }
        public string sContractFirstName { get; set; }
        public string sContractLastName { get; set; }
        public string sContractPhone { get; set; }
        public string sContractEmail { get; set; }

    }
}