using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Exceptions;
using Microsoft.EntityFrameworkCore;
using Repository.Context;
using Service.Interfaces;
using Service.Models.MedicineModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Service.Services
{
    public class MedicineService : IMedicineService
    {
        private readonly PharmacyManagementDbContext _dbContext;
        private readonly IMapper _mapper;
        public MedicineService(PharmacyManagementDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public IEnumerable<MedicineToReturnDTO> GetAllMedicinesFiltered(Func<Medicine, bool> Filter)
        {
            var medicines = _dbContext.Medicines
                            .Where(Filter)
                            .ToList();

            return _mapper.Map<IEnumerable<MedicineToReturnDTO>>(medicines);
        }

        public async Task<IEnumerable<MedicineToReturnDTO>> GetAllMedicinesPagedAsync(int pageNumber, int pageSize)
        {
            var medicines = await _dbContext.Medicines
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();

            return _mapper.Map<IEnumerable<MedicineToReturnDTO>>(medicines);
        }

        public async Task<MedicineToReturnDTO> GetMedicineByIDAsync(int medicineId)
        {
            var specifiedMedicine = await _dbContext.Medicines.FirstOrDefaultAsync(m=>m.ID ==  medicineId);
            if (specifiedMedicine is null)
                throw new NotFoundException("This medicne isn't exist.");

            return _mapper.Map<MedicineToReturnDTO>(specifiedMedicine);
        }

        public async Task<MedicineToReturnDTO> GetMedicineByNameAsync(string medicineName)
        {
            var specifiedMedicine = await _dbContext.Medicines.FirstOrDefaultAsync(m => m.MedicineName == medicineName);
            if (specifiedMedicine is null)
                throw new NotFoundException("This medicne isn't exist.");

            return _mapper.Map<MedicineToReturnDTO>(specifiedMedicine);
        }
        public async Task<MedicineToReturnDTO> AddNewMedicineAsync(AddNewMedicineModel addNewMedicine)
        {
            var existsBarcode = await _dbContext.Medicines.AnyAsync(m=>m.Barcode == addNewMedicine.Barcode);
            if (existsBarcode)
                throw new ValidationException("This medicine is already exist.");

            if (addNewMedicine.MinimumStockLevel < 0)
                throw new ValidationException("Minimum stock level must be greater than zero.");

            if (addNewMedicine.SellingPrice <= 0)
                throw new ValidationException("Selling price must be greater than or equal zero.");

            if (addNewMedicine.PurchasePrice <= 0)
                throw new ValidationException("Purchase price must be greater than or equal zero.");

            var medicine = new Medicine
            {
                MedicineName = addNewMedicine.MedicineName,
                ScientificName = addNewMedicine.ScientificName,
                BrandName = addNewMedicine.BrandName,
                Description = addNewMedicine.Description,
                Category = addNewMedicine.Category,
                DosageForm = addNewMedicine.DosageForm,
                Strength = addNewMedicine.Strength,
                Barcode = addNewMedicine.Barcode,
                Manufacturer = addNewMedicine.Manufacturer,
                PrescriptionRequirement = addNewMedicine.PrescriptionRequirement,
                AvailabilityStatus = "Available",
                SellingPrice = addNewMedicine.SellingPrice,
                PurchasePrice = addNewMedicine.PurchasePrice,
                MinimumStockLevel = addNewMedicine.MinimumStockLevel
            };

            await _dbContext.Medicines.AddAsync(medicine);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<MedicineToReturnDTO>(medicine);
        }

        public async Task<string> DeleteMedicineByIDAsync(int medicineId)
        {
            var specifiedMedicine = await _dbContext.Medicines.FirstOrDefaultAsync(m => m.ID == medicineId);
            if (specifiedMedicine is null)
                throw new NotFoundException("This medicne isn't exist.");

            _dbContext.Medicines.Remove(specifiedMedicine);
            await _dbContext.SaveChangesAsync();

            return "Medicine deleted successfully.";
        }

        public async Task<MedicineToReturnDTO> UpdateMedicineInformationAsync(int medicineId, UpdateMedicineInformationModel updateMedicineInformation)
        {
            var specifiedMedicine = await _dbContext.Medicines.FirstOrDefaultAsync(m => m.ID == medicineId);
            if (specifiedMedicine is null)
                throw new NotFoundException("This medicne isn't exist.");

            var existsBarcode = await _dbContext.Medicines.AnyAsync(m => m.Barcode == updateMedicineInformation.Barcode);
            if (existsBarcode)
                throw new ValidationException("This medicine is already exist.");

            specifiedMedicine.MedicineName = updateMedicineInformation.MedicineName;
            specifiedMedicine.ScientificName = updateMedicineInformation.ScientificName;
            specifiedMedicine.BrandName = updateMedicineInformation.BrandName;
            specifiedMedicine.Description = updateMedicineInformation.Description;
            specifiedMedicine.Category = updateMedicineInformation.Category;
            specifiedMedicine.DosageForm = updateMedicineInformation.DosageForm;
            specifiedMedicine.Strength = updateMedicineInformation.Strength;
            specifiedMedicine.Barcode = updateMedicineInformation.Barcode;
            specifiedMedicine.Manufacturer = updateMedicineInformation.Manufacturer;
            specifiedMedicine.PrescriptionRequirement = updateMedicineInformation.PrescriptionRequirement;

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<MedicineToReturnDTO>(specifiedMedicine);
        }

        public async Task<MedicineToReturnDTO> UpdateMedicineMinimumStockLevelAsync(int medicineId, int MinimumStockLevel)
        {
            var specifiedMedicine = await _dbContext.Medicines.FirstOrDefaultAsync(m => m.ID == medicineId);
            if (specifiedMedicine is null)
                throw new NotFoundException("This medicne isn't exist.");

            if (MinimumStockLevel < 0)
                throw new ValidationException("Minimum stock level must be greater than zero.");

            specifiedMedicine.MinimumStockLevel = MinimumStockLevel;

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<MedicineToReturnDTO>(specifiedMedicine);
        }

        public async Task<MedicineToReturnDTO> UpdateMedicinePurchasePriceAsync(int medicineId, decimal purchasePrice)
        {
            var specifiedMedicine = await _dbContext.Medicines.FirstOrDefaultAsync(m => m.ID == medicineId);
            if (specifiedMedicine is null)
                throw new NotFoundException("This medicne isn't exist.");

            if (purchasePrice <= 0)
                throw new ValidationException("Purchase price must be greater than or equal zero.");

            specifiedMedicine.PurchasePrice = purchasePrice;

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<MedicineToReturnDTO>(specifiedMedicine);
        }

        public async Task<MedicineToReturnDTO> UpdateMedicineSellingPriceAsync(int medicineId, decimal sellingPrice)
        {
            var specifiedMedicine = await _dbContext.Medicines.FirstOrDefaultAsync(m => m.ID == medicineId);
            if (specifiedMedicine is null)
                throw new NotFoundException("This medicne isn't exist.");

            if (sellingPrice <= 0)
                throw new ValidationException("Purchase price must be greater than or equal zero.");

            specifiedMedicine.SellingPrice = sellingPrice;

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<MedicineToReturnDTO>(specifiedMedicine);
        }

        public async Task<MedicineToReturnDTO> UpdateMedicineStatusAsync(int medicineId, string status)
        {
            var specifiedMedicine = await _dbContext.Medicines.FirstOrDefaultAsync(m => m.ID == medicineId);
            if (specifiedMedicine is null)
                throw new NotFoundException("This medicne isn't exist.");

            var validStatuses = new[] { "Available", "Unavailable", "Discontinued", "Recalled" };
            if (!validStatuses.Contains(status))
                throw new ValidationException("This status isn't valid. Valid statuses(Available,Unavailable,Discontinued,Recalled).");

            specifiedMedicine.AvailabilityStatus = status;

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<MedicineToReturnDTO>(specifiedMedicine);
        }
    }
}
