using System.ComponentModel.DataAnnotations;

namespace QuantityMeasurementApp.Models.DTOs
{
    public class CompareRequestDto
    {
        [Required]
        public QuantityDTO First { get; set; } = new();

        [Required]
        public QuantityDTO Second { get; set; } = new();
    }
}
