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
            //RuleFor(x => x.sub_total).GreaterThan(0).WithMessage("กรุณาระบุยอดเงิน");
            RuleFor(x => x.discount).GreaterThanOrEqualTo(0).WithMessage("กรุณาระบุส่วนลด");
            //RuleFor(x => x.total_amount).GreaterThan(0).WithMessage("กรุณาระบุยอดเงินรวม");
            //RuleFor(x => x.net_amount).GreaterThan(0).WithMessage("กรุณาระบุยอดเงินสุทธิ");
            RuleFor(x => x.payment_condition).Must(x => !string.IsNullOrEmpty(x)).WithMessage("กรุณาใส่เงื่อนไขการชำระเงิน");
            RuleFor(x => x.project_name).Must(x => !string.IsNullOrEmpty(x)).WithMessage("กรุณาใส่ชื่อโครงการ");
            RuleFor(x => x.project_description).Must(x => !string.IsNullOrEmpty(x)).WithMessage("กรุณาใส่รายละเอียดโครงการ");
            RuleFor(x => x.procurement_type_id).GreaterThan(0).WithMessage("กรุณาใส่ประเภทการจัดซื้อจัดจ้าง");
            RuleFor(x => x.procurement_category_id).GreaterThan(0).WithMessage("กรุณาใส่หมวดหมู่การจัดซื้อจัดจ้าง");
            RuleFor(x => x.start_date).NotEmpty().WithMessage("กรุณาใส่วันที่เริ่มต้น");
            RuleFor(x => x.end_date).NotEmpty().WithMessage("กรุณาใส่วันที่สิ้นสุด");
            RuleFor(x => x.required_date).NotEmpty().WithMessage("กรุณาใส่วันที่ต้องการ");
            //RuleFor(x => x.contract_value).GreaterThan(0).WithMessage("กรุณาระบุมูลค่าสัญญา");
            RuleFor(x => x.items).Must(x => x?.Count > 0).WithMessage("กรุณาใส่รายการสินค้า");
            //When(x => x.items != null && x.items.Count > 0, () =>
            //{
                //RuleFor(x => x.items).Must(x => x.Count > 0).WithMessage("กรุณาใส่รายการสินค้า");
                //RuleForEach(x => x.items).SetValidator(new RFQItemCreateValidation());
            //});
            ///When(x => x.attachments != null && x.attachments.Count > 0, () =>
            //{
                //RuleFor(x => x.attachments).Must(x => x.Count > 0).WithMessage("กรุณาใส่ไฟล์แนบ");
                //RuleForEach(x => x.attachments).SetValidator(new RFQAttachmentCreateValidation());
            //});
        }
    }

    // internal class RFQItemCreateValidation : IPropertyValidator<RFQCreateRequest, RFQItemData>
    // {
    //     public string Name => throw new System.NotImplementedException();

    //     public string GetDefaultMessageTemplate(string errorCode)
    //     {
    //         return "{PropertyName} is not valid.";
    //     }

    //     public bool IsValid(ValidationContext<RFQCreateRequest> context, RFQItemData value)
    //     {
    //         if (value == null)
    //         {
    //             context.AddFailure("items", "กรุณาใส่รายการสินค้า");
    //             return false;
    //         }

    //         if (string.IsNullOrEmpty(value.item_name))
    //         {
    //             context.AddFailure("items", "กรุณาใส่ชื่อสินค้า");
    //             return false;
    //         }
    //         if(string.IsNullOrEmpty(value.item_uom_name))
    //         {
    //             context.AddFailure("items", "กรุณาใส่หน่วยนับสินค้า");
    //             return false;
    //         }

    //         if (value.unit_price <= 0)
    //         {
    //             context.AddFailure("items", "กรุณาระบุราคาต่อหน่วย");
    //             return false;
    //         }

    //         if (value.quantity <= 0)
    //         {
    //             context.AddFailure("items", "กรุณาระบุจำนวน");
    //             return false;
    //         }

    //         return true;
    //     }
    // }
}