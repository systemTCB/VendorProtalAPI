using System.Security.Cryptography.X509Certificates;
using FluentValidation;
using FluentValidation.Validators;
using Microsoft.AspNetCore.Rewrite;
using VendorPortal.Application.Models.v1.Request;

namespace VendorPortal.Application.Models.v1.ValidationRequest
{
    public class RFQCreateValidation : AbstractValidator<RFQCreateRequest>
    {
        public RFQCreateValidation()
        {
            RuleFor(x => x).NotNull().WithMessage("Request cannot be null.");
            RuleFor(x => x).NotEmpty().WithMessage("Request cannot be empty.");
            RuleFor(x => x.rfq_number).Must(x => !string.IsNullOrEmpty(x)).WithMessage("กรุณาใส่เลขที่ใบเสนอราคา");
            RuleFor(x => x.company_id).GreaterThan(0).WithMessage("กรุณาใส่เลขที่ใบเสนอราคา");
            RuleFor(x => x.discount).GreaterThanOrEqualTo(0).WithMessage("กรุณาระบุส่วนลด");
            RuleFor(x => x.payment_condition).Must(x => !string.IsNullOrEmpty(x)).WithMessage("กรุณาใส่เงื่อนไขการชำระเงิน");
            RuleFor(x => x.project_name).Must(x => !string.IsNullOrEmpty(x)).WithMessage("กรุณาใส่ชื่อโครงการ");
            RuleFor(x => x.project_description).Must(x => !string.IsNullOrEmpty(x)).WithMessage("กรุณาใส่รายละเอียดโครงการ");
            RuleFor(x => x.procurement_type_id).GreaterThan(0).WithMessage("กรุณาใส่ประเภทการจัดซื้อจัดจ้าง");
            RuleFor(x => x.procurement_category_id).GreaterThan(0).WithMessage("กรุณาใส่หมวดหมู่การจัดซื้อจัดจ้าง");
            RuleFor(x => x.start_date).NotEmpty().WithMessage("กรุณาใส่วันที่เริ่มต้น");
            RuleFor(x => x.end_date).NotEmpty().WithMessage("กรุณาใส่วันที่สิ้นสุด");
            RuleFor(x => x.required_date).NotEmpty().WithMessage("กรุณาใส่วันที่ต้องการ");
            RuleFor(x => x.items).Must(x => x?.Count > 0).WithMessage("กรุณาใส่รายการสินค้า");
        }
    }
}