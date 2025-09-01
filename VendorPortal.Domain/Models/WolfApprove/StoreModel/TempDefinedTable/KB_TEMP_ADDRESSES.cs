namespace VendorPortal.Domain.Models.WolfApprove.StoreModel.TempDefinedTable
{
    public class KB_TEMP_ADDRESSES
    {
        public int vendor_id { get; set; }
        public string sAddress_1 { get; set; } = string.Empty;
        public string sAddress_2 { get; set; } = string.Empty;
        public string sProvince_name { get; set; } = string.Empty;
        public string sDistrict_name { get; set; } = string.Empty;
        public string sSub_district_name { get; set; } = string.Empty;
        public string sPostal_code { get; set; } = string.Empty;
    }
}