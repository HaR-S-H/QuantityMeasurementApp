using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using QuantityMeasurementApp.Models.Entities;

namespace QuantityMeasurementApp.Repository
{
    public sealed class RevokedTokenDatabaseRepository : IRevokedTokenRepository
    {
        private readonly QuantityMeasurementDbContext _dbContext;

        public RevokedTokenDatabaseRepository(QuantityMeasurementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void RevokeToken(string tokenId, DateTime expiresAtUtc)
        {
            if (string.IsNullOrWhiteSpace(tokenId))
            {
                throw new ArgumentException("Token ID is required.", nameof(tokenId));
            }

            if (IsRevoked(tokenId))
            {
                return;
            }

            _dbContext.RevokedTokens.Add(new RevokedTokenEntity(tokenId, expiresAtUtc));
            _dbContext.SaveChanges();
        }

        public bool IsRevoked(string tokenId)
        {
            if (string.IsNullOrWhiteSpace(tokenId))
            {
                return false;
            }

            return _dbContext.RevokedTokens.AsNoTracking().Any(token => token.TokenId == tokenId);
        }

        public void RemoveExpiredTokens(DateTime utcNow)
        {
            var expiredTokens = _dbContext.RevokedTokens.Where(token => token.ExpiresAtUtc <= utcNow).ToList();
            if (expiredTokens.Count == 0)
            {
                return;
            }

            _dbContext.RevokedTokens.RemoveRange(expiredTokens);
            _dbContext.SaveChanges();
        }
    }
}