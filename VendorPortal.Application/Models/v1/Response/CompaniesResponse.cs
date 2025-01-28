using System.Collections.Generic;
using VendorPortal.Application.Models.Common;

namespace VendorPortal.Application.Models.v1.Response
{
    public class CompaniesResponse : BaseResponse
    {
        public int Total { get; set; }
        public int PerPage { get; set; }
        public int CurrentPage { get; set; }
        public int LastPage { get; set; }
        public string FirstPageUrl { get; set; }
        public string LastPageUrl { get; set; }
        public string NextPageUrl { get; set; }
        public string PrevPageUrl { get; set; }
        public string Path { get; set; }
        public int From { get; set; }
        public int To { get; set; }
        public List<CompainesData> Data { get; set; }
        // Add properties and methods here
    }
    public class CompainesData
    {

    }
}