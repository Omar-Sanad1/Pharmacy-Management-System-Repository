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
using Service.Models.PurchaseOrderModels;

namespace PharmacyManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseOrderController : ControllerBase
    {
        private readonly IPurchaseOrderService _purchaseOrderService;
        public PurchaseOrderController(IPurchaseOrderService purchaseOrderService)
        {
            _purchaseOrderService = purchaseOrderService;
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner")]
        [HttpGet("GetAllPurchaseOrdersPagedFiltered")]
        public async Task<IActionResult> GetAllPurchaseOrdersPagedFiltered([FromQuery] PurchaseOrderFiltering purchaseOrderFiltering, [FromQuery] PaginationParameters paginationParameters)
        {
            var purchaseOrders = _purchaseOrderService.GetAllPurchaseOrdersFiltered(p =>
            //  فلترة ب ApprovalStatus
            (string.IsNullOrEmpty(purchaseOrderFiltering.ApprovalStatus) || p.ApprovalStatus == purchaseOrderFiltering.ApprovalStatus) &&
            //  فلترة ب OrderDate
            (!purchaseOrderFiltering.OrderDate.HasValue || p.OrderDate == purchaseOrderFiltering.OrderDate)
            );

            purchaseOrders = purchaseOrderFiltering.SortBy?.ToLower() switch
            {
                "orderdate" => purchaseOrderFiltering.isDescending
                ? purchaseOrders.OrderByDescending(p => p.OrderDate)
                : purchaseOrders.OrderBy(p => p.OrderDate),

                "expirationdate" => purchaseOrderFiltering.isDescending
                ? purchaseOrders.OrderByDescending(p => p.TotalAmount)
                : purchaseOrders.OrderBy(p => p.TotalAmount),

                _ => purchaseOrders
            };

            var totalPurchaseOrders = purchaseOrders.Count();

            purchaseOrders = await _purchaseOrderService.GetAllPurchaseOrdersPagedAsync(paginationParameters.PageNumber, paginationParameters.PageSize);

            var response = new PaginationResponse<PurchaseOrderToReturnDTO>
                (
                    data: purchaseOrders,
                    pageNumber: paginationParameters.PageNumber,
                    pageSize: paginationParameters.PageSize,
                    totalItems: totalPurchaseOrders
                );

            return Ok(response);
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner")]
        [HttpGet("GetPurchaseOrderByID{id}")]
        public async Task<IActionResult> GetPurchaseOrderByIDAsync(int id)
        {
            var purchaseOrder = await _purchaseOrderService.GetPurchaseOrderByIDAsync(id);
            return Ok(purchaseOrder);
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner")]
        [HttpGet("GetBranchPurchaseOrders{branchId}")]
        public async Task<IActionResult> GetBranchPurchaseOrdersAsync(int branchId)
        {
            var purchaseOrders = await _purchaseOrderService.GetBranchPurchaseOrdersAsync(branchId);
            return Ok(purchaseOrders);
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner")]
        [HttpGet("GetSupplierPurchaseOrders{supplierId}")]
        public async Task<IActionResult> GetSupplierPurchaseOrdersAsync(int supplierId)
        {
            var purchaseOrders = await _purchaseOrderService.GetSupplierPurchaseOrdersAsync(supplierId);
            return Ok(purchaseOrders);
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner")]
        [HttpPost("AddNewPurchaseOrder")]
        public async Task<IActionResult> AddNewPurchaseOrderAsync([FromBody]AddNewPurchaseOrderModel addNewPurchaseOrder)
        {
            var addedPurchaseOrder = await _purchaseOrderService.AddNewPurchaseOrderAsync(addNewPurchaseOrder);
            return Ok(addedPurchaseOrder);
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner")]
        [HttpPut("UpdatePurchaseOrderInformation")]
        public async Task<IActionResult> UpdatePurchaseOrderInformationAsync(int purchaseOrderId,[FromBody]UpdatePurchaseOrderInformationModel updatePurchaseOrderInformation)
        {
            var updatedPurchaseOrder = await _purchaseOrderService.UpdatePurchaseOrderInformationAsync(purchaseOrderId , updatePurchaseOrderInformation);
            return Ok(updatedPurchaseOrder);
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner")]
        [HttpPut("UpdatePurchaseOrderStatus")]
        public async Task<IActionResult> UpdatePurchaseOrderStatusAsync(int purchaseOrderId, string purchaseOrderStatus)
        {
            var updatedPurchaseOrder = await _purchaseOrderService.UpdatePurchaseOrderStatusAsync(purchaseOrderId, purchaseOrderStatus);
            return Ok(updatedPurchaseOrder);
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner")]
        [HttpPut("UpdateApprovalStatus")]
        public async Task<IActionResult> UpdateApprovalStatusAsync(int purchaseOrderId, string approvalStatus)
        {
            var updatedPurchaseOrder = await _purchaseOrderService.UpdateApprovalStatusAsync(purchaseOrderId, approvalStatus);
            return Ok(updatedPurchaseOrder);
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner")]
        [HttpPut("UpdatePaymentStatus")]
        public async Task<IActionResult> UpdatePaymentStatusAsync(int purchaseOrderId, string paymentStatus)
        {
            var updatedPurchaseOrder = await _purchaseOrderService.UpdatePaymentStatusAsync(purchaseOrderId, paymentStatus);
            return Ok(updatedPurchaseOrder);
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner")]
        [HttpDelete("DeletePurchaseOrderByID{id}")]
        public async Task<IActionResult> DeletePurchaseOrderByIDAsync(int purchaseOrderId)
        {
            var deletedPurchaseOrder = await _purchaseOrderService.DeletePurchaseOrderByIDAsync(purchaseOrderId);
            return Ok(deletedPurchaseOrder);
        }
    }
}
