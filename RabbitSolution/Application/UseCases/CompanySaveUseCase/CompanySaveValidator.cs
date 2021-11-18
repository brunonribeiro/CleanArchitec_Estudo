using Application.Core;
using Application.Core.Validators;
using FluentValidation;
using System;

namespace Application.UseCases.CompanySaveUseCase
{
    public class CompanySaveValidator : AbstractValidator<CompanySaveCommand>
    {
        public CompanySaveValidator()
        {
            RuleFor(a => a.Name)
               .NotEmpty()
               .MaximumLength(Constantes.QuantidadeDeCaracteres100);

            RuleFor(a => a.Email)
                .EmailAddress();

            RuleFor(a => a.Cnpj)
                .NotEmpty()
                .IsValidCNPJ();

            RuleFor(a => a.FoundationDate)
                .NotEmpty()
                .Must(DateValidator.Valid).WithMessage(Constantes.MsgDataInvalida);
        }
    }
}
