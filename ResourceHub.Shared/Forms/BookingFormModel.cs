using System.ComponentModel.DataAnnotations;

namespace ResourceHub.Shared.Forms
{
    public class BookingFormModel
    {
        [Required(ErrorMessage = "Please select a resource.")]
        public int ResourceId { get; set; }

        [Required(ErrorMessage = "Start time is required")]
        public DateTime StartTime { get; set; }

        [Required(ErrorMessage = "End time is required")]
        public DateTime EndTime { get; set; }


        [Required(ErrorMessage = "Booked By is required")]
        [StringLength(100, ErrorMessage = "Booked By cannot exceed 100 characters")]
        public string BookedBy { get; set; } = string.Empty;

        [Required(ErrorMessage = "Purpose is required")]
        [StringLength(250, ErrorMessage = "Purpose cannot exceed 250 characters")]
        public string Purpose { get; set; } = string.Empty;
    }
}