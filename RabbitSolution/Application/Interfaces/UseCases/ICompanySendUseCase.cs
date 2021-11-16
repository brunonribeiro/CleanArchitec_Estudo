using Application.DTO;

namespace Application.Interfaces.UseCases
{
    public interface ICompanySendUseCase
    {
        void Execute(CompanyDto companyDto);
    }
}
