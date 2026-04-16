using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using QuantityMeasurementApp.Api.Options;
using QuantityMeasurementApp.Business;
using QuantityMeasurementApp.Models.DTOs;

namespace QuantityMeasurementApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly GoogleAuthOptions _googleAuthOptions;

    public AuthController(IAuthService authService, IOptions<GoogleAuthOptions> googleAuthOptions)
    {
        _authService = authService;
        _googleAuthOptions = googleAuthOptions.Value;
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

    [AllowAnonymous]
    [HttpPost("google")]
    public async Task<ActionResult<AuthResponseDTO>> GoogleLogin([FromBody] GoogleLoginRequestDTO request)
    {
        if (string.IsNullOrWhiteSpace(_googleAuthOptions.ClientId))
        {
            return StatusCode(500, "Google authentication is not configured.");
        }

        if (string.IsNullOrWhiteSpace(request.IdToken))
        {
            return BadRequest("Google id token is required.");
        }

        try
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(
                request.IdToken,
                new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[] { _googleAuthOptions.ClientId },
                }
            );

            if (payload is null || string.IsNullOrWhiteSpace(payload.Email))
            {
                return Unauthorized("Invalid Google token payload.");
            }

            if (payload.EmailVerified is not true)
            {
                return Unauthorized("Google email must be verified.");
            }

            var displayName = string.IsNullOrWhiteSpace(payload.Name) ? payload.Email : payload.Name;
            return Ok(_authService.ExternalLogin(displayName, payload.Email));
        }
        catch (InvalidJwtException)
        {
            return Unauthorized("Invalid Google token.");
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