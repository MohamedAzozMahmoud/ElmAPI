using Elm.Application.Contracts.Features.Department.Commands;
using Elm.Application.Contracts.Features.Department.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Elm.API.Controllers
{
    [Authorize("Admin")]
    [EnableRateLimiting("UserRolePolicy")]
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
        [ProducesResponseType(typeof(DepartmentDto), 200)]
        public async Task<IActionResult> CreateDepartment([FromBody] AddDepartmentCommand command)
          => HandleResult(await mediator.Send(command));


        [HttpPut]
        [Route("UpdateDepartment")]
        [ProducesResponseType(typeof(DepartmentDto), 200)]
        public async Task<IActionResult> UpdateDepartment([FromBody] UpdateDepartmentCommand command)
          => HandleResult(await mediator.Send(command));


        [HttpDelete]
        [Route("DeleteDepartment/{departmentId:int}")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> DeleteDepartment([FromRoute] int departmentId)
          => HandleResult(await mediator.Send(new DeleteDepartmentCommand(departmentId)));

    }
}
