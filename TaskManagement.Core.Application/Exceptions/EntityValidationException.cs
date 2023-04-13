using System.Net;

namespace TaskManagement.Core.Application.Exceptions
{
    public abstract class EntityValidationException : Exception
    {
        public EntityValidationException(string message) : base(message)
        {

        }
        public abstract HttpStatusCode StatusCode { get; }

    }
}
