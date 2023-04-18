namespace TaskManagement.Infrastructure.Persistence.JsonWebToken
{
    public class Jwt
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string key { get; set; }
        public string Subject { get; set; }
    }
}
