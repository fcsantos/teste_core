﻿using FluentValidation;

namespace Core.Business.Models.Validations
{
    public class InquiryValidation : AbstractValidator<Inquiry>
    {
        public InquiryValidation()
        {
            RuleFor(c => c.Title)
                .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido")
                .Length(2, 150).WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");
        }
    }
}