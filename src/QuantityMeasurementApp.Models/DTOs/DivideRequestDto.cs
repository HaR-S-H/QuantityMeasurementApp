using System.ComponentModel.DataAnnotations;

namespace QuantityMeasurementApp.Models.DTOs
{
    public class DivideRequestDto
    {
        [Required]
        public QuantityDTO Dividend { get; set; } = new();

        [Required]
        public QuantityDTO Divisor { get; set; } = new();
    }
}
