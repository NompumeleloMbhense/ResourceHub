using System.ComponentModel.DataAnnotations;
using ResourceHub.Api.Validation;

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

    public class CreateBookingDto
    {
        [Required]
        public int ResourceId { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        [DateGreaterThan("StartTime", ErrorMessage = "End time must be after start time.")]
        public DateTime EndTime { get; set; }
        [Required]
        public string BookedBy { get; set; } = string.Empty;
        [Required]
        public string Purpose { get; set; } = string.Empty;
    }

    public class UpdateBookingDto
    {
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        [DateGreaterThan("StartTime", ErrorMessage = "End time must be after start time.")]
        public DateTime EndTime { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string BookedBy { get; set; } = string.Empty;
        [Required]
        [StringLength(200, MinimumLength = 1)]
        public string Purpose { get; set; } = string.Empty;
    }
}
