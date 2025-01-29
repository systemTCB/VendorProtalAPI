namespace VendorPortal.Domain.Models.WolfApprove.StoreModel
{
    public class SP_GET_RFQ
    {
        // Add properties and methods here
        public string Id { get; set; }
        public string Code { get; set; }
        public string Company_Name { get; set; }
        public string Project_Name { get; set; }
        public string Description { get; set; }
        public string Procurement_Type_Name { get; set; }
        public string Category_Name { get; set; }
        public string Start_Date { get; set; }
        public string End_Date { get; set; }
        public string Require_Date { get; set; }
        public string Status { get; set; }
        public string Contract_Value { get; set; }
        public CompanyContract Company_Contract { get; set; }
        public string Net_Amount { get; set; }
        public string Payment_Condition { get; set; }
        public string Remark { get; set; }
    }

    public class CompanyContract
    {
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}