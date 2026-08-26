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
using Service.Models.UserModels;

namespace PharmacyManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner")]
        [HttpGet("GetAllUsersPagedFiltered")]
        public async Task<IActionResult> GetAllUsersPagedFiltered([FromQuery] UserFiltering userFiltering, [FromQuery] PaginationParameters paginationParameters)
        {
            var users = _userService.GetAllUsersFiltered(u =>
            //  فلترة ب CreatedAt
            (!userFiltering.CreatedAt.HasValue || u.CreatedAt == userFiltering.CreatedAt)
            );

            users = userFiltering.SortBy?.ToLower() switch
            {
                "createdat" => userFiltering.isDescending
                ? users.OrderByDescending(u => u.CreatedAt)
                : users.OrderBy(u => u.CreatedAt),

                "roleid" => userFiltering.isDescending
                ? users.OrderByDescending(u => u.RoleID)
                : users.OrderBy(u => u.RoleID),

                _ => users
            };

            var totalUsers = users.Count();

            users = await _userService.GetAllUsersPagedAsync(paginationParameters.PageNumber, paginationParameters.PageSize);

            var response = new PaginationResponse<UserToReturnDTO>
                (
                    data: users,
                    pageNumber: paginationParameters.PageNumber,
                    pageSize: paginationParameters.PageSize,
                    totalItems: totalUsers
                );

            return Ok(response);
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner")]
        [HttpGet("GetUserByID{id}")]
        public async Task<IActionResult> GetUserByIDAsync(int id)
        {
            var user = await _userService.GetUserByIDAsync(id);
            return Ok(user);
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner")]
        [HttpGet("GetUserByUserName{userName}")]
        public async Task<IActionResult> GetUserByUserNameAsync(string userName)
        {
            var user = await _userService.GetUserByUserNameAsync(userName);
            return Ok(user);
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner")]
        [HttpGet("GetUserByEmailAddress{emailAddress}")]
        public async Task<IActionResult> GetUserByEmailAddressAsync(string emailAddress)
        {
            var user = await _userService.GetUserByEmailAddressAsync(emailAddress);
            return Ok(user);
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner")]
        [HttpGet("GetAllUsersByRoleID{roleId}")]
        public async Task<IActionResult> GetAllUsersByRoleIDAsync(int roleId)
        {
            var user = await _userService.GetAllUsersByRoleIDAsync(roleId);
            return Ok(user);
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner")]
        [HttpPut("UpdateUserInformation")]
        public async Task<IActionResult> UpdateUserInformationAsync(int userId,[FromBody]UpdateUserInformationModel updateUserInformation)
        {
            var updatedUser = await _userService.UpdateUserInformationAsync(userId, updateUserInformation);
            return Ok(updatedUser);
        }

        [Authorize(Roles = "System Administrator , Pharmacy Owner")]
        [HttpDelete("DeleteUserByID{id}")]
        public async Task<IActionResult> DeleteUserByIDAsync(int userId)
        {
            var deletedUser = await _userService.DeleteUserByIDAsync(userId);
            return Ok(deletedUser);
        }
    }
}
