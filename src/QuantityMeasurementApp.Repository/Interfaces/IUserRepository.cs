using QuantityMeasurementApp.Models.Entities;

namespace QuantityMeasurementApp.Repository
{
    public interface IUserRepository
    {
        UserEntity? GetByEmail(string email);
        bool Exists(Guid userId);
        void Add(UserEntity user);
    }
}