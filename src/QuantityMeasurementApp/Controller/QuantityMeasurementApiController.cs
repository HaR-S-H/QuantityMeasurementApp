using Microsoft.AspNetCore.Mvc;
using QuantityMeasurementApp.Business;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Models.DTOs;

namespace QuantityMeasurementApp.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuantityMeasurementApiController : ControllerBase
    {
        private readonly IQuantityMeasurementService _service;

        public QuantityMeasurementApiController(IQuantityMeasurementService service)
        {
            _service = service;
        }

        [HttpPost("compare")]
        public async Task<ActionResult<object>> Compare(
            [FromBody] CompareRequestDto request,
            CancellationToken cancellationToken
        )
        {
            var result = await _service.CompareAsync(
                request.First,
                request.Second,
                cancellationToken
            );
            return Ok(new { areEqual = result });
        }

        [HttpPost("convert")]
        public async Task<ActionResult<QuantityDTO>> Convert(
            [FromBody] ConvertRequestDto request,
            CancellationToken cancellationToken
        )
        {
            var result = await _service.ConvertAsync(
                request.Source,
                request.TargetUnit,
                cancellationToken
            );
            return Ok(result);
        }

        [HttpPost("add")]
        public async Task<ActionResult<QuantityDTO>> Add(
            [FromBody] ArithmeticRequestDto request,
            CancellationToken cancellationToken
        )
        {
            var result = await _service.AddAsync(
                request.First,
                request.Second,
                request.TargetUnit,
                cancellationToken
            );
            return Ok(result);
        }

        [HttpPost("subtract")]
        public async Task<ActionResult<QuantityDTO>> Subtract(
            [FromBody] ArithmeticRequestDto request,
            CancellationToken cancellationToken
        )
        {
            var result = await _service.SubtractAsync(
                request.First,
                request.Second,
                request.TargetUnit,
                cancellationToken
            );
            return Ok(result);
        }

        [HttpPost("divide")]
        public async Task<ActionResult<object>> Divide(
            [FromBody] DivideRequestDto request,
            CancellationToken cancellationToken
        )
        {
            var result = await _service.DivideAsync(
                request.Dividend,
                request.Divisor,
                cancellationToken
            );
            return Ok(new { result });
        }

        [HttpGet("history")]
        public async Task<ActionResult<IReadOnlyList<OperationHistoryDto>>> History(
            [FromQuery] OperationType? operationType,
            CancellationToken cancellationToken
        )
        {
            var result = await _service.GetHistoryAsync(operationType, cancellationToken);
            return Ok(result);
        }
    }
}
