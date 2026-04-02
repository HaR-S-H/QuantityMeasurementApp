using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using QuantityMeasurementApp.Models.DTOs;
using QuantityMeasurementApp.Models.Entities;
using QuantityMeasurementApp.Repository;

namespace QuantityMeasurementApp.Business
{
    public sealed class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRevokedTokenRepository _revokedTokenRepository;
        private readonly JwtOptions _jwtOptions;

        public AuthService(
            IUserRepository userRepository,
            IRevokedTokenRepository revokedTokenRepository,
            IOptions<JwtOptions> jwtOptions
        )
        {
            _userRepository = userRepository;
            _revokedTokenRepository = revokedTokenRepository;
            _jwtOptions = jwtOptions.Value;
        }

        public AuthResponseDTO Signup(SignupRequestDTO request)
        {
            ArgumentNullException.ThrowIfNull(request);

            var name = request.Name?.Trim() ?? string.Empty;
            var email = NormalizeEmail(request.Email);
            var password = request.Password ?? string.Empty;

            ValidateSignup(name, email, password);

            if (_userRepository.GetByEmail(email) is not null)
            {
                throw new InvalidOperationException("User with this email already exists.");
            }

            var hash = BCrypt.Net.BCrypt.HashPassword(password);

            // BCrypt embeds salt and work factor into the hash string.
            var user = new UserEntity(name, email, hash, string.Empty);
            _userRepository.Add(user);

            return CreateAuthResponse(user);
        }

        public AuthResponseDTO Login(LoginRequestDTO request)
        {
            ArgumentNullException.ThrowIfNull(request);

            var email = NormalizeEmail(request.Email);
            var password = request.Password ?? string.Empty;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Email and password are required.");
            }

            var user = _userRepository.GetByEmail(email);
            if (user is null)
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            return CreateAuthResponse(user);
        }

        public void Logout(string tokenId, DateTime expiresAtUtc)
        {
            if (string.IsNullOrWhiteSpace(tokenId))
            {
                throw new ArgumentException("Token ID is required.", nameof(tokenId));
            }

            _revokedTokenRepository.RemoveExpiredTokens(DateTime.UtcNow);
            _revokedTokenRepository.RevokeToken(tokenId, expiresAtUtc);
        }

        public bool IsTokenRevoked(string tokenId)
        {
            return _revokedTokenRepository.IsRevoked(tokenId);
        }

        private static string NormalizeEmail(string email)
        {
            return (email ?? string.Empty).Trim().ToLowerInvariant();
        }

        private static void ValidateSignup(string name, string email, string password)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name is required.");
            }

            if (name.Length > 150)
            {
                throw new ArgumentException("Name cannot exceed 150 characters.");
            }

            if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
            {
                throw new ArgumentException("Valid email is required.");
            }

            if (password.Length < 6)
            {
                throw new ArgumentException("Password must be at least 6 characters long.");
            }
        }

        private AuthResponseDTO CreateAuthResponse(UserEntity user)
        {
            if (string.IsNullOrWhiteSpace(_jwtOptions.Secret))
            {
                throw new InvalidOperationException("JWT secret is not configured.");
            }

            var now = DateTime.UtcNow;
            var expiresAtUtc = now.AddMinutes(_jwtOptions.ExpiryMinutes <= 0 ? 60 : _jwtOptions.ExpiryMinutes);
            var tokenId = Guid.NewGuid().ToString("N");

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Name),
                new Claim(JwtRegisteredClaimNames.Jti, tokenId),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                notBefore: now,
                expires: expiresAtUtc,
                signingCredentials: credentials
            );

            return new AuthResponseDTO
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiresAtUtc = expiresAtUtc,
                Name = user.Name,
                Email = user.Email,
            };
        }
    }
}