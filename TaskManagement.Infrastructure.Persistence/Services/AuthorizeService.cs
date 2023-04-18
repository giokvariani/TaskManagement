using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManagement.Core.Application.Interfaces;
using TaskManagement.Core.Application.Services;
using TaskManagement.Core.Domain.Entities;
using TaskManagement.Infrastructure.Persistence.JsonWebToken;

namespace TaskManagement.Infrastructure.Persistence.Services
{
    public class AuthorizeService : IAuthorizeService
    {
        const string ValidTokenDuration = nameof(ValidTokenDuration);
        public IConfiguration _configiuration;
        public IUserRepository _userRepository;
        public IPersonalAccessTokenRepository _personalAccessTokenRepository;
        public AuthorizeService(IConfiguration configiuration, IUserRepository userRepository, IPersonalAccessTokenRepository personalAccessTokenRepository)
        {
            _configiuration = configiuration;
            _userRepository = userRepository;
            _personalAccessTokenRepository = personalAccessTokenRepository;
        }

        public async Task<string> ShowValidToken(string identifier, string password)
        {
            var user = await CheckUser(identifier, password);
            var validTokenDuration = _configiuration.GetValue<int>(ValidTokenDuration);
            var personalAccessTokens = (await _personalAccessTokenRepository.GetAsync(x => x.UserId == user.Id)).ToList();
            var activeToken = personalAccessTokens
                .Where(x => x.CreatedOn.AddMinutes(validTokenDuration) > DateTime.Now)
                .OrderBy(x => x.Identifier)
                .FirstOrDefault();
            return activeToken == null ? string.Empty : activeToken.Value;
        }

        private async Task<User> CheckUser(string identifier, string password)
        {
            var userByFirstAttempt = (await _userRepository.GetAsync(x => x.UserName == identifier && x.Password == password)).SingleOrDefault();
            var user = userByFirstAttempt == null ?
                (await _userRepository.GetAsync(x => x.Email == identifier && x.Password == password)).SingleOrDefault()
                : userByFirstAttempt;

            if (user == null)
                throw new InvalidOperationException("Invalid identifier or password");

            return user;
        }

        public async Task<string> GenerateToken(string identifier, string password)
        {
            var user = await CheckUser(identifier, password);
            var tokens = await _personalAccessTokenRepository.GetAsync(x => x.UserId == user.Id);
            var lastToken = tokens.OrderByDescending(x => x.CreatedOn).FirstOrDefault();

            var validTokenDuration = _configiuration.GetValue<int>(ValidTokenDuration);
            if (lastToken != null && lastToken.CreatedOn > DateTime.Now.AddMinutes(validTokenDuration))
                throw new InvalidOperationException("Please use already generated token");

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
            var token = new JwtSecurityToken(jwt.Issuer, jwt.Audience, claims, expires: DateTime.Now.AddMinutes(validTokenDuration), signingCredentials: signIn);
            var generatedKey = new JwtSecurityTokenHandler().WriteToken(token);
            var personalAccessToken = new PersonalAccessToken() { Identifier = user.Email, Password = user.Password, UserId = user.Id, Value = generatedKey };
            await _personalAccessTokenRepository.CreateAsync(personalAccessToken);
            return generatedKey;
        }
    }
}
