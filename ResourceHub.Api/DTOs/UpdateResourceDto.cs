using System.ComponentModel.DataAnnotations;

namespace ResourceHub.Api.DTOs
{
    public class UpdateResourceDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Required]
        public string Location { get; set; } = string.Empty;
        [Range(1, int.MaxValue, ErrorMessage = "Capacity must be greater than 0")]
        public int Capacity { get; set; }
        public bool IsAvailable { get; set; }
    }
}