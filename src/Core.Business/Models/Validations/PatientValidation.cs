using System;
using FluentValidation;

namespace Core.Business.Models.Validations
{
    public class PatientValidation : AbstractValidator<Patient>
    {
        public PatientValidation()
        {
            RuleFor(p => p.FirstName)
                .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido")
                .Length(2, 50)
                .WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(p => p.LastName)
                .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido")
                .Length(2, 50)
                .WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(p => p.Document)
                .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido")
                .MaximumLength(9)
                .WithMessage("O campo {PropertyName} precisa ter {MaxLength} caracteres");

            RuleFor(p => p.DocumentCard)
                .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido")
                .MaximumLength(9)
                .WithMessage("O campo {PropertyName} precisa ter {MaxLength} caracteres");

            RuleFor(p => p.Email)
                .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido")
                .EmailAddress()
                .WithMessage("Um {PropertyName} válido é necessário.");

            RuleFor(p => p.Cell)
                .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido");

            RuleFor(p => p.BirthDate)
                .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido")
                .Must(BeAValidDate).WithMessage("O campo {PropertyName} deve ser menor que a data atual");
        }

        private bool BeAValidDate(DateTime date)
        {
            return !date.Equals(default(DateTime));
        }
    }
}
