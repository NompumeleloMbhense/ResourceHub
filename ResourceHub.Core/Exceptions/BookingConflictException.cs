namespace ResourceHub.Core.Exceptions
{
    public class BookingConflictException : Exception
    {
        public BookingConflictException(string message) : base(message){}
    }
}