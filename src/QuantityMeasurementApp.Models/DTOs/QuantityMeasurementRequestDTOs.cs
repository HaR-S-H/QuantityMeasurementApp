namespace QuantityMeasurementApp.Models.DTOs
{
    public class ConvertRequestDTO
    {
        public QuantityDTO Source { get; set; } = new();
        public string TargetUnit { get; set; } = string.Empty;
    }

    public class CompareRequestDTO
    {
        public QuantityDTO First { get; set; } = new();
        public QuantityDTO Second { get; set; } = new();
    }

    public class AddRequestDTO
    {
        public QuantityDTO First { get; set; } = new();
        public QuantityDTO Second { get; set; } = new();
        public string? TargetUnit { get; set; }
    }

    public class SubtractRequestDTO
    {
        public QuantityDTO First { get; set; } = new();
        public QuantityDTO Second { get; set; } = new();
        public string? TargetUnit { get; set; }
    }

    public class DivideRequestDTO
    {
        public QuantityDTO First { get; set; } = new();
        public QuantityDTO Second { get; set; } = new();
    }
}
