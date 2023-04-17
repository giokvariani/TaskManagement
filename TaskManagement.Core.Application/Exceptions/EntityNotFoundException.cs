using System.Net;

namespace TaskManagement.Core.Application.Exceptions
{
    public class EntityNotFoundException : EntityValidationException
    {
        public EntityNotFoundException() : base("ჩანაწერი არ მოიძებნა")
        {
        }
        public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;
    }
}
