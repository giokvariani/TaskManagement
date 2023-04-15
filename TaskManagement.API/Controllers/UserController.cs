using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Core.Application.Dtos;
using TaskManagement.Core.Application.Interfaces;
using TaskManagement.Core.Domain.Entities;

namespace TaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult> Create(UserDto createUserDto)
        {
            var user = _mapper.Map<User>(createUserDto);
            var result = await _userRepository.CreateAsync(user);
            return Ok(user);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var user = await _userRepository.GetAsync(id);
            
            return Ok(user);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _userRepository.DeleteAsync(id);
            return Ok(result); 
        }
        [HttpPut]
        public async Task<ActionResult> Update(IdempotentUserDto idempotentUserDto)
        {
            var user = _mapper.Map<User>(idempotentUserDto);
            var result =  await _userRepository.UpdateAsync(user);
            return Ok(result);
        }
    }
}
