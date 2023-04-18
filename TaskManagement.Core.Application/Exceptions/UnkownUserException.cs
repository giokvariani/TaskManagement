namespace TaskManagement.Core.Application.Exceptions
{
    public class UnknownUserException : InvalidOperationException
    {
        public UnknownUserException() : base("Unkown User is detected")
        {

        }
    }
}
