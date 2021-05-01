using System;
using FluentValidation;

namespace Core.Business.Models.Validations
{
    public class DoctorValidation : AbstractValidator<Doctor>
    {
        public DoctorValidation()
        {
            RuleFor(d => d.Name)
                .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido")
                .Length(2, 100)
                .WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(d => d.Email)
                .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido")
                .EmailAddress().WithMessage("Um {PropertyName} válido é necessário.");

            RuleFor(d => d.Cell)
                .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido");

            RuleFor(d => d.DocumentCard)
                .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido")
                .MaximumLength(9).WithMessage("O campo {PropertyName} precisa ter {MaxLength} caracteres");
        }
    }
}
