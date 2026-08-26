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
using Service.Models.MedicineModels;

namespace PharmacyManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicineController : ControllerBase
    {
        private readonly IMedicineService _medicineService;
        public MedicineController(IMedicineService medicineService)
        {
            _medicineService = medicineService;
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner , Pharmacist , Branch Manager")]
        [HttpGet("GetAllMedicinesMappingPagedFiltered")]
        public async Task<IActionResult> GetAllMedicinesMappingPagedFiltered([FromQuery] MedicineFiltering medicineFiltering, [FromQuery] PaginationParameters paginationParameters)
        {
            var medicines = _medicineService.GetAllMedicinesFiltered(m =>
            //  فلترة ب Category
            (string.IsNullOrEmpty(medicineFiltering.Category) || m.Category == medicineFiltering.Category) &&
            //  فلترة ب AvailabilityStatus
            (string.IsNullOrEmpty(medicineFiltering.AvailabilityStatus) || m.AvailabilityStatus == medicineFiltering.AvailabilityStatus) &&
            //  فلترة ب PurchasePrice
            (!medicineFiltering.PurchasePrice.HasValue || m.PurchasePrice == medicineFiltering.PurchasePrice)
            );

            medicines = medicineFiltering.SortBy?.ToLower() switch
            {
                "purchaseprice" => medicineFiltering.isDescending
                ? medicines.OrderByDescending(m => m.PurchasePrice)
                : medicines.OrderBy(m => m.PurchasePrice),

                "minimumstocklevel" => medicineFiltering.isDescending
                ? medicines.OrderByDescending(m => m.MinimumStockLevel)
                : medicines.OrderBy(m => m.MinimumStockLevel),

                "strength" => medicineFiltering.isDescending
                ? medicines.OrderByDescending(m => m.Strength)
                : medicines.OrderBy(m => m.Strength),

                _ => medicines
            };

            var totalMedicines = medicines.Count();

            medicines = await _medicineService.GetAllMedicinesPagedAsync(paginationParameters.PageNumber, paginationParameters.PageSize);

            var response = new PaginationResponse<MedicineToReturnDTO>
                (
                    data: medicines,
                    pageNumber: paginationParameters.PageNumber,
                    pageSize: paginationParameters.PageSize,
                    totalItems: totalMedicines
                );

            return Ok(response);
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner , Pharmacist , Branch Manager")]
        [HttpGet("GetMedicineByID{id}")]
        public async Task<IActionResult> GetMedicineByIDAsync(int id)
        {
            var medicine = await _medicineService.GetMedicineByIDAsync(id);
            return Ok(medicine);
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner , Pharmacist , Branch Manager")]
        [HttpGet("GetMedicineByName{name}")]
        public async Task<IActionResult> GetMedicineByNameAsync(string name)
        {
            var medicine = await _medicineService.GetMedicineByNameAsync(name);
            return Ok(medicine);
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner , Pharmacist")]
        [HttpPost("AddNewMedicine")]
        public async Task<IActionResult> AddNewMedicineAsync([FromBody]AddNewMedicineModel addNewMedicine)
        {
            var addedMedicine = await _medicineService.AddNewMedicineAsync(addNewMedicine);
            return Ok(addedMedicine);
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner , Pharmacist")]
        [HttpPut("UpdateMedicineInformation")]
        public async Task<IActionResult> UpdateMedicineInformationAsync(int medicineId,[FromBody]UpdateMedicineInformationModel updateMedicineInformation)
        {
            var updatedMedicine = await _medicineService.UpdateMedicineInformationAsync(medicineId,updateMedicineInformation);
            return Ok(updatedMedicine);
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner , Pharmacist")]
        [HttpPut("UpdateMedicineStatus")]
        public async Task<IActionResult> UpdateMedicineStatusAsync(int medicineId, string status)
        {
            var updatedMedicine = await _medicineService.UpdateMedicineStatusAsync(medicineId, status);
            return Ok(updatedMedicine);
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner , Pharmacist")]
        [HttpPut("UpdateMedicineSellingPrice")]
        public async Task<IActionResult> UpdateMedicineSellingPriceAsync(int medicineId, decimal sellingPrice)
        {
            var updatedMedicine = await _medicineService.UpdateMedicineSellingPriceAsync(medicineId, sellingPrice);
            return Ok(updatedMedicine);
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner , Pharmacist")]
        [HttpPut("UpdateMedicinePurchasePrice")]
        public async Task<IActionResult> UpdateMedicinePurchasePriceAsync(int medicineId, decimal purchasePrice)
        {
            var updatedMedicine = await _medicineService.UpdateMedicinePurchasePriceAsync(medicineId, purchasePrice);
            return Ok(updatedMedicine);
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner , Pharmacist")]
        [HttpPut("UpdateMedicineMinimumStockLevel")]
        public async Task<IActionResult> UpdateMedicineMinimumStockLevelAsync(int medicineId, int MinimumStockLevel)
        {
            var updatedMedicine = await _medicineService.UpdateMedicinePurchasePriceAsync(medicineId, MinimumStockLevel);
            return Ok(updatedMedicine);
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner")]
        [HttpDelete("DeleteMedicineByID{id}")]
        public async Task<IActionResult> DeleteMedicine(int medicineId)
        {
            var deletedMedicine = await _medicineService.DeleteMedicineByIDAsync(medicineId);
            return Ok(deletedMedicine);
        }
    }
}
