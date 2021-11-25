using Application.Core;
using Application.DTO;
using MediatR;

namespace Application.UseCases.CompanySaveUseCase
{
    public class CompanySaveCommand : CompanyDto, IRequest<Response>
    {
    }
}