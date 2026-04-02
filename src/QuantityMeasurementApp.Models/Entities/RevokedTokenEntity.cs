using System;

namespace QuantityMeasurementApp.Models.Entities
{
    [Serializable]
    public class RevokedTokenEntity
    {
        public Guid Id { get; private set; }
        public string TokenId { get; private set; }
        public DateTime ExpiresAtUtc { get; private set; }
        public DateTime RevokedAtUtc { get; private set; }

        public RevokedTokenEntity(string tokenId, DateTime expiresAtUtc)
        {
            Id = Guid.NewGuid();
            TokenId = tokenId;
            ExpiresAtUtc = expiresAtUtc;
            RevokedAtUtc = DateTime.UtcNow;
        }

        private RevokedTokenEntity(Guid id, string tokenId, DateTime expiresAtUtc, DateTime revokedAtUtc)
        {
            Id = id;
            TokenId = tokenId;
            ExpiresAtUtc = expiresAtUtc;
            RevokedAtUtc = revokedAtUtc;
        }

        public static RevokedTokenEntity Rehydrate(
            Guid id,
            string tokenId,
            DateTime expiresAtUtc,
            DateTime revokedAtUtc
        )
        {
            return new RevokedTokenEntity(id, tokenId, expiresAtUtc, revokedAtUtc);
        }
    }
}