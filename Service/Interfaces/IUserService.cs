using Core.DTOs;
using Core.Entities;
using Service.Models.SupplierModels;
using Service.Models.UserModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IUserService
    {
        public Task<IEnumerable<UserToReturnDTO>> GetAllUsersPagedAsync(int pageNumber, int pageSize);
        public IEnumerable<UserToReturnDTO> GetAllUsersFiltered(Func<User, bool> Filter);
        public Task<UserToReturnDTO> GetUserByIDAsync(int userId);
        public Task<UserToReturnDTO> GetUserByUserNameAsync(string userName);
        public Task<UserToReturnDTO> GetUserByEmailAddressAsync(string emailAddress);
        public Task<IEnumerable<UserToReturnDTO>> GetAllUsersByRoleIDAsync(int roleId);
        public Task<UserToReturnDTO> UpdateUserInformationAsync(int userId , UpdateUserInformationModel updateUserInformation);
        public Task<string> DeleteUserByIDAsync(int userId);
    }
}
