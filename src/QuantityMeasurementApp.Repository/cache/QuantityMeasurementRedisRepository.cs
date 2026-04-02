using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using QuantityMeasurementApp.Models.Entities;
using StackExchange.Redis;

namespace QuantityMeasurementApp.Repository
{
    /// <summary>
    /// Cache-aside repository that uses Redis for read optimization and SQL repository as source of truth.
    /// </summary>
    public sealed class QuantityMeasurementRedisRepository : IQuantityMeasurementRepository
    {
        private const string CacheKeyAll = "quantity-measurements:all";
        private static readonly TimeSpan CacheTtl = TimeSpan.FromMinutes(10);

        private readonly IQuantityMeasurementRepository _databaseRepository;
        private readonly IDatabase _redisDatabase;

        public QuantityMeasurementRedisRepository(
            IQuantityMeasurementRepository databaseRepository,
            IConnectionMultiplexer connectionMultiplexer
        )
        {
            _databaseRepository = databaseRepository;
            _redisDatabase = connectionMultiplexer.GetDatabase();
        }

        public void Save(QuantityMeasurementEntity entity)
        {
            _databaseRepository.Save(entity);
            // Invalidate relevant cache keys to keep reads consistent after writes.
            _redisDatabase.KeyDelete(CacheKeyAll);
            if (entity.UserId.HasValue)
            {
                _redisDatabase.KeyDelete(GetUserCacheKey(entity.UserId.Value));
            }
        }

        public IEnumerable<QuantityMeasurementEntity> GetAll()
        {
            // Return cached payload when available to avoid repeated SQL reads.
            var cached = _redisDatabase.StringGet(CacheKeyAll);
            if (cached.HasValue)
            {
                var payload = JsonSerializer.Deserialize<List<CachedQuantityMeasurement>>(cached!);
                if (payload is not null)
                {
                    return payload
                        .Select(entry =>
                            QuantityMeasurementEntity.Rehydrate(
                                entry.Id,
                                entry.UserId,
                                entry.Description,
                                entry.IsError,
                                entry.ErrorMessage,
                                entry.CreatedAt
                            )
                        )
                        .ToList();
                }
            }

            var fresh = _databaseRepository.GetAll().ToList();
            var serialized = JsonSerializer.Serialize(
                fresh.Select(entry => new CachedQuantityMeasurement(entry)).ToList()
            );
            // Cache fresh data with TTL to reduce DB load while limiting staleness.
            _redisDatabase.StringSet(CacheKeyAll, serialized, CacheTtl);

            return fresh;
        }

        public IEnumerable<QuantityMeasurementEntity> GetByUserId(Guid userId)
        {
            var cacheKey = GetUserCacheKey(userId);
            var cached = _redisDatabase.StringGet(cacheKey);
            if (cached.HasValue)
            {
                var payload = JsonSerializer.Deserialize<List<CachedQuantityMeasurement>>(cached!);
                if (payload is not null)
                {
                    return payload
                        .Select(entry =>
                            QuantityMeasurementEntity.Rehydrate(
                                entry.Id,
                                entry.UserId,
                                entry.Description,
                                entry.IsError,
                                entry.ErrorMessage,
                                entry.CreatedAt
                            )
                        )
                        .ToList();
                }
            }

            var fresh = _databaseRepository.GetByUserId(userId).ToList();
            var serialized = JsonSerializer.Serialize(
                fresh.Select(entry => new CachedQuantityMeasurement(entry)).ToList()
            );
            _redisDatabase.StringSet(cacheKey, serialized, CacheTtl);

            return fresh;
        }

        private static string GetUserCacheKey(Guid userId) => $"quantity-measurements:user:{userId}";

        private sealed record CachedQuantityMeasurement(
            Guid Id,
            Guid? UserId,
            string Description,
            bool IsError,
            string ErrorMessage,
            DateTime CreatedAt
        )
        {
            public CachedQuantityMeasurement(QuantityMeasurementEntity source)
                : this(
                    source.Id,
                    source.UserId,
                    source.Description,
                    source.IsError,
                    source.ErrorMessage,
                    source.CreatedAt
                ) { }
        }
    }
}