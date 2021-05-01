﻿using FluentValidation;

namespace Core.Business.Models.Validations
{
    public class MessageValidation : AbstractValidator<Message>
    {
        public MessageValidation()
        {
            RuleFor(c => c.Text)
                .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido")
                .Length(2, 5000).WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");
        }
    }
}
