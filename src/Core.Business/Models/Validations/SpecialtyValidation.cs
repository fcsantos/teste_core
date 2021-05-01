using FluentValidation;

namespace Core.Business.Models.Validations
{
    public class SpecialtyValidation : AbstractValidator<Specialty>
    {
        public SpecialtyValidation()
        { 
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido")
                .Length(2, 100).WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");
        }
    }
}
