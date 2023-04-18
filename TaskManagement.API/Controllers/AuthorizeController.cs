using Microsoft.AspNetCore.Mvc;
using TaskManagement.Core.Application.Interfaces;
using TaskManagement.Core.Application.Services;

namespace TaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizeController : ControllerBase
    {
        private readonly IAuthorizeService _authorizeService;
        public AuthorizeController(IAuthorizeService authorizeService) => _authorizeService = authorizeService;

        [HttpPost("GenerateToken")]
        public async Task<ActionResult> GenerateToken(string identifier, string password)
        {
            var token = await _authorizeService.GenerateToken(identifier, password);
            return Ok(token);
        }

        [HttpGet("ShowValidToken")]
        public async Task<ActionResult> ShowValidToken(string identifier, string password)
        {
            var activeToken = await _authorizeService.ShowValidToken(identifier, password);
            return Ok(activeToken);
        }
    }
}
