using Application.Core;
using Application.Core.Validators;
using FluentValidation;
using System;

namespace Application.UseCases.CompanyUpdateUseCase
{
    public class CompanyUpdateValidator : AbstractValidator<CompanyUpdateCommand>
    {
        public CompanyUpdateValidator()
        {
            RuleFor(a => a.Cnpj)
              .NotEmpty()
              .IsValidCNPJ();

            RuleFor(a => a.Name)
               .NotEmpty()
               .MaximumLength(Constantes.QuantidadeDeCaracteres100);

            RuleFor(a => a.Email)
                .EmailAddress();

            RuleFor(a => a.FoundationDate)
                .Must(DateValidator.Valid).WithMessage(Constantes.MsgDataInvalida);
        }
    }
}
