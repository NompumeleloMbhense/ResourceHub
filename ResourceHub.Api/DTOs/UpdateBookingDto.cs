using System.ComponentModel.DataAnnotations;
using Microsoft.OpenApi.MicrosoftExtensions;
using ResourceHub.Api.Validation;

namespace ResourceHub.Api.DTOs
{
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
