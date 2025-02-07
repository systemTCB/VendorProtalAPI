namespace VendorPortal.Domain.Models.WolfApprove.StoreModel
{
    public class SP_GET_CLAIM_LIST
    {
        // Class properties and methods go here
        public string Id { get; set; }
        public string Code { get; set; }
        public string Create_date { get; set; }
        public string Company_name { get; set; }
        public PurchaseOrderData Purchase_order { get; set; }
        public string Status { get; set; }
        public string Claim_date { get; set; }
        public string Claim_reason { get; set; }
        public string Claim_description { get; set; }
        public string Claim_option { get; set; }
        public string Claim_return_address { get; set; }
    }

    public class PurchaseOrderData
    {
        public string Code { get; set; }
        public string Purchase_date { get; set; }
    }
}