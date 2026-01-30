using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Department.Commands;
using Elm.Application.Contracts.Features.Department.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Elm.API.Controllers
{
    //[Authorize("Admin")]
    //[EnableRateLimiting("UserRolePolicy")]
    [Route("api/admin/[controller]")]
    [ApiController]
    public class DepartmentAdminController : ApiBaseController
    {
        private readonly IMediator mediator;

        public DepartmentAdminController(IMediator _mediator)
        {
            mediator = _mediator;
        }
        [HttpPost]
        [Route("CreateDepartment")]
        [ProducesResponseType(typeof(Result<DepartmentDto>), 200)]
        public async Task<IActionResult> CreateDepartment([FromBody] AddDepartmentCommand command)
          => HandleResult(await mediator.Send(command));


        [HttpPut]
        [Route("UpdateDepartment")]
        [ProducesResponseType(typeof(Result<DepartmentDto>), 200)]
        public async Task<IActionResult> UpdateDepartment([FromBody] UpdateDepartmentCommand command)
          => HandleResult(await mediator.Send(command));


        [HttpDelete]
        [Route("DeleteDepartment/{departmentId:int}")]
        [ProducesResponseType(typeof(Result<bool>), 200)]
        public async Task<IActionResult> DeleteDepartment([FromRoute] int departmentId)
          => HandleResult(await mediator.Send(new DeleteDepartmentCommand(departmentId)));

    }
}
