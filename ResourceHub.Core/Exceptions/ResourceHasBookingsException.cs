namespace ResourceHub.Core.Exceptions
{
    public class ResourceHasBookingsException : AppException
    {
        
        public ResourceHasBookingsException(string message)
            : base(message, 400)
        {
        }
    }
}