using FluentValidation;

namespace Core.Business.Models.Validations
{
    public class ActionValidation : AbstractValidator<AppAction>
    {
        public ActionValidation()
        { 
            RuleFor(x => x.ActionName)
                .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório.")
                .Length(3).WithMessage("O campo {PropertyName} precisa ter no mínimo {MinLength} caracteres.");
        }
    }
}
