using System;

namespace QuantityMeasurementApp.Repository
{
    public interface IRevokedTokenRepository
    {
        void RevokeToken(string tokenId, DateTime expiresAtUtc);
        bool IsRevoked(string tokenId);
        void RemoveExpiredTokens(DateTime utcNow);
    }
}