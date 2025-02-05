using System;
using System.ComponentModel;
using System.Linq;

namespace VendorPortal.Application.Models.Common
{
    public static class AppEnum
    {
        public static string Description(this Enum value)
        {
            // get attributes  
            var field = value.GetType().GetField(value.ToString());
            var attributes = field.GetCustomAttributes(false);

            // Description is in a hidden Attribute class called DisplayAttribute
            // Not to be confused with DisplayNameAttribute
            dynamic displayAttribute = null;

            if (attributes.Any())
            {
                displayAttribute = attributes.ElementAt(0);
            }

            // return description
            return displayAttribute?.Description ?? "Description Not Found";
        }
        public static string Text(this Enum value)
        {
            return Convert.ToInt16(value).ToString();
        }

        public enum ResponseCode
        {
            [Description("ทำรายการสำเร็จ")] Success = 200,
            [Description("รูปแบบข้อมูลที่ส่งเข้าระบบไม่ถูกต้อง")] BadRequest = 400,
            [Description("ไม่อนุญาตให้เข้าถึงระบบ")] Unauthorized = 401,
            [Description("ไม่มีสิทธิ์ในการใช้งาน")] Forbidden = 403,
            [Description("ไม่พบข้อมูล")] NotFound = 404,
            [Description("ข้อมูลที่ส่งเข้ามาไม่ถูกต้อง")] Unprocessable = 422,
            [Description("มีข้อผิดพลาดบนระบบ")] InternalServerError = 500,
            [Description("ระบบไม่สามารถดำเนินการต่อได้")] NotImplement = 501,
        }
    }
}