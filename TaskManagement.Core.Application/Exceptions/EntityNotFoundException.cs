using System.Net;

namespace TaskManagement.Core.Application.Exceptions
{
    public class EntityNotFoundException : EntityValidationException
    {
        public EntityNotFoundException(string message) : base(message)
        {
        }
        public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;
    }
}
