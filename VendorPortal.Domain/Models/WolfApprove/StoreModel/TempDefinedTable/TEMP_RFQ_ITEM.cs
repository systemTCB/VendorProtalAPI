namespace VendorPortal.Domain.Models.WolfApprove.StoreModel
{
    public class TEMP_RFQ_ITEM
    {
        public string sRFQID { get; set; }
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
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
    }
}