using System.Net;

namespace TaskManagement.Core.Application.Exceptions
{
    public class EntityNotFoundException : EntityValidationException
    {
        const string Text = "ჩანაწერი არ მოიძებნა";
        public EntityNotFoundException() : base(Text)
        {
        }
        public EntityNotFoundException(string message) : base($"ასეთი {message} {Text}")
        {

        }
        public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;
    }
}
