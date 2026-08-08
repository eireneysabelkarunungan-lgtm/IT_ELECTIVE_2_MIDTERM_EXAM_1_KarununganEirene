using VehicleServiceMonitoringSystem.Models;

namespace VehicleServiceMonitoringSystem.Helpers
{
    public static class StatusBadgeHelper
    {
        public static string CssClass(ServiceStatus status) => status switch
        {
            ServiceStatus.Waiting => "badge badge-status-waiting",
            ServiceStatus.InService => "badge badge-status-inservice",
            ServiceStatus.ReadyForRelease => "badge badge-status-ready",
            ServiceStatus.Released => "badge badge-status-released",
            _ => "badge bg-secondary"
        };

        public static string DisplayName(ServiceStatus status) => status switch
        {
            ServiceStatus.Waiting => "Waiting",
            ServiceStatus.InService => "In Service",
            ServiceStatus.ReadyForRelease => "Ready for Release",
            ServiceStatus.Released => "Released",
            _ => status.ToString()
        };
    }
}
