namespace VendorPortal.Domain.Models.WolfApprove.StoreModel
{
    public class SP_CREATE_RFQ
    {
        public bool result { get; set; }
        public string message { get; set; }
        public string rfq_id { get; set; } = string.Empty;
        public string rfq_number { get; set; } = string.Empty;
    }
}