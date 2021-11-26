using FluentValidation;

namespace Application.UseCases.CompanyDeleteUseCase
{
    public class CompanyDeleteValidator : AbstractValidator<CompanyDeleteCommand>
    {
        public CompanyDeleteValidator()
        {
            RuleFor(a => a.Cnpj)
              .NotEmpty()
              .IsValidCNPJ();
        }
    }
}
