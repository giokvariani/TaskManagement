using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManagement.API.JsonWebToken;
using TaskManagement.Core.Application.Interfaces;

namespace TaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JWTTokenController : ControllerBase
    {
        public IConfiguration _configiuration;
        public IUserRepository _userRepository;
        public JWTTokenController(IConfiguration configiuration, IUserRepository userRepository)
        {
            _configiuration = configiuration;
            _userRepository = userRepository;
        }
        [HttpPost]
        public async Task<ActionResult> GenerateToken(string identifier, string password)
        {
            var user = (await _userRepository.GetAsync(x => (x.UserName == identifier || x.Email == identifier) && x.Password == password)).SingleOrDefault();
            if (user == null)
                throw new InvalidOperationException("Invalid identifier or password");

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
            var token = new JwtSecurityToken(jwt.Issuer, jwt.Audience, claims, expires: DateTime.Now.AddMinutes(20), signingCredentials: signIn);
            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }
    }
}
