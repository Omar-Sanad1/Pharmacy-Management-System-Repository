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
using System.Net.NetworkInformation;

namespace PharmacyManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [Authorize(Roles = "System Administrator , Branch Manager , Pharmacy Owner")]
        [HttpGet("GetAllCustomersPagedFiltered")]
        public async Task<IActionResult> GetAllCustomersPagedFiltered([FromQuery]CustomerFiltering customerFiltering ,[FromQuery]PaginationParameters paginationParameters)
        {
            var customers = _customerService.GetAllCustomersFiltered(c =>
            // فلترة ب Address
            (string.IsNullOrEmpty(customerFiltering.Address) || c.Address == customerFiltering.Address) &&
            // فلترة ب AccountStatus
            (string.IsNullOrEmpty(customerFiltering.AccountStatus) || c.AccountStatus == customerFiltering.AccountStatus) &&
            // فلترة ب RegistrationDate
            (!customerFiltering.RegistrationDate.HasValue || c.RegistrationDate == customerFiltering.RegistrationDate)
            );

            customers = customerFiltering.SortBy?.ToLower() switch
            {
                "registrationdate" => customerFiltering.isDescending
                ? customers.OrderByDescending(c=>c.RegistrationDate)
                : customers.OrderBy(c=>c.RegistrationDate),

                "dateofbirth" => customerFiltering.isDescending
                ? customers.OrderByDescending(c => c.DateOfBirth)
                : customers.OrderBy(c => c.DateOfBirth),

                _ => customers
            };

            var totalCustomers = customers.Count();

            customers = await _customerService.GetAllCustomersPagedAsync(paginationParameters.PageNumber, paginationParameters.PageSize);

            var result = new PaginationResponse<CustomerToReturnDTO>
                (
                    data: customers,
                    pageNumber: paginationParameters.PageNumber,
                    pageSize: paginationParameters.PageSize,
                    totalItems: totalCustomers
                );

            return Ok(result);
        }

        [Authorize(Roles = "System Administrator , Branch Manager , Pharmacy Owner")]
        [HttpGet("GetCustomerByID{id}")]
        public async Task<IActionResult> GetCustomerByIDAsync(int id)
        {
            var customer = await _customerService.GetCustomerByIDAsync(id);
            return Ok(customer);
        }

        [Authorize(Roles = "System Administrator , Branch Manager , Pharmacy Owner")]
        [HttpGet("GetCustomerByName{name}")]
        public async Task<IActionResult> GetCustomerByNameAsync(string name)
        {
            var customer = await _customerService.GetCustomerByNameAsync(name);
            return Ok(customer);
        }

        [Authorize(Roles = "System Administrator")]
        [HttpPut("UpdateCustomerStatus")]
        public async Task<IActionResult> UpdateCustomerStatus(int customerId , string status)
        {
            var updatedCustomer = await _customerService.UpdateCustomerStatusAsync(customerId, status);
            return Ok(updatedCustomer);
        }

        [Authorize(Roles = "System Administrator")]
        [HttpDelete("DeleteCustomerbyID{id}")]
        public async Task<IActionResult> DeleteCustomerByIDAsync(int customerId)
        {
            var deletedCustomer = await _customerService.DeleteCustomerAsync(customerId);
            return Ok(deletedCustomer);
        }
    }
}
