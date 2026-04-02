using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuantityMeasurementApp.Business;
using QuantityMeasurementApp.Models.DTOs;

namespace QuantityMeasurementApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [AllowAnonymous]
    [HttpPost("signup")]
    public ActionResult<AuthResponseDTO> Signup([FromBody] SignupRequestDTO request)
    {
        try
        {
            return Ok(_authService.Signup(request));
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public ActionResult<AuthResponseDTO> Login([FromBody] LoginRequestDTO request)
    {
        try
        {
            return Ok(_authService.Login(request));
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        var tokenId = User.FindFirstValue(JwtRegisteredClaimNames.Jti);
        var expiresAtUnix = User.FindFirstValue(JwtRegisteredClaimNames.Exp);

        if (string.IsNullOrWhiteSpace(tokenId) || string.IsNullOrWhiteSpace(expiresAtUnix))
        {
            return BadRequest("Unable to resolve token information from claims.");
        }

        if (!long.TryParse(expiresAtUnix, out var expiresUnixSeconds))
        {
            return BadRequest("Invalid token expiry claim.");
        }

        var expiresAtUtc = DateTimeOffset.FromUnixTimeSeconds(expiresUnixSeconds).UtcDateTime;
        _authService.Logout(tokenId, expiresAtUtc);

        return Ok(new { message = "Logged out successfully." });
    }
}