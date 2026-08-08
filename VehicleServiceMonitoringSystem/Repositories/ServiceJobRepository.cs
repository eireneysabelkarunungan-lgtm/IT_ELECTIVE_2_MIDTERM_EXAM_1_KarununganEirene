using VehicleServiceMonitoringSystem.Models;

namespace VehicleServiceMonitoringSystem.Repositories
{
    /// <summary>
    /// In-memory repository backed by a static List, as required by the spec
    /// (no database). Registered as a Singleton so the same static-backed
    /// store is shared across requests for the lifetime of the application.
    /// </summary>
    public class ServiceJobRepository : IServiceJobRepository
    {
        private static readonly List<ServiceJob> _jobs = new();
        private static int _nextId = 1;
        private static int _nextServiceNumber = 1;
        private static readonly object _lock = new();

        public IEnumerable<ServiceJob> GetAll()
        {
            lock (_lock)
            {
                return _jobs
                    .OrderByDescending(j => j.CheckInDateTime)
                    .ToList();
            }
        }

        public IEnumerable<ServiceJob> Search(string? searchTerm)
        {
            lock (_lock)
            {
                var query = _jobs.AsEnumerable();

                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    var term = searchTerm.Trim();
                    query = query.Where(j =>
                        j.ServiceNumber.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                        j.CustomerName.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                        j.PlateNumber.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                        j.VehicleMake.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                        j.VehicleModel.Contains(term, StringComparison.OrdinalIgnoreCase));
                }

                return query
                    .OrderByDescending(j => j.CheckInDateTime)
                    .ToList();
            }
        }

        public ServiceJob? GetById(int id)
        {
            lock (_lock)
            {
                return _jobs.FirstOrDefault(j => j.Id == id);
            }
        }

        public ServiceJob Add(ServiceJob job)
        {
            lock (_lock)
            {
                job.Id = _nextId++;
                _jobs.Add(job);
                return job;
            }
        }

        public bool Update(ServiceJob job)
        {
            lock (_lock)
            {
                var existing = _jobs.FirstOrDefault(j => j.Id == job.Id);
                if (existing is null)
                {
                    return false;
                }

                existing.CustomerName = job.CustomerName;
                existing.ContactNumber = job.ContactNumber;
                existing.VehicleMake = job.VehicleMake;
                existing.VehicleModel = job.VehicleModel;
                existing.ModelYear = job.ModelYear;
                existing.PlateNumber = job.PlateNumber;
                existing.VehicleColor = job.VehicleColor;
                existing.ServiceType = job.ServiceType;
                existing.ServiceBay = job.ServiceBay;
                existing.CheckInDateTime = job.CheckInDateTime;
                existing.ExpectedReleaseDate = job.ExpectedReleaseDate;
                existing.ActualReleaseDateTime = job.ActualReleaseDateTime;
                existing.Status = job.Status;
                existing.Remarks = job.Remarks;

                return true;
            }
        }

        public string GenerateNextServiceNumber()
        {
            lock (_lock)
            {
                var number = $"SVC-{_nextServiceNumber:D4}";
                _nextServiceNumber++;
                return number;
            }
        }
    }
}
