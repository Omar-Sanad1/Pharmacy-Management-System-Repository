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
using Service.Models.BatchModels;

namespace PharmacyManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BatchController : ControllerBase
    {
        private readonly IBatchService _batchService;
        public BatchController(IBatchService batchService)
        {
            _batchService = batchService;
        }

        [Authorize(Roles = "System Administrator , Pharmacist , Pharmacy Owner")]
        [HttpGet("GetAllBatchesPagedFiltered")]
        public async Task<IActionResult> GetAllBatchesPagedFiltered([FromQuery]BatchFiltering batchFiltering , [FromQuery]PaginationParameters paginationParameters)
        {
            var batches = _batchService.GetAllBatchesFiltered(b => 
            //  فلترة ب PurchasePrice
            (!batchFiltering.PurchasePrice.HasValue || b.PurchasePrice == batchFiltering.PurchasePrice) &&
            //  فلترة ب AvailableQuantity
            (!batchFiltering.AvailableQuantity.HasValue || b.AvailableQuantity == batchFiltering.AvailableQuantity)
            );

            batches = batchFiltering.SortBy?.ToLower() switch
            {
                "purchaseprice" => batchFiltering.isDescending
                ? batches.OrderByDescending(b => b.PurchasePrice)
                : batches.OrderBy(b => b.PurchasePrice),

                "availablequantity" => batchFiltering.isDescending
                ? batches.OrderByDescending(b => b.AvailableQuantity)
                : batches.OrderBy(b => b.AvailableQuantity),

                "expirationdate" => batchFiltering.isDescending
                ? batches.OrderByDescending(b => b.ExpirationDate)
                : batches.OrderBy(b => b.ExpirationDate),

                _ => batches
            };

            var totalBatches = batches.Count();

            batches = await _batchService.GetAllBatchesPagedAsync(paginationParameters.PageNumber, paginationParameters.PageSize);

            var response = new PaginationResponse<BatchToReturnDTO>
                (
                    data:batches,
                    pageNumber:paginationParameters.PageNumber,
                    pageSize:paginationParameters.PageSize,
                    totalItems:totalBatches
                );

            return Ok(response);
        }

        [Authorize(Roles = "System Administrator , Pharmacist")]
        [HttpGet("GetBatchByID{id}")]
        public async Task<IActionResult> GetBatchByIDAsync(int id)
        {
            var batch = await _batchService.GetBatchByIDAsync(id);
            return Ok(batch);
        }

        [Authorize(Roles = "System Administrator , Pharmacist")]
        [HttpGet("GetBatchByBatchNumber{number}")]
        public async Task<IActionResult> GetBatchByBatchNumberAsync(string batchNumber)
        {
            var batch = await _batchService.GetBatchByBatchNumberAsync(batchNumber);
            return Ok(batch);
        }

        [Authorize(Roles = "System Administrator")]
        [HttpPost("AddNewBatch")]
        public async Task<IActionResult> AddNewBatchAsync([FromBody]AddNewBatchModel addNewBatch)
        {
            var addedBatch = await _batchService.AddNewBatchAsync(addNewBatch);
            return Ok(addedBatch);
        }

        [Authorize(Roles = "System Administrator")]
        [HttpPut("UpdateBatchInformation")]
        public async Task<IActionResult> UpdateBatchInformationAsync(int batchId,[FromBody]UpdateBatchInformationModel updateBatchInformation)
        {
            var updatedBatch = await _batchService.UpdateBatchInformationAsync(batchId,updateBatchInformation);
            return Ok(updatedBatch);
        }

        [Authorize(Roles = "System Administrator")]
        [HttpDelete("DeleteBatch")]
        public async Task<IActionResult> DeleteBatchAsync(int batchId)
        {
            var deletedBatch = await _batchService.DeleteBatchAsync(batchId);
            return Ok(deletedBatch);
        }
    }
}
