using Application.Core;
using FluentValidation;
using System;

namespace Application.UseCases.CompanyUpdateUseCase
{
    public class CompanyUpdateValidator : AbstractValidator<CompanyUpdateCommand>
    {
        public CompanyUpdateValidator()
        {
            RuleFor(a => a.Id)
              .NotEmpty()
              .GreaterThan(0);

            RuleFor(a => a.Name)
               .MaximumLength(Constantes.QuantidadeDeCaracteres100);

            RuleFor(a => a.Email)
                .EmailAddress();

            RuleFor(a => a.FoundationDate)
                .Must(DateValid).WithMessage(Constantes.MsgDataInvalida);
        }

        private static bool DateValid(string foundationDate)
        {
            return string.IsNullOrEmpty(foundationDate) || (foundationDate.ToDate() < DateTime.Today && foundationDate.ToDate() > DateTime.MinValue);
        }
    }
}
