using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.API.Attributes;
using TaskManagement.Core.Application.Dtos;
using TaskManagement.Core.Application.Interfaces;
using TaskManagement.Core.Domain.Entities;

namespace TaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AdminPrivilege]
    public class RoleController : ControllerBase
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public RoleController(IRoleRepository roleRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult> Create(RoleDto roleDto)
        {
            var role = _mapper.Map<Role>(roleDto);
            var result = await _roleRepository.CreateAsync(role);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var role = await _roleRepository.GetAsync(id);

            return Ok(role);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _roleRepository.DeleteAsync(id);
            return Ok(result);
        }

        [HttpPut]
        public async Task<ActionResult> Update(IdempotentRoleDto idempotentRoleDto)
        {
            var role = _mapper.Map<Role>(idempotentRoleDto);
            var result = await _roleRepository.UpdateAsync(role);
            return Ok(result);
        }
    }
}
