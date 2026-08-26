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
using Service.Models.SupplierModels;

namespace PharmacyManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _supplierService;
        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner")]
        [HttpGet("GetAllSuppliersPagedFiltered")]
        public async Task<IActionResult> GetAllSuppliersPagedFiltered([FromQuery] SupplierFiltering supplierFiltering, [FromQuery] PaginationParameters paginationParameters)
        {
            var suppliers = _supplierService.GetAllSuppliersFiltered(s =>
            //  فلترة ب SupplierStatus
            (string.IsNullOrEmpty(supplierFiltering.SupplierStatus) || s.SupplierStatus == supplierFiltering.SupplierStatus) 
            );

            suppliers = supplierFiltering.SortBy?.ToLower() switch
            {
                "subtotal" => supplierFiltering.isDescending
                ? suppliers.OrderByDescending(s => s.SupplierRating)
                : suppliers.OrderBy(s => s.SupplierRating),

                _ => suppliers
            };

            var totalSuppliers = suppliers.Count();

            suppliers = await _supplierService.GetAllSuppliersPagedAsync(paginationParameters.PageNumber, paginationParameters.PageSize);

            var response = new PaginationResponse<SupplierToReturnDTO>
                (
                    data: suppliers,
                    pageNumber: paginationParameters.PageNumber,
                    pageSize: paginationParameters.PageSize,
                    totalItems: totalSuppliers
                );

            return Ok(response);
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner")]
        [HttpGet("GetSupplierByID{id}")]
        public async Task<IActionResult> GetSupplierByID(int id)
        {
            var supplier = await _supplierService.GetSupplierByIDAsync(id);
            return Ok(supplier);
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner")]
        [HttpPost("AddNewSupplier")]
        public async Task<IActionResult> AddNewSupplierAsync([FromBody]AddNewSupplierModel addNewSupplier)
        {
            var addedSupplier = await _supplierService.AddNewSupplierAsync(addNewSupplier);
            return Ok(addedSupplier);
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner")]
        [HttpPut("UpdateSupplierInformation")]
        public async Task<IActionResult> UpdateSupplierInformationAsync(int supplierId,[FromBody]UpdateSupplierInformationModel updateSupplierInformation)
        {
            var updatedSupplier = await _supplierService.UpdateSupplierInformationAsync(supplierId , updateSupplierInformation);
            return Ok(updatedSupplier);
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner")]
        [HttpPut("UpdateSupplierStatus")]
        public async Task<IActionResult> UpdateSupplierStatusAsync(int supplierId, string supplierStatus)
        {
            var updatedSupplier = await _supplierService.UpdateSupplierStatusAsync(supplierId, supplierStatus);
            return Ok(updatedSupplier);
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner")]
        [HttpPut("UpdateSupplierRating")]
        public async Task<IActionResult> UpdateSupplierRatingAsync(int supplierId, int rating)
        {
            var updatedSupplier = await _supplierService.UpdateSupplierRatingAsync(supplierId, rating);
            return Ok(updatedSupplier);
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner")]
        [HttpDelete("DeleteSupplierByID{id}")]
        public async Task<IActionResult> DeleteSupplierByIDAsync(int supplierId)
        {
            var deletedSupplier = await _supplierService.DeleteSupplierByIDAsync(supplierId);
            return Ok(deletedSupplier);
        }
    }
}
