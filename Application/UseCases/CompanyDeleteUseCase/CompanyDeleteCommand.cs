using Application.Core;
using MediatR;

namespace Application.UseCases.CompanyDeleteUseCase
{
    public class CompanyDeleteCommand : IRequest<Response>
    {
        public string Cnpj { get; set; }
    }
}
