using VehicleServiceMonitoringSystem.Models;

namespace VehicleServiceMonitoringSystem.Repositories
{
    public interface IServiceJobRepository
    {
        IEnumerable<ServiceJob> GetAll();
        IEnumerable<ServiceJob> Search(string? searchTerm);
        ServiceJob? GetById(int id);
        ServiceJob Add(ServiceJob job);
        bool Update(ServiceJob job);
        string GenerateNextServiceNumber();
    }
}
