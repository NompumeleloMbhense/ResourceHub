using System.ComponentModel.DataAnnotations;
using ResourceHub.Api.Validation;

namespace ResourceHub.Api.DTOs
{
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
}
