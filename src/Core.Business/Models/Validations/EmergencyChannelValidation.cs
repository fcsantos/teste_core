using FluentValidation;

namespace Core.Business.Models.Validations
{
    public class EmergencyChannelValidation : AbstractValidator<EmergencyChannel>
    {
        public EmergencyChannelValidation()
        {
            RuleFor(h => h.Name)
               .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido")
               .Length(2, 100)
               .WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");            
            
            RuleFor(h => h.Description)
               .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido")
               .Length(2, 200)
               .WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(n => n.Cell)
                .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido");


        }
    }
}
