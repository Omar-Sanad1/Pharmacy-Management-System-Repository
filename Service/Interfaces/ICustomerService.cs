using Core.DTOs;
using Core.Entities;
using Service.Models.BranchModels;
using Service.Models.CustomerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface ICustomerService
    {
        public Task<IEnumerable<CustomerToReturnDTO>> GetAllCustomersPagedAsync(int pageNumber, int pageSize);
        public IEnumerable<CustomerToReturnDTO> GetAllCustomersFiltered(Func<Customer, bool> Filter);
        public Task<CustomerToReturnDTO> GetCustomerByIDAsync(int customerId);
        public Task<CustomerToReturnDTO> GetCustomerByNameAsync(string customerName);
        public Task<CustomerToReturnDTO> UpdateCustomerStatusAsync(int customerId, string status);
        public Task<string> DeleteCustomerAsync(int customerId);
    }
}
