using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.API.Attributes;
using TaskManagement.Core.Application.Dtos;
using TaskManagement.Core.Application.Features.Commands.User;
using TaskManagement.Core.Application.Features.Queries.User;

namespace TaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AdminPrivilege]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var user = await _mediator.Send(new GetUserQuery(id));
            return Ok(user);
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var users = await _mediator.Send(new GetUsersQuery());
            return Ok(users);
        }

        [HttpPost]
        public async Task<ActionResult> Create(UserDto createUserDto)
        {
            var result = await _mediator.Send(new CreateUserCommand(createUserDto));
            return Ok(result);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteUserCommand(id));
            return Ok(result);
        }

        [HttpPut]
        public async Task<ActionResult> Update(IdempotentUserDto idempotentUserDto)
        {
            var result = await _mediator.Send(new UpdateUserCommand(idempotentUserDto));
            return Ok(result);
        }

        [HttpPost("EnableRole")]
        public async Task<ActionResult> EnableRole(int userId, int roleId)
        {
            var result = await _mediator.Send(new EnableRoleCommand(userId, roleId));
            return Ok(result);
        }

        [HttpDelete("DisableRole")]
        public async Task<ActionResult> DisableRole(int userId, int roleId)
        {
            var result = await _mediator.Send(new DisableRoleCommand(userId, roleId));
            return Ok(result);
        }
    }
}
