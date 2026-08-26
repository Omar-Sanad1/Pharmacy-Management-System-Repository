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
using Service.Models.EmployeeModels;

namespace PharmacyManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner , Branch Manager")]
        [HttpGet("GetAllEmployeesPagedFiltered")]
        public async Task<IActionResult> GetAllEmployeesPagedFiltered([FromQuery]EmployeeFiltering employeeFiltering, [FromQuery] PaginationParameters paginationParameters)
        {
            var employees = _employeeService.GetAllEmployeesFiltered(d =>
            // فلترة ب Shift
            (string.IsNullOrEmpty(employeeFiltering.Shift) || d.Shift == employeeFiltering.Shift) &&
            // فلترة ب EmploymentStatus
            (string.IsNullOrEmpty(employeeFiltering.EmploymentStatus) || d.EmploymentStatus == employeeFiltering.EmploymentStatus) &&
            // فلترة ب HiringDate
            (!employeeFiltering.HiringDate.HasValue || d.HiringDate == employeeFiltering.HiringDate)
            );

            employees = employeeFiltering.SortBy?.ToLower() switch
            {
                "hiringdate" => employeeFiltering.isDescending
                ? employees.OrderByDescending(e => e.HiringDate)
                : employees.OrderBy(e => e.HiringDate),

                "userid" => employeeFiltering.isDescending
                ? employees.OrderByDescending(e => e.UserID)
                : employees.OrderBy(e => e.UserID),

                _ => employees
            };

            var totalEmployees = employees.Count();

            employees = await _employeeService.GetAllEmployeesPagedAsync(paginationParameters.PageNumber, paginationParameters.PageSize);

            var result = new PaginationResponse<EmployeeToReturnDTO>
                (
                    data: employees,
                    pageNumber: paginationParameters.PageNumber,
                    pageSize: paginationParameters.PageSize,
                    totalItems: totalEmployees
                );

            return Ok(result);
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner , Branch Manager")]
        [HttpGet("GetAllEmployeesByBranchId{branchId}")]
        public async Task<IActionResult> GetAllEmployeesByBranchId(int branchId)
        {
            var employees = await _employeeService.GetAllEmployeesByBranchId(branchId);
            return Ok(employees);
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner , Branch Manager")]
        [HttpGet("GetEmployeeByID{id}")]
        public async Task<IActionResult> GetEmployeeByIDAsync(int id)
        {
            var employee = await _employeeService.GetEmployeeByIDAsync(id);
            return Ok(employee);
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner , Branch Manager")]
        [HttpGet("GetEmployeeByName{name}")]
        public async Task<IActionResult> GetEmployeeByNameAsync(string name)
        {
            var employee = await _employeeService.GetEmployeeByNameAsync(name);
            return Ok(employee);
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner")]
        [HttpPut("UpdateEmoployeeInformation")]
        public async Task<IActionResult> UpdateEmoployeeInformationAsync(int employeeId,[FromBody]UpdateEmoployeeInformationModel updateEmoployeeInformation)
        {
            var updatedEmployee = await _employeeService.UpdateEmoployeeInformationAsync(employeeId, updateEmoployeeInformation);
            return Ok(updatedEmployee);
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner")]
        [HttpPut("UpdateEmoployeeStatus")]
        public async Task<IActionResult> UpdateEmoployeeStatusAsync(int employeeId, string status)
        {
            var updatedEmployee = await _employeeService.UpdateEmoployeeStatusAsync(employeeId, status);
            return Ok(updatedEmployee);
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner")]
        [HttpPut("UpdateEmoployeeShift")]
        public async Task<IActionResult> UpdateEmoployeeShiftAsync(int employeeId, string shift)
        {
            var updatedEmployee = await _employeeService.UpdateEmoployeeShiftAsync(employeeId, shift);
            return Ok(updatedEmployee);
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner")]
        [HttpPut("TransferEmployeeFromBranchToBranch")]
        public async Task<IActionResult> TransferEmployeeFromBranchToBranchAsync(int employeeId, int branchId)
        {
            var updatedEmployee = await _employeeService.TransferEmployeeFromBranchToBranchAsync(employeeId, branchId);
            return Ok(updatedEmployee);
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner")]
        [HttpDelete("DeleteEmployeeByID{id}")]
        public async Task<IActionResult> DeleteEmployeeAsync(int employeeId)
        {
            var deletedEmployee = await _employeeService.DeleteEmployeeAsync(employeeId);
            return Ok(deletedEmployee);
        }
    }
}
