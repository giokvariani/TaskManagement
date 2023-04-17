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
        private readonly IIssueRepository _issueRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public IssueController(IIssueRepository issueRepository, IUserRepository userRepository, IMapper mapper)
        {
            _issueRepository = issueRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult> Create(IssueDto issueDto)
        {
            var userName = User.Claims.SingleOrDefault(x => x.Type == "UserName")?.Value;
            var password = User.Claims.SingleOrDefault(x => x.Type == "Password")?.Value;
            var user = (await _userRepository.GetAsync(x => x.UserName == userName)).Single();
            issueDto.ReporterId = user.Id;
            issueDto.Status = Core.Domain.Enums.IssueStatusType.Open;

            var issue = _mapper.Map<Issue>(issueDto);
            var result = await _issueRepository.CreateAsync(issue);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var Issue = await _issueRepository.GetAsync(id);

            return Ok(Issue);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _issueRepository.DeleteAsync(id);
            return Ok(result);
        }

        [HttpPut]
        public async Task<ActionResult> Update(IdempotentIssueDto idempotentIssueDto)
        {
            var Issue = _mapper.Map<Issue>(idempotentIssueDto);
            var result = await _issueRepository.UpdateAsync(Issue);
            return Ok(result);
        }
    }
}
