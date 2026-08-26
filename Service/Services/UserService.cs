using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Exceptions;
using Microsoft.EntityFrameworkCore;
using Repository.Context;
using Service.Interfaces;
using Service.Models.UserModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Service.Services
{
    public class UserService : IUserService
    {
        private readonly PharmacyManagementDbContext _dbContext;
        private readonly IMapper _mapper;
        public UserService(PharmacyManagementDbContext dbContext , IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<IEnumerable<UserToReturnDTO>> GetAllUsersByRoleIDAsync(int roleId)
        {
            var specifiedRole = await _dbContext.Roles
                                .Include(r=>r.Users)
                                .FirstOrDefaultAsync(r=>r.ID ==  roleId);

            if (specifiedRole is null)
                throw new NotFoundException("This role isn't exist.");

            var roleUsers = specifiedRole.Users;

            return _mapper.Map<IEnumerable<UserToReturnDTO>>(roleUsers);
        }

        public IEnumerable<UserToReturnDTO> GetAllUsersFiltered(Func<User, bool> Filter)
        {
            var users = _dbContext.Users
                        .Where(Filter)
                        .ToList();

            return _mapper.Map<IEnumerable<UserToReturnDTO>>(users);
        }

        public async Task<IEnumerable<UserToReturnDTO>> GetAllUsersPagedAsync(int pageNumber, int pageSize)
        {
            var users = await _dbContext.Users
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToListAsync();

            return _mapper.Map<IEnumerable<UserToReturnDTO>>(users);
        }

        public async Task<UserToReturnDTO> GetUserByEmailAddressAsync(string emailAddress)
        {
            var specifiedUser = await _dbContext.Users
                                .FirstOrDefaultAsync(u=>u.EmailAddress ==  emailAddress);

            if (specifiedUser is null)
                throw new NotFoundException("This user isn't exist.");

            return _mapper.Map<UserToReturnDTO>(specifiedUser);
        }

        public async Task<UserToReturnDTO> GetUserByIDAsync(int userId)
        {
            var specifiedUser = await _dbContext.Users
                                .FirstOrDefaultAsync(u => u.ID == userId);

            if (specifiedUser is null)
                throw new NotFoundException("This user isn't exist.");

            return _mapper.Map<UserToReturnDTO>(specifiedUser);
        }

        public async Task<UserToReturnDTO> GetUserByUserNameAsync(string userName)
        {
            var specifiedUser = await _dbContext.Users
                                .FirstOrDefaultAsync(u => u.UserName == userName);

            if (specifiedUser is null)
                throw new NotFoundException("This user isn't exist.");

            return _mapper.Map<UserToReturnDTO>(specifiedUser);
        }
        public async Task<string> DeleteUserByIDAsync(int userId)
        {
            var specifiedUser = await _dbContext.Users
                                .FirstOrDefaultAsync(u => u.ID == userId);

            if (specifiedUser is null)
                throw new NotFoundException("This user isn't exist.");

            _dbContext.Users.Remove(specifiedUser);
            await _dbContext.SaveChangesAsync();

            return "User deleted successfully.";
        }
        public async Task<UserToReturnDTO> UpdateUserInformationAsync(int userId, UpdateUserInformationModel updateUserInformation)
        {
            var specifiedUser = await _dbContext.Users
                                .FirstOrDefaultAsync(u => u.ID == userId);

            if (specifiedUser is null)
                throw new NotFoundException("This user isn't exist.");

            var specifiedUserName = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == updateUserInformation.UserName);
            if (specifiedUserName is not null)
                throw new ValidationException("This user is already exist.");

            var specifiedEmailAddress = await _dbContext.Users.FirstOrDefaultAsync(u => u.EmailAddress == updateUserInformation.EmailAddress);
            if (specifiedEmailAddress is not null)
                throw new ValidationException("This user is already exist.");

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(updateUserInformation.PasswordHash);

            var specifiedRole = await _dbContext.Roles
                                .Include(r => r.Users)
                                .FirstOrDefaultAsync(r => r.ID == updateUserInformation.RoleID);

            if (specifiedRole is null)
                throw new NotFoundException("This role isn't exist.");

            specifiedUser.UserName = updateUserInformation.UserName;
            specifiedUser.EmailAddress = updateUserInformation.EmailAddress;
            specifiedUser.PasswordHash = hashedPassword;
            specifiedUser.CreatedAt = updateUserInformation.CreatedAt;
            specifiedUser.RoleID = updateUserInformation.RoleID;

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<UserToReturnDTO>(specifiedUser);
        }
    }
}
