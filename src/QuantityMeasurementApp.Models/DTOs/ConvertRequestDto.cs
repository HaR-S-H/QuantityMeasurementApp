using System.ComponentModel.DataAnnotations;

namespace QuantityMeasurementApp.Models.DTOs
{
    public class ConvertRequestDto
    {
        [Required]
        public QuantityDTO Source { get; set; } = new();

        [Required]
        public string TargetUnit { get; set; } = string.Empty;
    }
}
