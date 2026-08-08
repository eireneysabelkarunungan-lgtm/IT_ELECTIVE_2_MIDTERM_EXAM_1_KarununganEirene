using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleServiceMonitoringSystem.DTOs;
using VehicleServiceMonitoringSystem.Models;
using VehicleServiceMonitoringSystem.Repositories;

namespace VehicleServiceMonitoringSystem.Controllers
{
    [Authorize]
    public class ServiceJobController : Controller
    {
        private readonly IServiceJobRepository _serviceJobRepository;

        public ServiceJobController(IServiceJobRepository serviceJobRepository)
        {
            _serviceJobRepository = serviceJobRepository;
        }


        [HttpGet]
        public IActionResult Index(string? searchTerm)
        {
            var jobs = _serviceJobRepository.Search(searchTerm)
                .Select(ToListItemDto)
                .ToList();

            ViewData["SearchTerm"] = searchTerm;

            //return View();
            return View(jobs);

            
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            var job = _serviceJobRepository.GetById(id);
            if (job is null)
            {
                return NotFound();
            }

            return View(ToDetailsDto(job));
        }





        [HttpGet]
        public IActionResult Create()
        {
            var dto = new ServiceJobCreateDto
            {
                CheckInDateTime = DateTime.Now,
                ExpectedReleaseDate = DateTime.Now.AddHours(2)
            };
            return View(dto);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ServiceJobCreateDto dto)
        {
            if (dto.ExpectedReleaseDate <= dto.CheckInDateTime)
            {
                ModelState.AddModelError(
                    nameof(dto.ExpectedReleaseDate),
                    "Expected release date must be after the check-in date and time.");
            }

            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var job = new ServiceJob
            {
                ServiceNumber = _serviceJobRepository.GenerateNextServiceNumber(),
                CustomerName = dto.CustomerName.Trim(),
                ContactNumber = dto.ContactNumber.Trim(),
                VehicleMake = dto.VehicleMake.Trim(),
                VehicleModel = dto.VehicleModel.Trim(),
                ModelYear = dto.ModelYear,
                PlateNumber = dto.PlateNumber.Trim().ToUpperInvariant(),
                VehicleColor = dto.VehicleColor.Trim(),
                ServiceType = dto.ServiceType.Trim(),
                ServiceBay = dto.ServiceBay.Trim(),
                CheckInDateTime = dto.CheckInDateTime,
                ExpectedReleaseDate = dto.ExpectedReleaseDate,
                Status = ServiceStatus.Waiting,
                Remarks = dto.Remarks?.Trim()
            };

            _serviceJobRepository.Add(job);

            TempData["SuccessMessage"] = $"Vehicle registered successfully under service number {job.ServiceNumber}.";
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            var job = _serviceJobRepository.GetById(id);
            if (job is null)
            {
                return NotFound();
            }

            return View(ToEditDto(job));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, ServiceJobEditDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest();
            }

            var existing = _serviceJobRepository.GetById(id);
            if (existing is null)
            {
                return NotFound();
            }

            if (dto.ExpectedReleaseDate <= dto.CheckInDateTime)
            {
                ModelState.AddModelError(
                    nameof(dto.ExpectedReleaseDate),
                    "Expected release date must be after the check-in date and time.");
            }

            if (!ModelState.IsValid)
            {
                dto.ServiceNumber = existing.ServiceNumber;
                return View(dto);
            }

            var updatedJob = new ServiceJob
            {
                Id = dto.Id,
                ServiceNumber = existing.ServiceNumber,
                CustomerName = dto.CustomerName.Trim(),
                ContactNumber = dto.ContactNumber.Trim(),
                VehicleMake = dto.VehicleMake.Trim(),
                VehicleModel = dto.VehicleModel.Trim(),
                ModelYear = dto.ModelYear,
                PlateNumber = dto.PlateNumber.Trim().ToUpperInvariant(),
                VehicleColor = dto.VehicleColor.Trim(),
                ServiceType = dto.ServiceType.Trim(),
                ServiceBay = dto.ServiceBay.Trim(),
                CheckInDateTime = dto.CheckInDateTime,
                ExpectedReleaseDate = dto.ExpectedReleaseDate,
                ActualReleaseDateTime = existing.ActualReleaseDateTime,
                Status = dto.Status,
                Remarks = dto.Remarks?.Trim()
            };

            _serviceJobRepository.Update(updatedJob);

            TempData["SuccessMessage"] = $"Service job {updatedJob.ServiceNumber} updated successfully.";
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public IActionResult Release(int id)
        {
            var job = _serviceJobRepository.GetById(id);
            if (job is null)
            {
                return NotFound();
            }

            if (job.Status == ServiceStatus.Released)
            {
                TempData["ErrorMessage"] = $"Service job {job.ServiceNumber} has already been released.";
                return RedirectToAction(nameof(Details), new { id });
            }

            var dto = new ServiceJobReleaseDto
            {
                Id = job.Id,
                ServiceNumber = job.ServiceNumber,
                CustomerName = job.CustomerName,
                Vehicle = $"{job.ModelYear} {job.VehicleMake} {job.VehicleModel}",
                PlateNumber = job.PlateNumber,
                ActualReleaseDateTime = DateTime.Now,
                Remarks = job.Remarks
            };

            return View(dto);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Release(int id, ServiceJobReleaseDto dto)
        {
            if (id != dto.Id)
            {
                return BadRequest();
            }

            var existing = _serviceJobRepository.GetById(id);
            if (existing is null)
            {
                return NotFound();
            }

            if (dto.ActualReleaseDateTime < existing.CheckInDateTime)
            {
                ModelState.AddModelError(
                    nameof(dto.ActualReleaseDateTime),
                    "Release date and time cannot be earlier than the check-in date and time.");
            }

            if (!ModelState.IsValid)
            {
                dto.ServiceNumber = existing.ServiceNumber;
                dto.CustomerName = existing.CustomerName;
                dto.Vehicle = $"{existing.ModelYear} {existing.VehicleMake} {existing.VehicleModel}";
                dto.PlateNumber = existing.PlateNumber;
                return View(dto);
            }

            existing.ActualReleaseDateTime = dto.ActualReleaseDateTime;
            existing.Status = ServiceStatus.Released;
            existing.Remarks = dto.Remarks?.Trim();

            _serviceJobRepository.Update(existing);

            TempData["SuccessMessage"] = $"Vehicle for service number {existing.ServiceNumber} has been released.";
            return RedirectToAction(nameof(Index));
        }

        private static ServiceJobListItemDto ToListItemDto(ServiceJob job) => new()
        {
            Id = job.Id,
            ServiceNumber = job.ServiceNumber,
            CustomerName = job.CustomerName,
            Vehicle = $"{job.ModelYear} {job.VehicleMake} {job.VehicleModel}",
            PlateNumber = job.PlateNumber,
            ServiceType = job.ServiceType,
            ServiceBay = job.ServiceBay,
            CheckInDateTime = job.CheckInDateTime,
            ActualReleaseDateTime = job.ActualReleaseDateTime,
            Status = job.Status
        };

        private static ServiceJobDetailsDto ToDetailsDto(ServiceJob job) => new()
        {
            Id = job.Id,
            ServiceNumber = job.ServiceNumber,
            CustomerName = job.CustomerName,
            ContactNumber = job.ContactNumber,
            VehicleMake = job.VehicleMake,
            VehicleModel = job.VehicleModel,
            ModelYear = job.ModelYear,
            PlateNumber = job.PlateNumber,
            VehicleColor = job.VehicleColor,
            ServiceType = job.ServiceType,
            ServiceBay = job.ServiceBay,
            CheckInDateTime = job.CheckInDateTime,
            ExpectedReleaseDate = job.ExpectedReleaseDate,
            ActualReleaseDateTime = job.ActualReleaseDateTime,
            Status = job.Status,
            Remarks = job.Remarks
        };

        private static ServiceJobEditDto ToEditDto(ServiceJob job) => new()
        {
            Id = job.Id,
            ServiceNumber = job.ServiceNumber,
            CustomerName = job.CustomerName,
            ContactNumber = job.ContactNumber,
            VehicleMake = job.VehicleMake,
            VehicleModel = job.VehicleModel,
            ModelYear = job.ModelYear,
            PlateNumber = job.PlateNumber,
            VehicleColor = job.VehicleColor,
            ServiceType = job.ServiceType,
            ServiceBay = job.ServiceBay,
            CheckInDateTime = job.CheckInDateTime,
            ExpectedReleaseDate = job.ExpectedReleaseDate,
            Status = job.Status,
            Remarks = job.Remarks
        };
    }
}
