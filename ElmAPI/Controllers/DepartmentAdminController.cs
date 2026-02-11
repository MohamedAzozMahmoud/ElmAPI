using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Department.Commands;
using Elm.Application.Contracts.Features.Department.DTOs;
using Elm.Application.Contracts.Features.Department.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Elm.API.Controllers
{
    [Authorize(Roles = "Admin")]
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
        [ProducesResponseType(typeof(Result<bool>), 200)]
        public async Task<IActionResult> UpdateDepartment([FromBody] UpdateDepartmentCommand command)
          => HandleResult(await mediator.Send(command));


        [HttpDelete]
        [Route("DeleteDepartment/{departmentId:int}")]
        [ProducesResponseType(typeof(Result<bool>), 200)]
        public async Task<IActionResult> DeleteDepartment([FromRoute] int departmentId)
          => HandleResult(await mediator.Send(new DeleteDepartmentCommand(departmentId)));

        //GetAllDepartmentByCollegeIdQuery
        [HttpGet]
        [Route("GetAllDepartmentsByCollegeId/{collegeId:int}")]
        [ProducesResponseType(typeof(Result<List<DepartmentDto>>), 200)]
        public async Task<IActionResult> GetAllDepartmentsByCollegeId([FromRoute] int collegeId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
          => HandleResult(await mediator.Send(new GetAllDepartmentByCollegeIdQuery(collegeId, pageNumber, pageSize)));


        // PUT api/admin/DepartmentAdmin/TogglePublishDepartment/5
        [HttpPut]
        [Route("TogglePublishDepartment/{departmentId:int}")]
        [ProducesResponseType(typeof(Result<bool>), 200)]
        public async Task<IActionResult> TogglePublishDepartment([FromRoute] int departmentId)
          => HandleResult(await mediator.Send(new PublishedDepartmentCommand(departmentId)));


    }
}
