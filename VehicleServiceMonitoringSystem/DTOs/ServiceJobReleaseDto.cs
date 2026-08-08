using System.ComponentModel.DataAnnotations;

namespace VehicleServiceMonitoringSystem.DTOs
{
    public class ServiceJobReleaseDto
    {
        [Required]
        public int Id { get; set; }

        [Display(Name = "Service Number")]
        public string ServiceNumber { get; set; } = string.Empty;

        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; } = string.Empty;

        [Display(Name = "Vehicle")]
        public string Vehicle { get; set; } = string.Empty;

        [Display(Name = "Plate Number")]
        public string PlateNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Release date and time is required.")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Actual Release Date & Time")]
        public DateTime ActualReleaseDateTime { get; set; } = DateTime.Now;
        public string? Remarks { get; set; }
    }
}
