using Application.UseCases.CompanySaveUseCase;
using Application.UseCases.CompanyUpdateUseCase;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CompanyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("Save")]
        public async Task<IActionResult> Post([FromBody] CompanySaveCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(response);
        }

        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] CompanyUpdateCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(response);
        }
    }
}