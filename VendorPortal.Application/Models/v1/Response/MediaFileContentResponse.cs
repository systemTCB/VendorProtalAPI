using System;
using System.Security.Cryptography;
using VendorPortal.Application.Models.Common;

namespace VendorPortal.Application.Models.v1.Response
{
    public class MediaFileContentResponse : BaseResponse
    {
        public string file_name { get; set; }
        public int file_size { get; set; }
        public string mime_type { get; set; }
        public string extension { get; set; }
        public string content { get; set; }
    }

}