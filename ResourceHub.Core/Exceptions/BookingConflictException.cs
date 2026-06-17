namespace ResourceHub.Core.Exceptions
{
    public class BookingConflictException : AppException
    {
        public BookingConflictException(string message) : base(message, 409){}
    }
}