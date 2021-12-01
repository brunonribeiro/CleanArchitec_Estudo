using Application.DTO;

namespace Application.Interfaces.Services
{
    public interface IKafkaService
    {
        void Produce(CompanyDto company);

        void Consume();
    }
}
