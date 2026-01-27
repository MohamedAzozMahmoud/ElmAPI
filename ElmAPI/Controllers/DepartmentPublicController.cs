using Elm.Application.Contracts.Features.Department.DTOs;
using Elm.Application.Contracts.Features.Department.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Elm.API.Controllers
{
    [EnableRateLimiting("UserRolePolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentPublicController : ApiBaseController
    {
        private readonly IMediator mediator;

        public DepartmentPublicController(IMediator _mediator)
        {
            mediator = _mediator;
        }

        // this endpoint is public because it is used in the login process to load user departments
        [HttpGet]
        [Route("GetAllDepartments/{yearId:int}")]
        [ProducesResponseType(typeof(List<GetDepartmentDto>), 200)]
        public async Task<IActionResult> GetAllDepartments([FromRoute] int yearId)
          => HandleResult(await mediator.Send(new GetAllDepartmentQuery(yearId)));

        [HttpGet]
        [Route("GetDepartmentById/{departmentId:int}")]
        [ProducesResponseType(typeof(GetDepartmentDto), 200)]
        public async Task<IActionResult> GetDepartmentById([FromRoute] int departmentId)
          => HandleResult(await mediator.Send(new GetDepartmentByIdQuery(departmentId)));

    }
}
