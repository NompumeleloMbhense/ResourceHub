namespace ResourceHub.Api.DTOs
{
    public class BookingDto
    {
        public int Id { get; set; }
        public int ResourceId { get; set; }
        public string ResourceName { get; set; } = string.Empty;

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public string BookedBy { get; set; } = string.Empty;
        public string Purpose { get; set; } = string.Empty;
    }
}
