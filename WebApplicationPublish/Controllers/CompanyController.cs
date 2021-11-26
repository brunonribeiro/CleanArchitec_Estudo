using Application.Interfaces.Repositories;
using Application.UseCases.CompanyDeleteUseCase;
using Application.UseCases.CompanySaveUseCase;
using Application.UseCases.CompanyUpdateUseCase;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICompanyRepositoryMongoDb _companyRepositoryMongoDb;

        public CompanyController(IMediator mediator, ICompanyRepositoryMongoDb companyRepositoryMongoDb)
        {
            _mediator = mediator;
            _companyRepositoryMongoDb = companyRepositoryMongoDb;
        }

        [HttpPost]
        [Route("Save")]
        public async Task<IActionResult> Post([FromBody] CompanySaveCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(response);
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] CompanyUpdateCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(response);
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete([FromBody] CompanyDeleteCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(response);
        }

        [HttpGet]
        [Route("List")]
        public IActionResult Get()
        {
            var response = _companyRepositoryMongoDb.QueryAll().ToList();

            return Ok(response);
        }

        [HttpGet]
        [Route("GetById")]
        public IActionResult GetById(int id)
        {
            var response = _companyRepositoryMongoDb.Query(id);

            return Ok(response);
        }
    }
}