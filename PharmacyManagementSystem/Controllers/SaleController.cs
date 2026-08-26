using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Filtering;
using Core.Interfaces;
using Core.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Context;
using Service.Interfaces;
using Service.Models.SaleModels;

namespace PharmacyManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleController : ControllerBase
    {
        private readonly ISaleService _saleService;
        public SaleController(ISaleService saleService)
        {
            _saleService = saleService;
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner")]
        [HttpGet("GetAllSalesPagedFiltered")]
        public async Task<IActionResult> GetAllSalesPagedFiltered([FromQuery] SaleFiltering saleFiltering, [FromQuery] PaginationParameters paginationParameters)
        {
            var sales = _saleService.GetAllSalesFiltered(s =>
            //  فلترة ب Subtotal
            (!saleFiltering.Subtotal.HasValue || s.Subtotal == saleFiltering.Subtotal) &&
            //  فلترة ب SaleDate
            (!saleFiltering.SaleDate.HasValue || s.SaleDate == saleFiltering.SaleDate)
            );

            sales = saleFiltering.SortBy?.ToLower() switch
            {
                "subtotal" => saleFiltering.isDescending
                ? sales.OrderByDescending(s => s.Subtotal)
                : sales.OrderBy(s => s.Subtotal),

                "discount" => saleFiltering.isDescending
                ? sales.OrderByDescending(s => s.Discount)
                : sales.OrderBy(s => s.Discount),

                "saledate" => saleFiltering.isDescending
                ? sales.OrderByDescending(s => s.SaleDate)
                : sales.OrderBy(s => s.SaleDate),

                _ => sales
            };

            var totalSales = sales.Count();

            sales = await _saleService.GetAllSalesPagedAsync(paginationParameters.PageNumber, paginationParameters.PageSize);

            var response = new PaginationResponse<SaleToReturnDTO>
                (
                    data: sales,
                    pageNumber: paginationParameters.PageNumber,
                    pageSize: paginationParameters.PageSize,
                    totalItems: totalSales
                );

            return Ok(response);
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner")]
        [HttpGet("GetAllBranchSales")]
        public async Task<IActionResult> GetAllBranchSalesAsync(int branchId)
        {
            var branchSales = await _saleService.GetAllBranchSalesAsync(branchId);
            return Ok(branchSales);
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner")]
        [HttpGet("GetAllCustomerSales")]
        public async Task<IActionResult> GetAllCustomerSalesAsync(int customerId)
        {
            var customerSales = await _saleService.GetAllCustomerSalesAsync(customerId);
            return Ok(customerSales);
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner")]
        [HttpGet("GetAllEmployeeSales")]
        public async Task<IActionResult> GetAllEmployeeSalesAsync(int employeeId)
        {
            var employeeSales = await _saleService.GetAllEmployeeSalesAsync(employeeId);
            return Ok(employeeSales);
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner")]
        [HttpGet("GetSaleByID{id}")]
        public async Task<IActionResult> GetSaleByID(int id)
        {
            var sale = await _saleService.GetSaleByIDAsync(id);
            return Ok(sale);
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner")]
        [HttpPost("AddNewSale")]
        public async Task<IActionResult> AddNewSaleAsync([FromBody]AddNewSaleModel addNewSale)
        {
            var addedSale = await _saleService.AddNewSaleAsync(addNewSale);
            return Ok(addedSale);
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner")]
        [HttpPut("UpdateSaleInformation")]
        public async Task<IActionResult> UpdateSaleInformationAsync(int saleId,[FromBody]UpdateSaleInformationModel updateSaleInformation)
        {
            var updatedSale = await _saleService.UpdateSaleInformationAsync(saleId,updateSaleInformation);
            return Ok(updatedSale);
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner")]
        [HttpPut("UpdateSaleStatus")]
        public async Task<IActionResult> UpdateSaleStatusAsync(int saleId, string saleStatus)
        {
            var updatedSale = await _saleService.UpdateSaleStatusAsync(saleId, saleStatus);
            return Ok(updatedSale);
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner")]
        [HttpPut("UpdateSalePaymentStatus")]
        public async Task<IActionResult> UpdateSalePaymentStatusAsync(int saleId, string paymentStatus)
        {
            var updatedSale = await _saleService.UpdateSalePaymentStatusAsync(saleId, paymentStatus);
            return Ok(updatedSale);
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner")]
        [HttpPut("UpdateSaleSubtotal")]
        public async Task<IActionResult> UpdateSaleSubtotalAsync(int saleId, decimal subTotal)
        {
            var updatedSale = await _saleService.UpdateSaleSubtotalAsync(saleId, subTotal);
            return Ok(updatedSale);
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner")]
        [HttpPut("UpdateSaleDiscount")]
        public async Task<IActionResult> UpdateSaleDiscountAsync(int saleId, decimal discount)
        {
            var updatedSale = await _saleService.UpdateSaleDiscountAsync(saleId, discount);
            return Ok(updatedSale);
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner")]
        [HttpPut("UpdateSaleTaxes")]
        public async Task<IActionResult> UpdateSaleTaxesAsync(int saleId, decimal taxes)
        {
            var updatedSale = await _saleService.UpdateSaleTaxesAsync(saleId, taxes);
            return Ok(updatedSale);
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner")]
        [HttpPut("UpdateSaleTotalAmount")]
        public async Task<IActionResult> UpdateSaleTotalAmountAsync(int saleId, decimal totalAmount)
        {
            var updatedSale = await _saleService.UpdateSaleTotalAmountAsync(saleId, totalAmount);
            return Ok(updatedSale);
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner")]
        [HttpDelete("DeleteSaleByID{id}")]
        public async Task<IActionResult> DeleteSaleByIDAsync(int saleId)
        {
            var deletedSale = await _saleService.DeleteSaleByIDAsync(saleId);
            return Ok(deletedSale);
        }
    }
}
