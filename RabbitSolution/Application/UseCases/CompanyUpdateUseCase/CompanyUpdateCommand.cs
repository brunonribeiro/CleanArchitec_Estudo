using Application.Core;
using Application.DTO;
using MediatR;

namespace Application.UseCases.CompanyUpdateUseCase
{
    public class CompanyUpdateCommand : CompanyDto, IRequest<Response>
    {
        public int Id { get; set; }
    }
}
