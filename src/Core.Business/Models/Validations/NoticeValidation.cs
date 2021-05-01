using System;
using FluentValidation;

namespace Core.Business.Models.Validations
{
    public class NoticeValidation : AbstractValidator<Notice>
    {
        public NoticeValidation()
        {
            RuleFor(n => n.Description)
                .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido")
                .Length(2, 200).WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(m => m.StartDate)
               .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido")
               .GreaterThan(DateTime.Now.AddDays(-1)).WithMessage("A data de início não pode ser inferior à data atual");

            RuleFor(m => m.EndDate)
                .GreaterThanOrEqualTo(m => m.StartDate).WithMessage("A data de fim deve ser posterior à data de início");
        }
    }
}