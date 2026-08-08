using VehicleServiceMonitoringSystem.Models;

namespace VehicleServiceMonitoringSystem.Repositories
{
    /// <summary>
    /// In-memory repository backed by a static List, as required by the spec
    /// (no database). Registered as a Singleton so the same static-backed
    /// store is shared across requests for the lifetime of the application.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private static readonly List<User> _users = new();
        private static int _nextId = 1;
        private static readonly object _lock = new();

        public IEnumerable<User> GetAll()
        {
            lock (_lock)
            {
                return _users.ToList();
            }
        }

        public User? GetById(int id)
        {
            lock (_lock)
            {
                return _users.FirstOrDefault(u => u.Id == id);
            }
        }

        public User? GetByUsername(string username)
        {
            lock (_lock)
            {
                return _users.FirstOrDefault(u =>
                    string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase));
            }
        }

        public bool UsernameExists(string username)
        {
            lock (_lock)
            {
                return _users.Any(u =>
                    string.Equals(u.Username, username, StringComparison.OrdinalIgnoreCase));
            }
        }

        public bool EmailExists(string email)
        {
            lock (_lock)
            {
                return _users.Any(u =>
                    string.Equals(u.Email, email, StringComparison.OrdinalIgnoreCase));
            }
        }

        public User Add(User user)
        {
            lock (_lock)
            {
                user.Id = _nextId++;
                _users.Add(user);
                return user;
            }
        }
    }
}
