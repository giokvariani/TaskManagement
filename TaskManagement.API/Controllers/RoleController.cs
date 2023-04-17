using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.API.Attributes;
using TaskManagement.Core.Application.Dtos;
using TaskManagement.Core.Application.Features.Commands.Role;
using TaskManagement.Core.Application.Features.Queries.Role;

namespace TaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AdminPrivilege]
    public class RoleController : ControllerBase
    {
        private readonly IMediator _mediator;
        public RoleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var role = await _mediator.Send(new GetRoleQuery(id));
            return Ok(role);
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var roles = await _mediator.Send(new GetRolesQuery());
            return Ok(roles);
        }

        [HttpPost]
        public async Task<ActionResult> Create(RoleDto createRoleDto)
        {
            var result = await _mediator.Send(new CreateRoleCommand(createRoleDto));
            return Ok(result);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteRoleCommand(id));
            return Ok(result);
        }

        [HttpPut]
        public async Task<ActionResult> Update(IdempotentRoleDto idempotentRoleDto)
        {
            var result = await _mediator.Send(new UpdateRoleCommand(idempotentRoleDto));
            return Ok(result);
        }
    }
}
