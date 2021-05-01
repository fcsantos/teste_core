using FluentValidation;

namespace Core.Business.Models.Validations
{
    public class PatientAnswersValidation : AbstractValidator<PatientAnswers>
    {
        public PatientAnswersValidation()
        {
            //RuleFor(x => x.QuestionTitle)
            //    .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido")
            //    .Length(2, 100).WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");
            
            //RuleFor(x => x.AnswerText)
            //    .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido")
            //    .Length(2, 100).WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

        }
    }
}
