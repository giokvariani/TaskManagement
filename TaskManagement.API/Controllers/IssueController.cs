using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Core.Application.Dtos;
using TaskManagement.Core.Application.Features.Commands.Issue;
using TaskManagement.Core.Application.Features.Queries.Issue;

namespace TaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class IssueController : ControllerBase
    {
        private readonly IMediator _mediator;
        public IssueController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            var issue = await _mediator.Send(new GetIssueQuery(id));
            return Ok(issue);
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var issues = await _mediator.Send(new GetIssuesQuery());
            return Ok(issues);
        }

        [HttpPost]
        public async Task<ActionResult> Create(IssueDto createIssueDto)
        {
            var result = await _mediator.Send(new CreateIssueCommand(createIssueDto, User));
            return Ok(result);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteIssueCommand(id));
            return Ok(result);
        }

        [HttpPut]
        public async Task<ActionResult> Update(IdempotentIssueDto idempotentIssueDto)
        {
            var result = await _mediator.Send(new UpdateIssueCommand(idempotentIssueDto));
            return Ok(result);
        }
    }
}
