using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using QuantityMeasurementApp.Models.Entities;

namespace QuantityMeasurementApp.Repository
{
    public sealed class UserDatabaseRepository : IUserRepository
    {
        private readonly QuantityMeasurementDbContext _dbContext;

        public UserDatabaseRepository(QuantityMeasurementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public UserEntity? GetByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email is required.", nameof(email));
            }

            var normalizedEmail = email.Trim().ToLowerInvariant();

            return _dbContext.Users.AsNoTracking().FirstOrDefault(user => user.Email == normalizedEmail);
        }

        public bool Exists(Guid userId)
        {
            return _dbContext.Users.AsNoTracking().Any(user => user.Id == userId);
        }

        public void Add(UserEntity user)
        {
            ArgumentNullException.ThrowIfNull(user);
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
        }
    }
}