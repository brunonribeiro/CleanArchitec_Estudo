using Application.UseCases.CompanySaveUseCase;
using Domain.Entities;

namespace Application.Interfaces.Services
{
    public interface IRabbitService
    {
        void Post(CompanySaveCommand company);
        void Read();
    }
}
