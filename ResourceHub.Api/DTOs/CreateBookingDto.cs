namespace ResourceHub.Api.DTOs
{
    public class CreateBookingDto
    {
        public int ResourceId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string BookedBy { get; set; } = string.Empty;
        public string Purpose { get; set; } = string.Empty;
    }
}
