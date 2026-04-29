namespace ResourceHub.Shared.Forms
{
    public class BookingFormModel
    {
        public int ResourceId { get; set; }

        public DateTime StartTime { get; set; } = DateTime.Now;
        public DateTime EndTime { get; set; } = DateTime.Now.AddHours(1);

        public string BookedBy { get; set; } = string.Empty;
        public string Purpose { get; set; } = string.Empty;
    }
}