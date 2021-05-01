using FluentValidation;

namespace Core.Business.Models.Validations
{
    public class ControllerValidation : AbstractValidator<AppController>
    {
        public ControllerValidation()
        {
            RuleFor(x => x.ControllerName)
                .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório.");

        }
    }
}
