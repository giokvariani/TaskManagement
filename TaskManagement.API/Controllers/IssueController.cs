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
    [Authorize]
    public class IssueController : ControllerBase
    {
        private readonly IIssueRepository _IssueRepository;
        private readonly IMapper _mapper;

        public IssueController(IIssueRepository IssueRepository, IMapper mapper)
        {
            _IssueRepository = IssueRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult> Create(IssueDto IssueDto)
        {
            var issue = _mapper.Map<Issue>(IssueDto);
            var result = await _IssueRepository.CreateAsync(issue);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var Issue = await _IssueRepository.GetAsync(id);

            return Ok(Issue);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _IssueRepository.DeleteAsync(id);
            return Ok(result);
        }

        [HttpPut]
        public async Task<ActionResult> Update(IdempotentIssueDto idempotentIssueDto)
        {
            var Issue = _mapper.Map<Issue>(idempotentIssueDto);
            var result = await _IssueRepository.UpdateAsync(Issue);
            return Ok(result);
        }
    }
}
