namespace ResourceHub.Core.Exceptions
{
    public class ResourceHasBookingsException : Exception
    {
        public ResourceHasBookingsException(string message) : base(message) { }
    }
}