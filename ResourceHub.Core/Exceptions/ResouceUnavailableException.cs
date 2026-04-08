namespace ResourceHub.Core.Exceptions
{
    public class ResourceUnavailableException : Exception
    {
        public ResourceUnavailableException(string message) : base(message) { }
    }
}