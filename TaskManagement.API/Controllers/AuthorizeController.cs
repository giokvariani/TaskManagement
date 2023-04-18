using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManagement.API.JsonWebToken;
using TaskManagement.Core.Application.Interfaces;
using TaskManagement.Core.Domain.Entities;

namespace TaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizeController : ControllerBase
    {
        public IConfiguration _configiuration;
        public IUserRepository _userRepository;
        public IPersonalAccessTokenRepository _personalAccessTokenRepository;
        public AuthorizeController(IConfiguration configiuration, IUserRepository userRepository , IPersonalAccessTokenRepository personalAccessTokenRepository)
        {
            _configiuration = configiuration;
            _userRepository = userRepository;
            _personalAccessTokenRepository = personalAccessTokenRepository;
        }
        [HttpPost("GenerateToken")]
        public async Task<ActionResult> GenerateToken(string identifier, string password)
        {
            var validTokenDuration = _configiuration.GetValue<int>("ValidTokenDuration");

            var user = (await _userRepository.GetAsync(x => (x.UserName == identifier || x.Email == identifier) && x.Password == password)).SingleOrDefault();
            if (user == null)
                throw new InvalidOperationException("Invalid identifier or password");

            var tokens = await _personalAccessTokenRepository.GetAsync(x => x.UserId == user.Id);
            var lastToken = tokens.OrderByDescending(x => x.CreatedOn).FirstOrDefault();
            if (lastToken != null && lastToken.CreatedOn > DateTime.Now.AddMinutes(validTokenDuration))
                return BadRequest("Please use already generated token");

            var jwt = _configiuration.GetSection("Jwt").Get<Jwt>();
            var claims = new[]
            {
                        new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("Id", user.Id.ToString()),
                        new Claim("UserName", user.UserName),
                        new Claim("Password", user.Password)
                    };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.key));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            //TO DO expire Config
            var token = new JwtSecurityToken(jwt.Issuer, jwt.Audience, claims, expires: DateTime.Now.AddMinutes(validTokenDuration), signingCredentials: signIn);
            var generatedKey = new JwtSecurityTokenHandler().WriteToken(token);
            var personalAccessToken = new PersonalAccessToken() { Identifier = identifier, Password = password, UserId = user.Id, Value = generatedKey };
            await _personalAccessTokenRepository.CreateAsync(personalAccessToken);
            return Ok(generatedKey);
        }

        [HttpGet("ShowValidToken")]
        public async Task<ActionResult> ShowValidToken(string identifier, string password)
        {
            var validTokenDuration = _configiuration.GetValue<int>("ValidTokenDuration");
            var personalAccessTokens = (await _personalAccessTokenRepository.GetAsync(x => x.Identifier == identifier && password == x.Password)).ToList();
            //TO DO expire Config
            var activeTokens = personalAccessTokens.Where(x => x.CreatedOn.AddMinutes(validTokenDuration) > DateTime.Now).OrderBy(x => x.Identifier);
            return Ok(activeTokens);
        }
    }
}
