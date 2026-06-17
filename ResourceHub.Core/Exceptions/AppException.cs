/// <summary>
/// Base class for application-specific exceptions. This class provides a common structure 
/// for exceptions that are thrown within the application, allowing for consistent
/// error handling and response generation. Each derived exception class can specify its
/// own HTTP status code and message, which can be used by middleware to generate appropriate
/// HTTP responses.
/// </summary>


namespace ResourceHub.Core.Exceptions
{
    public abstract class AppException : Exception
    {
        public int StatusCode { get; }

        protected AppException(string message, int statusCode): base(message)
        {
            StatusCode = statusCode;
        }
    }
}