using System;

namespace VendorPortal.Domain.Models.WolfApprove.StoreModel
{
    public class SYSConfigEndPoint
    {
        public int ID { get; set; }
        public string sEndPoint { get; set; }
        public string jParameter { get; set; }
        public int nHoursRepeat { get; set; }
        public int nMinuteRepeat { get; set; }
        public bool IsActive { get; set; }
        public DateTime? dNextDateTime { get; set; }
        public int? nLastResponseHeaderCode { get; set; }
        public string sLastResponseBody { get; set; }
        public string sResponseMessage { get; set; }
    }
}