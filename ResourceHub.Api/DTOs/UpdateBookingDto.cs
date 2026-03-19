namespace ResourceHub.Api.DTOs
{
    public class UpdateBookingDto
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string BookedBy { get; set; } = string.Empty;
        public string Purpose { get; set; } = string.Empty;
    }
}
