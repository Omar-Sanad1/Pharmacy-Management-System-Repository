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
using Service.Models.BranchModels;
using System.Net.NetworkInformation;

namespace PharmacyManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchController : ControllerBase
    {
        private readonly IBranchService _branchService;
        public BranchController(IBranchService branchService)
        {
            _branchService = branchService; 
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner")]
        [HttpGet("GetAllBranchesPagedFiltered")]
        public async Task<IActionResult> GetAllBranchesPagedFiltered([FromQuery] BranchFiltering branchFiltering, [FromQuery] PaginationParameters paginationParameters)
        {
            var branches = _branchService.GetAllBranchesFiltered(b =>
            //  فلترة ب Location
            (string.IsNullOrEmpty(branchFiltering.Location) || b.Location == branchFiltering.Location) &&
            //  فلترة ب OperationalStatus
            (string.IsNullOrEmpty(branchFiltering.OperationalStatus) || b.OperationalStatus == branchFiltering.OperationalStatus)
            );

            branches = branchFiltering.SortBy?.ToLower() switch
            {
                "branchcode" => branchFiltering.isDescending
                ? branches.OrderByDescending(b => b.BranchCode)
                : branches.OrderBy(b => b.BranchCode),

                "operationalstatus" => branchFiltering.isDescending
                ? branches.OrderByDescending(b => b.OperationalStatus)
                : branches.OrderBy(b => b.OperationalStatus),

                "operatinghours" => branchFiltering.isDescending
                ? branches.OrderByDescending(b => b.OperatingHours)
                : branches.OrderBy(b => b.OperatingHours),

                _ => branches
            };

            var totalBranches = branches.Count();

            branches = await _branchService.GetAllBranchesPagedAsync(paginationParameters.PageNumber, paginationParameters.PageSize);

            var response = new PaginationResponse<BranchToReturnDTO>
                (
                    data: branches,
                    pageNumber: paginationParameters.PageNumber,
                    pageSize: paginationParameters.PageSize,
                    totalItems: totalBranches
                );

            return Ok(response);
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner")]
        [HttpGet("GetBranchByID{id}")]
        public async Task<IActionResult> GetBranchByIDAsync(int id)
        {
            var branch = await _branchService.GetBranchByIDAsync(id);
            return Ok(branch);
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner")]
        [HttpGet("GetBranchByBranchCode{code}")]
        public async Task<IActionResult> GetBranchByBranchCodeAsync(string code)
        {
            var branch = await _branchService.GetBranchByBranchCodeAsync(code);
            return Ok(branch);
        }

        [Authorize(Roles = "System Administrator")]
        [HttpPost("AddNewBranch")]
        public async Task<IActionResult> AddNewBranchAsync([FromBody]AddNewBranchModel addNewBranch)
        {
            var addedBranch = await _branchService.AddNewBranchAsync(addNewBranch);
            return Ok(addedBranch);
        }

        [Authorize(Roles = "System Administrator")]
        [HttpPut("UpdateBranchInformation")]
        public async Task<IActionResult> UpdateBranchInformationAsync(int branchId,[FromBody]UpdateBranchInformationModel updateBranchInformation)
        {
            var updatedBranch = await _branchService.UpdateBranchInformationAsync(branchId,updateBranchInformation);
            return Ok(updatedBranch);
        }

        [Authorize(Roles = "System Administrator")]
        [HttpPut("UpdateBranchStatus")]
        public async Task<IActionResult> UpdateBranchStatusAsync(int branchId, string status)
        {
            var updatedBranch = await _branchService.UpdateBranchStatusAsync(branchId, status);
            return Ok(updatedBranch);
        }

        [Authorize(Roles = "System Administrator")]
        [HttpDelete("DeleteBranch")]
        public async Task<IActionResult> DeleteBranchAsync(int branchId)
        {
            var deletedBranch = await _branchService.DeleteBranchAsync(branchId);
            return Ok(deletedBranch);
        }
    }
}
