using Application.DTO;

namespace Application.Interfaces.Services
{
    public interface IRabbitService
    {
        void Post(CompanyDto company);
        void Read();
    }
}
