namespace ResourceHub.Core.Exceptions
{
    public class ResourceUnavailableException : AppException
    {
        public ResourceUnavailableException(string message) : base(message, 400) { }
    }
}