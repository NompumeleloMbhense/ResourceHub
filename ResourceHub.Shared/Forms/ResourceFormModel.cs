using System.ComponentModel.DataAnnotations;

namespace ResourceHub.Shared.Forms
{
    public class ResourceFormModel
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        public string Description { get; set; } = string.Empty;
        
        [Required]
        public string Location { get; set; } = string.Empty;
        
        [Range(1, 1000)]
        public int Capacity { get; set; }
        public bool IsAvailable { get; set; } = true;
    }
}