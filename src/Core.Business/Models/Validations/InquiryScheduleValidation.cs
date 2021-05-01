using FluentValidation;
using System;

namespace Core.Business.Models.Validations
{
    public class InquiryScheduleValidation : AbstractValidator<InquirySchedule>
    {
        public InquiryScheduleValidation() 
        {
            RuleFor(m => m.StartDate)
               .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido")
               .GreaterThan(DateTime.Now.AddDays(-1)).WithMessage("A data de início não pode ser inferior à data atual");

            RuleFor(m => m.EndDate)
                .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido")
                .GreaterThanOrEqualTo(m => m.StartDate).WithMessage("A data de fim deve ser posterior à data de início");
        }
    }
}