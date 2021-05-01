using FluentValidation;

namespace Core.Business.Models.Validations
{
    public class ServiceValidation : AbstractValidator<Service>
    {
        public ServiceValidation()
        { 
            RuleFor(x => x.ServiceName)
                .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido")
                .Length(2, 100).WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");
        }
    }
}
