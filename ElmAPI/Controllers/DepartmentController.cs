using Elm.Application.Contracts.Features.Department.Commands;
using Elm.Application.Contracts.Features.Department.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Elm.API.Controllers
{
    [Authorize("Admin")]
    [EnableRateLimiting("UserRolePolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ApiBaseController
    {
        private readonly IMediator mediator;

        public DepartmentController(IMediator _mediator)
        {
            mediator = _mediator;
        }

        // this endpoint is public because it is used in the login process to load user departments
        [AllowAnonymous]
        [DisableRateLimiting]
        [HttpGet("GetAllDepartments/{yearId:int}")]
        public async Task<IActionResult> GetAllDepartments(int yearId)
          => HandleResult(await mediator.Send(new GetAllDepartmentQuery(yearId)));

        [AllowAnonymous]
        [DisableRateLimiting]
        [HttpGet("GetDepartmentById/{departmentId:int}")]
        public async Task<IActionResult> GetDepartmentById(int departmentId)
          => HandleResult(await mediator.Send(new GetDepartmentByIdQuery(departmentId)));


        [HttpPost("CreateDepartment")]
        public async Task<IActionResult> CreateDepartment([FromBody] AddDepartmentCommand command)
          => HandleResult(await mediator.Send(command));


        [HttpPut("UpdateDepartment")]
        public async Task<IActionResult> UpdateDepartment([FromBody] UpdateDepartmentCommand command)
          => HandleResult(await mediator.Send(command));


        [HttpDelete("DeleteDepartment/{departmentId:int}")]
        public async Task<IActionResult> DeleteDepartment(int departmentId)
          => HandleResult(await mediator.Send(new DeleteDepartmentCommand(departmentId)));

    }
}
