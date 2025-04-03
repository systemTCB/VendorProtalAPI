using FluentValidation;
using VendorPortal.Application.Models.v1.Request;

namespace Namespace.Application.Models.v1.ValidationRequest
{
    public class CancelQuotationValidation : AbstractValidator<CancelQuotationRequest>
    {
        public CancelQuotationValidation()
        {
            RuleFor(x => x.quo_number).Must(x => !string.IsNullOrEmpty(x)).WithMessage("กรุณาใส่เลขที่ใบเสนอราคา");
            RuleFor(x => x.reason).Must(x => !string.IsNullOrEmpty(x)).WithMessage("กรุณาใส่เหตุผลในการยกเลิกใบเสนอราคา");
            RuleFor(x => x.status).Must(x => !string.IsNullOrEmpty(x)).WithMessage("กรุณาใส่สถานะใบเสนอราคา");
            RuleFor(x => x.quo_number).NotEmpty().WithMessage("quo_number is required.");
            RuleFor(x => x.reason).NotEmpty().WithMessage("reason is required.");
        }
    }
}