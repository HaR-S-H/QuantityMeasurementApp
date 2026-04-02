using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using QuantityMeasurementApp.Business;
using QuantityMeasurementApp.Models.DTOs;
using QuantityMeasurementApp.Models.Entities;
using QuantityMeasurementApp.Repository;

namespace QuantityMeasurementApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuantityMeasurementController : ControllerBase
{
    private readonly IQuantityMeasurementService _service;
    private readonly IQuantityMeasurementRepository _repository;
    private readonly IUserRepository _userRepository;

    public QuantityMeasurementController(
        IQuantityMeasurementService service,
        IQuantityMeasurementRepository repository,
        IUserRepository userRepository
    )
    {
        _service = service;
        _repository = repository;
        _userRepository = userRepository;
    }

    [AllowAnonymous]
    [HttpPost("convert")]
    public ActionResult<QuantityDTO> Convert([FromBody] ConvertRequestDTO request)
    {
        try
        {
            var result = _service.Convert(request.Source, request.TargetUnit);
            SaveAudit(
                $"Convert: {request.Source.Value} {request.Source.Unit} to {request.TargetUnit} => {result.Value} {result.Unit}"
            );
            return Ok(result);
        }
        catch (Exception ex)
        {
            SaveAuditError(
                $"Convert failed for {request.Source.Value} {request.Source.Unit} to {request.TargetUnit}",
                ex.Message
            );
            return BadRequest(ex.Message);
        }
    }

    [AllowAnonymous]
    [HttpPost("compare")]
    public ActionResult<bool> Compare([FromBody] CompareRequestDTO request)
    {
        try
        {
            var result = _service.Compare(request.First, request.Second);
            SaveAudit(
                $"Compare: {request.First.Value} {request.First.Unit} vs {request.Second.Value} {request.Second.Unit} => {result}"
            );
            return Ok(result);
        }
        catch (Exception ex)
        {
            SaveAuditError(
                $"Compare failed for {request.First.Value} {request.First.Unit} and {request.Second.Value} {request.Second.Unit}",
                ex.Message
            );
            return BadRequest(ex.Message);
        }
    }

    [AllowAnonymous]
    [HttpPost("add")]
    public ActionResult<QuantityDTO> Add([FromBody] AddRequestDTO request)
    {
        try
        {
            var result = _service.Add(request.First, request.Second, request.TargetUnit);
            SaveAudit(
                $"Add: {request.First.Value} {request.First.Unit} + {request.Second.Value} {request.Second.Unit} => {result.Value} {result.Unit}"
            );
            return Ok(result);
        }
        catch (Exception ex)
        {
            SaveAuditError(
                $"Add failed for {request.First.Value} {request.First.Unit} and {request.Second.Value} {request.Second.Unit}",
                ex.Message
            );
            return BadRequest(ex.Message);
        }
    }

    [AllowAnonymous]
    [HttpPost("subtract")]
    public ActionResult<QuantityDTO> Subtract([FromBody] SubtractRequestDTO request)
    {
        try
        {
            var result = _service.Subtract(request.First, request.Second, request.TargetUnit);
            SaveAudit(
                $"Subtract: {request.First.Value} {request.First.Unit} - {request.Second.Value} {request.Second.Unit} => {result.Value} {result.Unit}"
            );
            return Ok(result);
        }
        catch (Exception ex)
        {
            SaveAuditError(
                $"Subtract failed for {request.First.Value} {request.First.Unit} and {request.Second.Value} {request.Second.Unit}",
                ex.Message
            );
            return BadRequest(ex.Message);
        }
    }

    [AllowAnonymous]
    [HttpPost("divide")]
    public ActionResult<double> Divide([FromBody] DivideRequestDTO request)
    {
        try
        {
            var result = _service.Divide(request.First, request.Second);
            SaveAudit(
                $"Divide: {request.First.Value} {request.First.Unit} / {request.Second.Value} {request.Second.Unit} => {result}"
            );
            return Ok(result);
        }
        catch (Exception ex)
        {
            SaveAuditError(
                $"Divide failed for {request.First.Value} {request.First.Unit} and {request.Second.Value} {request.Second.Unit}",
                ex.Message
            );
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpGet("history")]
    public ActionResult<IReadOnlyList<OperationHistoryDTO>> GetHistory()
    {
        var userId = GetCurrentUserIdOrNull();
        if (!userId.HasValue)
        {
            return Unauthorized("Authenticated user id claim is missing or invalid.");
        }

        var entries = _repository
            .GetByUserId(userId.Value)
            .Select(entry => new OperationHistoryDTO
            {
                CreatedAt = entry.CreatedAt,
                Description = entry.Description,
                IsError = entry.IsError,
                ErrorMessage = entry.ErrorMessage,
            })
            .ToList();

        return Ok(entries);
    }

    private void SaveAudit(string description)
    {
        try
        {
            _repository.Save(new QuantityMeasurementEntity(description, GetCurrentUserIdOrNull()));
        }
        catch
        {
            // Do not fail user-facing operations when audit persistence is unavailable.
        }
    }

    private void SaveAuditError(string description, string error)
    {
        try
        {
            _repository.Save(new QuantityMeasurementEntity(description, error, GetCurrentUserIdOrNull()));
        }
        catch
        {
            // Keep original API error response when error-audit persistence fails.
        }
    }

    private Guid? GetCurrentUserIdOrNull()
    {
        var userIdClaim = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (Guid.TryParse(userIdClaim, out var userId) && _userRepository.Exists(userId))
        {
            return userId;
        }

        return null;
    }
}
