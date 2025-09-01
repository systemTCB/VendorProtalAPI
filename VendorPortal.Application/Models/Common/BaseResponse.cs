namespace VendorPortal.Application.Models.Common
{
    /// <summary>
    /// ใช้สำหรับ Return ข้อมูลที่เป็น Object และไม่มี Paging
    /// </summary>
    public class BaseResponse
    {
        public Status status { get; set; }

    }
    public class Status
    {
        public string code { get; set; }
        public string message { get; set; }
    }
    /// <summary>
    /// ใช้สำหรับ Return ข้อมูลที่เป็น List พร้อม Paging
    /// </summary>
    /// <typeparam name="T">Any Class List</typeparam>
    public class BaseResponse<T> : BaseResponse
    {
        public T data { get; set; }
        public int total { get; set; }
        public int per_page { get; set; }
        public int current_page { get; set; }
        public int last_page { get; set; }
        public string first_page_url { get; set; }
        public string last_page_url { get; set; }
        public string next_page_url { get; set; }
        public string prev_page_url { get; set; }
        public string path { get; set; }
        public int from { get; set; }
        public int to { get; set; }

    }
}