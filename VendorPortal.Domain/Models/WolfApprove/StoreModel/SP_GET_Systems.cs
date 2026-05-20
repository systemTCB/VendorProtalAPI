using System;
using static System.Net.WebRequestMethods;

namespace VendorPortal.Domain.Models.WolfApprove.StoreModel
{
    public class SP_GET_Systems
    {
        public int SystemId { get; set; }
        public string SystemCode { get; set; }
        public string Host { get; set; }
        public string BaseUrl { get; set; }
        public bool IsActive { get; set; }
        public string Token { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}