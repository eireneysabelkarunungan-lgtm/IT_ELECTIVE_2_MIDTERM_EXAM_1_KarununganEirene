using VehicleServiceMonitoringSystem.Models;

namespace VehicleServiceMonitoringSystem.Repositories
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAll();
        User? GetById(int id);
        User? GetByUsername(string username);
        bool UsernameExists(string username);
        bool EmailExists(string email);
        User Add(User user);
    }
}
