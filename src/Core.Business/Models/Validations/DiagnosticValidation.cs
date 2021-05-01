using FluentValidation;

namespace Core.Business.Models.Validations
{
    public class DiagnosticValidation : AbstractValidator<Diagnostic>
    {
        public DiagnosticValidation()
        {
            RuleFor(c => c.Description)
                .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido")
                .Length(2, 5000).WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");
        }
    }
}
