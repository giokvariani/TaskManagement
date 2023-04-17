using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.API.Attributes;
using TaskManagement.Core.Application.Dtos;
using TaskManagement.Core.Application.Features.Commands.User;
using TaskManagement.Core.Application.Features.Queries.User;
using TaskManagement.Core.Application.Interfaces;
using TaskManagement.Core.Domain.Entities;

namespace TaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AdminPrivilege]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        public UserController(IUserRepository userRepository, IMapper mapper, IMediator mediator)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var user = await _mediator.Send(new GetUserQuery(id));
            return Ok(user);
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

        [HttpPost("DefineRole")]
        public async Task<ActionResult> DefineRole(int userId, int roleId)
        {
            var result = await _userRepository.DefineRole(userId, roleId);
            return Ok(result);
        }

        [HttpDelete("DeleteRole")]
        public async Task<ActionResult> DeleteRule(int userId, int roleId)
        {
            var result = await _userRepository.DeleteRole(userId, roleId);
            return Ok(result);
        }
    }
}
