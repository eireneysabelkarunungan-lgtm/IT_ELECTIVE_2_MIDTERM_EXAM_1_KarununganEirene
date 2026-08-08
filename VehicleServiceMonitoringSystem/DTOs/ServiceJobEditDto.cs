using System.ComponentModel.DataAnnotations;
using VehicleServiceMonitoringSystem.Models;

namespace VehicleServiceMonitoringSystem.DTOs
{
    public class ServiceJobEditDto
    {
        [Required]
        public int Id { get; set; }

        [Display(Name = "Service Number")]
        public string ServiceNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "This field is required.")]
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; } = string.Empty;

        [Required(ErrorMessage = "This field is required.")]
        [Display(Name = "Contact Number")]
        public string ContactNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "This field is required.")]
        [Display(Name = "Vehicle Make")]
        public string VehicleMake { get; set; } = string.Empty;

        [Required(ErrorMessage = "This field is required.")]
        [Display(Name = "Vehicle Model")]
        public string VehicleModel { get; set; } = string.Empty;

        [Required(ErrorMessage = "This field is required.")]
        [Display(Name = "Model Year")]
        public int ModelYear { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [Display(Name = "Plate Number")]
        public string PlateNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "This field is required.")]
        [Display(Name = "Vehicle Color")]
        public string VehicleColor { get; set; } = string.Empty;

        [Required(ErrorMessage = "This field is required.")]
        [Display(Name = "Service Type")]
        public string ServiceType { get; set; } = string.Empty;

        [Required(ErrorMessage = "This field is required.")]
        [Display(Name = "Service Bay")]
        public string ServiceBay { get; set; } = string.Empty;

        [Required(ErrorMessage = "This field is required.")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Check-in Date & Time")]
        public DateTime CheckInDateTime { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Expected Release Date")]
        public DateTime ExpectedReleaseDate { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [Display(Name = "Status")]
        public ServiceStatus Status { get; set; }

        public string? Remarks { get; set; }
    }
}
