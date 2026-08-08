using System.ComponentModel.DataAnnotations;

namespace VehicleServiceMonitoringSystem.Models
{
    public enum ServiceStatus
    {
        [Display(Name = "Waiting")]
        Waiting = 0,

        [Display(Name = "In Service")]
        InService = 1,

        [Display(Name = "Ready for Release")]
        ReadyForRelease = 2,

        [Display(Name = "Released")]
        Released = 3
    }
}
