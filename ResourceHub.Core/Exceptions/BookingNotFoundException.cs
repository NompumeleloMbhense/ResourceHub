namespace ResourceHub.Core.Exceptions
{
    public class BookingNotFoundException : AppException
    {
        public BookingNotFoundException(string message) : base(message, 404) { }
    }
}