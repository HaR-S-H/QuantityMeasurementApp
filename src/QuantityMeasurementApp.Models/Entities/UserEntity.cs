using System;

namespace QuantityMeasurementApp.Models.Entities
{
    [Serializable]
    public class UserEntity
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string PasswordHash { get; private set; }
        public string PasswordSalt { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public UserEntity(string name, string email, string passwordHash, string passwordSalt)
        {
            Id = Guid.NewGuid();
            Name = name;
            Email = email;
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
            CreatedAt = DateTime.UtcNow;
        }

        private UserEntity(
            Guid id,
            string name,
            string email,
            string passwordHash,
            string passwordSalt,
            DateTime createdAt
        )
        {
            Id = id;
            Name = name;
            Email = email;
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
            CreatedAt = createdAt;
        }

        public static UserEntity Rehydrate(
            Guid id,
            string name,
            string email,
            string passwordHash,
            string passwordSalt,
            DateTime createdAt
        )
        {
            return new UserEntity(id, name, email, passwordHash, passwordSalt, createdAt);
        }
    }
}