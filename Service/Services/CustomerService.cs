using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Exceptions;
using Microsoft.EntityFrameworkCore;
using Repository.Context;
using Service.Interfaces;
using Service.Models.CustomerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Service.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly PharmacyManagementDbContext _dbContext;
        private readonly IMapper _mapper;
        public CustomerService(PharmacyManagementDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public IEnumerable<CustomerToReturnDTO> GetAllCustomersFiltered(Func<Customer, bool> Filter)
        {
            var customers = _dbContext.Customers
                            .Where(Filter)
                            .ToList();

            return _mapper.Map<IEnumerable<CustomerToReturnDTO>>(customers);
        }

        public async Task<IEnumerable<CustomerToReturnDTO>> GetAllCustomersPagedAsync(int pageNumber, int pageSize)
        {
            var customers = await _dbContext.Customers
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();

            return _mapper.Map<IEnumerable<CustomerToReturnDTO>>(customers);
        }

        public async Task<CustomerToReturnDTO> GetCustomerByIDAsync(int customerId)
        {
            var specifiedCustomer = await _dbContext.Customers.FirstOrDefaultAsync(c => c.ID == customerId);
            if (specifiedCustomer is null)
                throw new NotFoundException("This customer isn't exist.");

            return _mapper.Map<CustomerToReturnDTO>(specifiedCustomer);
        }

        public async Task<CustomerToReturnDTO> GetCustomerByNameAsync(string customerName)
        {
            var specifiedCustomer = await _dbContext.Customers.FirstOrDefaultAsync(c => c.FullName == customerName);
            if (specifiedCustomer is null)
                throw new NotFoundException("This customer isn't exist.");

            return _mapper.Map<CustomerToReturnDTO>(specifiedCustomer);
        }
        public async Task<string> DeleteCustomerAsync(int customerId)
        {
            var specifiedCustomer = await _dbContext.Customers.FirstOrDefaultAsync(c => c.ID == customerId);
            if (specifiedCustomer is null)
                throw new NotFoundException("This customer isn't exist.");

            _dbContext.Customers.Remove(specifiedCustomer);
            await _dbContext.SaveChangesAsync();

            return "Customer deleted successfully";
        }
        public async Task<CustomerToReturnDTO> UpdateCustomerStatusAsync(int customerId, string status)
        {
            var specifiedCustomer = await _dbContext.Customers.FirstOrDefaultAsync(c => c.ID == customerId);
            if (specifiedCustomer is null)
                throw new NotFoundException("This customer isn't exist.");

            var validStatuses = new[] { "Active", "Inactive" };
            if (!validStatuses.Contains(status))
                throw new NotFoundException("This status isn't valid. Valid statuses(Active and Inactive).");

            specifiedCustomer.AccountStatus = status;

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<CustomerToReturnDTO>(specifiedCustomer);
        }
    }
}
