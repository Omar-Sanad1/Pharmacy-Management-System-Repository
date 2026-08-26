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
using Service.Models.PrescriptionModels;
using System.Net.NetworkInformation;

namespace PharmacyManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionController : ControllerBase
    {
        private readonly IPrescriptionService _prescriptionService;
        public PrescriptionController(IPrescriptionService prescriptionService)
        {
           _prescriptionService = prescriptionService;
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner , Pharmacist , Branch Manager")]
        [HttpGet("GetAllPrescriptionsPagedFiltered")]
        public async Task<IActionResult> GetAllPrescriptionsPagedFiltered([FromQuery] PrescriptionFiltering prescriptionFiltering, [FromQuery] PaginationParameters paginationParameters)
        {
            var prescriptions = _prescriptionService.GetAllPrescriptionsFiltered(p =>
            //  فلترة ب PrescriptionStatus
            (string.IsNullOrEmpty(prescriptionFiltering.PrescriptionStatus) || p.PrescriptionStatus == prescriptionFiltering.PrescriptionStatus)
            
            );

            prescriptions = prescriptionFiltering.SortBy?.ToLower() switch
            {
                "prescriptiondate" => prescriptionFiltering.isDescending
                ? prescriptions.OrderByDescending(p => p.PrescriptionDate)
                : prescriptions.OrderBy(p => p.PrescriptionDate),

                "expirationdate" => prescriptionFiltering.isDescending
                ? prescriptions.OrderByDescending(p => p.ExpirationDate)
                : prescriptions.OrderBy(p => p.ExpirationDate),

                _ => prescriptions
            };

            var totalPrescriptions = prescriptions.Count();

            prescriptions = await _prescriptionService.GetAllPrescriptionsPagedAsync(paginationParameters.PageNumber, paginationParameters.PageSize);

            var response = new PaginationResponse<PrescriptionToReturnDTO>
                (
                    data: prescriptions,
                    pageNumber: paginationParameters.PageNumber,
                    pageSize: paginationParameters.PageSize,
                    totalItems: totalPrescriptions
                );

            return Ok(response);
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner , Pharmacist , Branch Manager")]
        [HttpGet("GetPrescriptionByID{id}")]
        public async Task<IActionResult> GetPrescriptionByID(int id)
        {
            var prescription = await _prescriptionService.GetPrescriptionByIDAsync(id);
            return Ok(prescription);
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner , Pharmacist , Branch Manager")]
        [HttpGet("GetAllCustomerPrescriptions{customerId}")]
        public async Task<IActionResult> GetAllCustomerPrescriptionsAsync(int customerId)
        {
            var prescriptions = await _prescriptionService.GetAllCustomerPrescriptionsAsync(customerId);
            return Ok(prescriptions);
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner , Pharmacist , Branch Manager")]
        [HttpGet("GetAllDoctorPrescriptions{doctorId}")]
        public async Task<IActionResult> GetAllDoctorPrescriptionsAsync(int doctorId)
        {
            var prescriptions = await _prescriptionService.GetAllDoctorPrescriptionsAsync(doctorId);
            return Ok(prescriptions);
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner , Pharmacist")]
        [HttpPost("AddNewPrescription")]
        public async Task<IActionResult> AddNewPrescriptionAsync([FromBody]AddNewPrescriptionModel addNewPrescription)
        {
            var addedPrescription = await _prescriptionService.AddNewPrescriptionAsync(addNewPrescription);
            return Ok(addedPrescription);
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner , Pharmacist")]
        [HttpPut("UpdatePrescriptionInformation")]
        public async Task<IActionResult> UpdatePrescriptionInformationAsync(int prescriptionId,[FromBody]UpdatePrescriptionInformationModel updatePrescriptionInformation)
        {
            var updatedPrescription = await _prescriptionService.UpdatePrescriptionInformationAsync(prescriptionId,updatePrescriptionInformation);
            return Ok(updatedPrescription);
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner , Pharmacist")]
        [HttpPut("UpdatePrescriptionStatus")]
        public async Task<IActionResult> UpdatePrescriptionStatusAsync(int prescriptionId, string status)
        {
            var updatedPrescription = await _prescriptionService.UpdatePrescriptionStatusAsync(prescriptionId, status);
            return Ok(updatedPrescription);
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner")]
        [HttpDelete("DeletePrescriptionByID{id}")]
        public async Task<IActionResult> DeletePrescriptionByIDAsync(int prescriptionId)
        {
            var deletedPrescription = await _prescriptionService.DeletePrescriptionByIDAsync(prescriptionId);
            return Ok(deletedPrescription);
        }
    }
}
