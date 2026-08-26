using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Exceptions;
using Microsoft.EntityFrameworkCore;
using Repository.Context;
using Service.Interfaces;
using Service.Models.SupplierModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Service.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly PharmacyManagementDbContext _dbContext;
        private readonly IMapper _mapper;
        public SupplierService(PharmacyManagementDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IEnumerable<SupplierToReturnDTO> GetAllSuppliersFiltered(Func<Supplier, bool> Filter)
        {
            var suppliers = _dbContext.Suppliers
                            .Where(Filter)
                            .ToList();

            return _mapper.Map<IEnumerable<SupplierToReturnDTO>>(suppliers);
        }

        public async Task<IEnumerable<SupplierToReturnDTO>> GetAllSuppliersPagedAsync(int pageNumber, int pageSize)
        {
            var suppliers = await _dbContext.Suppliers
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();

            return _mapper.Map<IEnumerable<SupplierToReturnDTO>>(suppliers);
        }
        public async Task<SupplierToReturnDTO> GetSupplierByIDAsync(int supplierId)
        {
            var specifiedSupplier = await _dbContext.Suppliers.FirstOrDefaultAsync(s=>s.ID ==  supplierId);
            if (specifiedSupplier is null)
                throw new NotFoundException("This supplier isn't exist.");

            return _mapper.Map<SupplierToReturnDTO>(specifiedSupplier);
        }
        public async Task<SupplierToReturnDTO> AddNewSupplierAsync(AddNewSupplierModel addNewSupplier)
        {
            var existsEmail = await _dbContext.Suppliers.FirstOrDefaultAsync(s => s.EmailAddress == addNewSupplier.EmailAddress);
            if (existsEmail is not null)
                throw new ValidationException("This supplier is already exist.");

            var validSupplierStatuses = new[] { "Active" , "Inactive" };
            if (!validSupplierStatuses.Contains(addNewSupplier.SupplierStatus))
                throw new ValidationException("This status isn't valid. Valid statuses(Active , and Inactive).");

            if (addNewSupplier.SupplierRating < 0 || addNewSupplier.SupplierRating > 10)
                throw new ValidationException("Supplier rating must be greater than zero and less than eleven.");

            var supplier = new Supplier
            {
                CompanyName = addNewSupplier.CompanyName,
                ContactPerson = addNewSupplier.ContactPerson,
                PhoneNumber = addNewSupplier.PhoneNumber,
                EmailAddress = addNewSupplier.EmailAddress,
                Address = addNewSupplier.Address,
                TaxInformation = addNewSupplier.TaxInformation,
                PaymentTerms = addNewSupplier.PaymentTerms,
                SupplierStatus = addNewSupplier.SupplierStatus,
                SupplierRating = addNewSupplier.SupplierRating
            };

            await _dbContext.Suppliers.AddAsync(supplier);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<SupplierToReturnDTO>(supplier);
        }
        public async Task<SupplierToReturnDTO> UpdateSupplierInformationAsync(int supplierId, UpdateSupplierInformationModel updateSupplierInformation)
        {
            var specifiedSupplier = await _dbContext.Suppliers.FirstOrDefaultAsync(s => s.ID == supplierId);
            if (specifiedSupplier is null)
                throw new NotFoundException("This supplier isn't exist.");

            var existsEmail = await _dbContext.Suppliers.FirstOrDefaultAsync(s => s.EmailAddress == updateSupplierInformation.EmailAddress);
            if (existsEmail is not null)
                throw new ValidationException("This supplier is already exist.");

            specifiedSupplier.CompanyName = updateSupplierInformation.CompanyName;
            specifiedSupplier.ContactPerson = updateSupplierInformation.ContactPerson;
            specifiedSupplier.PhoneNumber = updateSupplierInformation.PhoneNumber;
            specifiedSupplier.EmailAddress = updateSupplierInformation.EmailAddress;
            specifiedSupplier.Address = updateSupplierInformation.Address;
            specifiedSupplier.TaxInformation = updateSupplierInformation.TaxInformation;
            specifiedSupplier.PaymentTerms = updateSupplierInformation.PaymentTerms;

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<SupplierToReturnDTO>(specifiedSupplier);
        }

        public async Task<SupplierToReturnDTO> UpdateSupplierRatingAsync(int supplierId, int rating)
        {
            var specifiedSupplier = await _dbContext.Suppliers.FirstOrDefaultAsync(s => s.ID == supplierId);
            if (specifiedSupplier is null)
                throw new NotFoundException("This supplier isn't exist.");

            if (rating < 0 || rating > 10)
                throw new ValidationException("Supplier rating must be greater than zero and less than eleven.");

            specifiedSupplier.SupplierRating = rating;

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<SupplierToReturnDTO>(specifiedSupplier);
        }

        public async Task<SupplierToReturnDTO> UpdateSupplierStatusAsync(int supplierId, string supplierStatus)
        {
            var specifiedSupplier = await _dbContext.Suppliers.FirstOrDefaultAsync(s => s.ID == supplierId);
            if (specifiedSupplier is null)
                throw new NotFoundException("This supplier isn't exist.");

            var validSupplierStatuses = new[] { "Active", "Inactive" };
            if (!validSupplierStatuses.Contains(supplierStatus))
                throw new ValidationException("This status isn't valid. Valid statuses(Active , and Inactive).");

            specifiedSupplier.SupplierStatus = supplierStatus;

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<SupplierToReturnDTO>(specifiedSupplier);
        }
        public async Task<string> DeleteSupplierByIDAsync(int supplierId)
        {
            var specifiedSupplier = await _dbContext.Suppliers.FirstOrDefaultAsync(s => s.ID == supplierId);
            if (specifiedSupplier is null)
                throw new NotFoundException("This supplier isn't exist.");

            _dbContext.Suppliers.Remove(specifiedSupplier);
            await _dbContext.SaveChangesAsync();

            return "Supplier deleted successfully.";
        }

    }
}
