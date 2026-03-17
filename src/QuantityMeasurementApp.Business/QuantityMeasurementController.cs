using System;
using QuantityMeasurementApp.Models.DTOs;

namespace QuantityMeasurementApp.Business
{
    public class QuantityMeasurementController
    {
        private readonly IQuantityMeasurementService _service;

        public QuantityMeasurementController(IQuantityMeasurementService service)
        {
            _service = service;
        }

        public void DisplayResult(string message)
        {
            Console.WriteLine(message);
        }

        public void PerformCompare(QuantityDTO a, QuantityDTO b)
        {
            try
            {
                var result = _service.Compare(a, b);
                DisplayResult($"Comparison result: {result}");
            }
            catch (Exceptions.QuantityMeasurementException ex)
            {
                DisplayResult($"Error: {ex.Message}");
            }
        }

        public void PerformConvert(QuantityDTO source, string targetUnit)
        {
            try
            {
                var result = _service.Convert(source, targetUnit);
                DisplayResult($"Converted result: {result.Value} {result.Unit}");
            }
            catch (Exceptions.QuantityMeasurementException ex)
            {
                DisplayResult($"Error: {ex.Message}");
            }
        }

        public void PerformAdd(QuantityDTO a, QuantityDTO b, string? targetUnit = null)
        {
            try
            {
                var result = _service.Add(a, b, targetUnit);
                DisplayResult($"Addition result: {result.Value} {result.Unit}");
            }
            catch (Exceptions.QuantityMeasurementException ex)
            {
                DisplayResult($"Error: {ex.Message}");
            }
        }

        public void PerformSubtract(QuantityDTO a, QuantityDTO b, string? targetUnit = null)
        {
            try
            {
                var result = _service.Subtract(a, b, targetUnit);
                DisplayResult($"Subtraction result: {result.Value} {result.Unit}");
            }
            catch (Exceptions.QuantityMeasurementException ex)
            {
                DisplayResult($"Error: {ex.Message}");
            }
        }

        public void PerformDivide(QuantityDTO a, QuantityDTO b)
        {
            try
            {
                var result = _service.Divide(a, b);
                DisplayResult($"Division result: {result}");
            }
            catch (Exceptions.QuantityMeasurementException ex)
            {
                DisplayResult($"Error: {ex.Message}");
            }
        }
    }
}
