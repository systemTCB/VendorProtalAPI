using System.Net.NetworkInformation;

namespace VendorPortal.Domain.Models.WolfApprove.StoreModel
{
    public class SP_GET_PURCHASE_ORDER
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public QuotationData Quotation { get; set; }

        public string Company_Name { get; set; }
        public string Description { get; set; }
        public string Purchase_type_name { get; set; }
        public string Category_name { get; set; }
        public string Order_date { get; set; }
        public string Require_date { get; set; }
        public string Status { get; set; }
        public CompanyContractData Company_contract { get; set; }
        public string Net_Amount { get; set; }
        public string Ship_to { get; set; }
        public string Payment_condition { get; set; }
        public string Remark { get; set; }
        public string Cancel_reason { get; set; }
        public string Cancel_description { get; set; }
    }
    public class QuotationData
    {
        public string Code { get; set; }
    }
    public class CompanyContractData
    {
        public string First_name { get; set; }
        public string Last_name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}