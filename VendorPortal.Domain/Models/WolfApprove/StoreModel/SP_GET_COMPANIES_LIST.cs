namespace VendorPortal.Domain.Models.WolfApprove.StoreModel
{
    public class SP_GET_COMPANIES_LIST
    {
        // Add properties and methods here
        public string Id { get; set; }
        public string Name { get; set; }
        public string Request_date { get; set; }
        public string Request_status { get; set; }
        public CompanyContract Company_contact { get; set; } 
    }
}