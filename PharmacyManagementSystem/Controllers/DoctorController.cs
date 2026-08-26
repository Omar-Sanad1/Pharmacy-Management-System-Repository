using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Filtering;
using Core.Interfaces;
using Core.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;
using Service.Models.DoctorModels;
using System.Net.NetworkInformation;

namespace PharmacyManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;
        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [Authorize(Roles = "System Administrator , Branch Manager , Pharmacy Owner , Pharmacist")]
        [HttpGet("GetAllDoctorsPagedFiltered")]
        public async Task<IActionResult> GetAllCustomersPagedFiltered([FromQuery] DoctorFiltering doctorFiltering, [FromQuery] PaginationParameters paginationParameters)
        {
            var doctors = _doctorService.GetAllDoctorsFiltered(d =>
            // فلترة ب Specialization
            (string.IsNullOrEmpty(doctorFiltering.Specialization) || d.Specialization == doctorFiltering.Specialization) &&
            // فلترة ب Status
            (string.IsNullOrEmpty(doctorFiltering.Status) || d.Status == doctorFiltering.Status)
            );

            doctors = doctorFiltering.SortBy?.ToLower() switch
            {
                "licensenumber" => doctorFiltering.isDescending
                ? doctors.OrderByDescending(d => d.LicenseNumber)
                : doctors.OrderBy(d => d.LicenseNumber),

                "status" => doctorFiltering.isDescending
                ? doctors.OrderByDescending(d => d.Status)
                : doctors.OrderBy(d => d.Status),

                _ => doctors
            };

            var totaldoctors = doctors.Count();

            doctors = await _doctorService.GetAllDoctorsPagedAsync(paginationParameters.PageNumber, paginationParameters.PageSize);

            var result = new PaginationResponse<DoctorToReturnDTO>
                (
                    data: doctors,
                    pageNumber: paginationParameters.PageNumber,
                    pageSize: paginationParameters.PageSize,
                    totalItems: totaldoctors
                );

            return Ok(result);
        }

        [Authorize(Roles = "System Administrator , Branch Manager , Pharmacy Owner , Pharmacist")]
        [HttpGet("GetDoctorByID{id}")]
        public async Task<IActionResult> GetDoctorByIDAsync(int id)
        {
            var doctor = await _doctorService.GetDoctorByIDAsync(id);
            return Ok(doctor);
        }

        [Authorize(Roles = "System Administrator , Branch Manager , Pharmacy Owner , Pharmacist")]
        [HttpGet("GetDoctorByName{name}")]
        public async Task<IActionResult> GetDoctorByNameAsync(string name)
        {
            var doctor = await _doctorService.GetDoctorByNameAsync(name);
            return Ok(doctor);
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner")]
        [HttpPost("AddNewDoctor")]
        public async Task<IActionResult> AddNewDoctorAsync([FromBody]AddNewDoctorModel addNewDoctor)
        {
            var addedDoctor = await _doctorService.AddNewDoctorAsync(addNewDoctor);
            return Ok(addedDoctor);
        }

        [Authorize(Roles = "System Administrator, Pharmacy Owner")]
        [HttpPut("UpdateDoctorInformationByID{id}")]
        public async Task<IActionResult> UpdateDoctorInformationAsync(int doctorId,[FromBody]UpdateDoctorInformationModel updateDoctorInformation)
        {
            var updatedDoctor = await _doctorService.UpdateDoctorInformationAsync(doctorId,updateDoctorInformation);
            return Ok(updatedDoctor);
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner")]
        [HttpPut("UpdateDoctorStatusByID{id}")]
        public async Task<IActionResult> UpdateDoctorStatusAsync(int doctorId, string status)
        {
            var updatedDoctor = await _doctorService.UpdateDoctorStatusAsync(doctorId, status);
            return Ok(updatedDoctor);
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner")]
        [HttpDelete("DeleteDoctorByID{id}")]
        public async Task<IActionResult> DeleteDoctorByIDAsync(int doctorId)
        {
            var deletedDoctor = await _doctorService.DeleteDoctorAsync(doctorId);
            return Ok(deletedDoctor);
        }
    }
}
