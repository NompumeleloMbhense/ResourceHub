using System.ComponentModel.DataAnnotations;

namespace ResourceHub.Api.DTOs
{
    public class CreateResourceDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Required]
        public string Location { get; set; } = string.Empty;
        [Range(1, int.MaxValue)]
        public int Capacity { get; set; }
    }
}