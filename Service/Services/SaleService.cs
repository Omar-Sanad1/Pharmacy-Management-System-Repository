using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Exceptions;
using Microsoft.EntityFrameworkCore;
using Repository.Context;
using Service.Interfaces;
using Service.Models.SaleModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Service.Services
{
    public class SaleService : ISaleService
    {
        private readonly PharmacyManagementDbContext _dbContext;
        private readonly IMapper _mapper;
        public SaleService(PharmacyManagementDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IEnumerable<SaleToReturnDTO> GetAllSalesFiltered(Func<Sale, bool> Filter)
        {
            var sales = _dbContext.Sales
                        .Include(s=>s.Branch)
                        .Include(s=>s.Customer)
                        .Include(s=>s.Employee)
                        .Where(Filter)
                        .ToList();

            return _mapper.Map<IEnumerable<SaleToReturnDTO>>(sales);
        }

        public async Task<IEnumerable<SaleToReturnDTO>> GetAllSalesPagedAsync(int pageNumber, int pageSize)
        {
            var sales = await _dbContext.Sales
                        .Include(s => s.Branch)
                        .Include(s => s.Customer)
                        .Include(s => s.Employee)
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToListAsync();

            return _mapper.Map<IEnumerable<SaleToReturnDTO>>(sales);
        }

        public async Task<IEnumerable<SaleToReturnDTO>> GetAllBranchSalesAsync(int branchId)
        {
            var specifiedBranch = await _dbContext.Branches
                                  .Include(b => b.Sales)
                                  .ThenInclude(b => b.Customer)
                                  .Include(b => b.Sales)
                                  .ThenInclude(b => b.Employee)
                                  .FirstOrDefaultAsync(b => b.ID == branchId);

            if (specifiedBranch is null)
                throw new NotFoundException("This branch isn't exist.");

            var sales = specifiedBranch.Sales;

            return _mapper.Map<IEnumerable<SaleToReturnDTO>>(sales);
        }

        public async Task<IEnumerable<SaleToReturnDTO>> GetAllCustomerSalesAsync(int customerId)
        {
            var specifiedCustomer = await _dbContext.Customers
                                    .Include(c=>c.Sales)
                                    .ThenInclude(c=>c.Branch)
                                    .Include(c=>c.Sales)
                                    .ThenInclude(c=>c.Employee)
                                    .FirstOrDefaultAsync(c=>c.ID == customerId);

            if(specifiedCustomer is null)
                throw new NotFoundException("This customer isn't exist.");

            var sales = specifiedCustomer.Sales;

            return _mapper.Map<IEnumerable<SaleToReturnDTO>>(sales);
        }

        public async Task<IEnumerable<SaleToReturnDTO>> GetAllEmployeeSalesAsync(int employeeId)
        {
            var specifiedEmployee = await _dbContext.Employees
                                    .Include(e=>e.Sales)
                                    .ThenInclude(e=>e.Customer)
                                    .Include(e=>e.Sales)
                                    .ThenInclude(e=>e.Branch)
                                    .FirstOrDefaultAsync(e=>e.ID == employeeId);

            if(specifiedEmployee is null)
                throw new NotFoundException("This employee isn't exist.");

            var sales = specifiedEmployee.Sales;

            return _mapper.Map<IEnumerable<SaleToReturnDTO>>(sales);
        }

        public async Task<SaleToReturnDTO> GetSaleByIDAsync(int saleId)
        {
            var specifiedSale = await _dbContext.Sales
                                .Include(s => s.Employee)
                                .Include(s => s.Customer)
                                .Include(s => s.Branch)
                                .FirstOrDefaultAsync(s => s.ID == saleId);

            if(specifiedSale is null)
                throw new NotFoundException("This sale isn't exist.");

            return _mapper.Map<SaleToReturnDTO>(specifiedSale);
        }
        public async Task<SaleToReturnDTO> AddNewSaleAsync(AddNewSaleModel addNewSale)
        {
            var specifiedEmployee = await _dbContext.Employees
                                   .FirstOrDefaultAsync(e => e.ID == addNewSale.EmployeeID);
            if (specifiedEmployee is null)
                throw new NotFoundException("This employee isn't exist.");

            var specifiedCustomer = await _dbContext.Customers
                                    .FirstOrDefaultAsync(c => c.ID == addNewSale.CustomerID);
            if (specifiedCustomer is null)
                throw new NotFoundException("This customer isn't exist.");

            var specifiedBranch = await _dbContext.Branches
                                 .FirstOrDefaultAsync(b => b.ID == addNewSale.BranchID);
            if (specifiedBranch is null)
                throw new NotFoundException("This branch isn't exist.");

            if (addNewSale.Subtotal < 0)
                throw new ValidationException("Subtotal must be greater than zero.");

            if (addNewSale.Discount < 0)
                throw new ValidationException("Discount must be greater than zero.");

            if (addNewSale.Taxes < 0)
                throw new ValidationException("Taxes must be greater than zero.");

            var totalAmount = addNewSale.Subtotal - addNewSale.Discount + addNewSale.Taxes;

            if (addNewSale.SaleDate > DateTime.Now)
                throw new ValidationException("Sale date must be less than or equal today.");

            var validPaymentStatuses = new[] { "Paid" , "Pending" , "Refunded" };
            if (!validPaymentStatuses.Contains(addNewSale.PaymentStatus))
                throw new ValidationException("This payment status isn't valid. Valid payment statuses (Paid , Panding , and Refunded).");

            var validSaleStatuses = new[] { "Completed", "Pending", "Cancelled" };
            if (!validSaleStatuses.Contains(addNewSale.SaleStatus))
                throw new ValidationException("This payment status isn't valid. Valid payment statuses (Completed , Panding , and Cancelled).");

            var sale = new Sale
            {
                Subtotal = addNewSale.Subtotal,
                Discount = addNewSale.Discount,
                Taxes = addNewSale.Taxes,
                TotalAmount = totalAmount,
                SaleDate = addNewSale.SaleDate,
                PaymentStatus = addNewSale.PaymentStatus,
                SaleStatus = addNewSale.SaleStatus,
                CustomerID = addNewSale.CustomerID,
                EmployeeID = addNewSale.EmployeeID,
                BranchID = addNewSale.BranchID
            };

            await _dbContext.Sales.AddAsync(sale);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<SaleToReturnDTO>(sale);
        }

        public async Task<SaleToReturnDTO> UpdateSaleDiscountAsync(int saleId, decimal discount)
        {
            var specifiedSale = await _dbContext.Sales.FirstOrDefaultAsync(s=>s.ID == saleId);
            if (specifiedSale is null)
                throw new NotFoundException("This sale isn't exist.");

            if (discount < 0)
                throw new ValidationException("Discount must be greater than zero.");

            specifiedSale.Discount = discount;

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<SaleToReturnDTO>(specifiedSale);
        }

        public async Task<SaleToReturnDTO> UpdateSaleSubtotalAsync(int saleId, decimal subTotal)
        {
            var specifiedSale = await _dbContext.Sales.FirstOrDefaultAsync(s => s.ID == saleId);
            if (specifiedSale is null)
                throw new NotFoundException("This sale isn't exist.");

            if (subTotal < 0)
                throw new ValidationException("Subtotal must be greater than zero.");

            specifiedSale.Subtotal = subTotal;

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<SaleToReturnDTO>(specifiedSale);
        }

        public async Task<SaleToReturnDTO> UpdateSaleTaxesAsync(int saleId, decimal taxes)
        {
            var specifiedSale = await _dbContext.Sales.FirstOrDefaultAsync(s => s.ID == saleId);
            if (specifiedSale is null)
                throw new NotFoundException("This sale isn't exist.");

            if (taxes < 0)
                throw new ValidationException("Taxes must be greater than zero.");

            specifiedSale.Taxes = taxes;

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<SaleToReturnDTO>(specifiedSale);
        }
        public async Task<SaleToReturnDTO> UpdateSaleTotalAmountAsync(int saleId, decimal totalAmount)
        {
            var specifiedSale = await _dbContext.Sales.FirstOrDefaultAsync(s => s.ID == saleId);
            if (specifiedSale is null)
                throw new NotFoundException("This sale isn't exist.");

            if (totalAmount < 0)
                throw new ValidationException("Total amount must be greater than zero.");

            specifiedSale.TotalAmount = totalAmount;

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<SaleToReturnDTO>(specifiedSale);
        }

        public async Task<SaleToReturnDTO> UpdateSaleInformationAsync(int saleId, UpdateSaleInformationModel updateSaleInformation)
        {
            var specifiedSale = await _dbContext.Sales.FirstOrDefaultAsync(s => s.ID == saleId);
            if (specifiedSale is null)
                throw new NotFoundException("This sale isn't exist.");

            var specifiedEmployee = await _dbContext.Employees
                                   .FirstOrDefaultAsync(e => e.ID == updateSaleInformation.EmployeeID);
            if (specifiedEmployee is null)
                throw new NotFoundException("This employee isn't exist.");

            var specifiedCustomer = await _dbContext.Customers
                                    .FirstOrDefaultAsync(c => c.ID == updateSaleInformation.CustomerID);
            if (specifiedCustomer is null)
                throw new NotFoundException("This customer isn't exist.");

            var specifiedBranch = await _dbContext.Branches
                                 .FirstOrDefaultAsync(b => b.ID == updateSaleInformation.BranchID);
            if (specifiedBranch is null)
                throw new NotFoundException("This branch isn't exist.");

            if (updateSaleInformation.SaleDate > DateTime.Now)
                throw new ValidationException("Sale date must be less than or equal today.");

            specifiedSale.EmployeeID = updateSaleInformation.EmployeeID;
            specifiedSale.CustomerID = updateSaleInformation.CustomerID;
            specifiedSale.BranchID = updateSaleInformation.BranchID;
            specifiedSale.SaleDate = updateSaleInformation.SaleDate;

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<SaleToReturnDTO>(specifiedSale);
        }

        public async Task<SaleToReturnDTO> UpdateSalePaymentStatusAsync(int saleId, string paymentStatus)
        {
            var specifiedSale = await _dbContext.Sales.FirstOrDefaultAsync(s => s.ID == saleId);
            if (specifiedSale is null)
                throw new NotFoundException("This sale isn't exist.");

            var validPaymentStatuses = new[] { "Paid", "Pending", "Refunded" };
            if (!validPaymentStatuses.Contains(paymentStatus))
                throw new ValidationException("This payment status isn't valid. Valid payment statuses (Paid , Panding , and Refunded).");

            specifiedSale.PaymentStatus = paymentStatus;
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<SaleToReturnDTO>(specifiedSale);
        }

        public async Task<SaleToReturnDTO> UpdateSaleStatusAsync(int saleId, string saleStatus)
        {
            var specifiedSale = await _dbContext.Sales.FirstOrDefaultAsync(s => s.ID == saleId);
            if (specifiedSale is null)
                throw new NotFoundException("This sale isn't exist.");

            var validSaleStatuses = new[] { "Completed", "Pending", "Cancelled" };
            if (!validSaleStatuses.Contains(saleStatus))
                throw new ValidationException("This payment status isn't valid. Valid payment statuses (Completed , Panding , and Cancelled).");

            specifiedSale.SaleStatus = saleStatus;
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<SaleToReturnDTO>(specifiedSale);
        }

        
        public async Task<string> DeleteSaleByIDAsync(int saleId)
        {
            var specifiedSale = await _dbContext.Sales
                                 .Include(s => s.Employee)
                                 .Include(s => s.Customer)
                                 .Include(s => s.Branch)
                                 .FirstOrDefaultAsync(s => s.ID == saleId);

            if (specifiedSale is null)
                throw new NotFoundException("This sale isn't exist.");

            _dbContext.Remove(specifiedSale);
            await _dbContext.SaveChangesAsync();

            return "Sale deleted successfully.";
        }


    }
}
