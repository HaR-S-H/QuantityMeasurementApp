using System;
using QuantityMeasurementApp.Models.DTOs;

namespace QuantityMeasurementApp.Business
{
    public interface IAuthService
    {
        AuthResponseDTO Signup(SignupRequestDTO request);
        AuthResponseDTO Login(LoginRequestDTO request);
        void Logout(string tokenId, DateTime expiresAtUtc);
        bool IsTokenRevoked(string tokenId);
    }
}