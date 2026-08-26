using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Exceptions;
using Microsoft.EntityFrameworkCore;
using Repository.Context;
using Service.Interfaces;
using Service.Models.BatchModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class BatchService : IBatchService
    {
        private readonly PharmacyManagementDbContext _dbContext;
        private readonly IMapper _mapper;
        public BatchService(PharmacyManagementDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public IEnumerable<BatchToReturnDTO> GetAllBatchesFiltered(Func<Batch, bool> Filter)
        {
            var batches = _dbContext.Batches
                          .Include(b => b.Medicine)
                          .Where(Filter)
                          .ToList();

            return _mapper.Map<IEnumerable<BatchToReturnDTO>>(batches);
        }

        public async Task<IEnumerable<BatchToReturnDTO>> GetAllBatchesPagedAsync(int pageNumber, int pageSize)
        {
            var batches = await _dbContext.Batches
                          .Include(b=>b.Medicine)
                          .Skip((pageNumber - 1) * pageSize)
                          .Take(pageSize)
                          .ToListAsync();

            return _mapper.Map<IEnumerable<BatchToReturnDTO>>(batches);
        }

        public async Task<BatchToReturnDTO> GetBatchByBatchNumberAsync(string batchNumber)
        {
            var specifiedBatch = await _dbContext.Batches
                                 .Include(b => b.Medicine)
                                 .FirstOrDefaultAsync(b => b.BatchNumber == batchNumber);
            if (specifiedBatch is null)
                throw new ValidationException("This batch isn't exist.");

            return _mapper.Map<BatchToReturnDTO>(specifiedBatch);
        }

        public async Task<BatchToReturnDTO> GetBatchByIDAsync(int batchId)
        {
            var specifiedBatch = await _dbContext.Batches
                                 .Include(b => b.Medicine)
                                 .FirstOrDefaultAsync(b => b.ID == batchId);
            if (specifiedBatch is null)
                throw new ValidationException("This batch isn't exist.");

            return _mapper.Map<BatchToReturnDTO>(specifiedBatch);
        }

        public async Task<BatchToReturnDTO> AddNewBatchAsync(AddNewBatchModel addNewBatch)
        {
            var validBatchNumber = await _dbContext.Batches.AnyAsync(b=>b.BatchNumber == addNewBatch.BatchNumber);
            if (validBatchNumber)
                throw new ValidationException("This batch is already exist.");

            var existsMedicine = await _dbContext.Medicines.FirstOrDefaultAsync(m => m.ID == addNewBatch.MedicineID);
            if (existsMedicine is null)
                throw new NotFoundException("This medicine isn't exist.");

            if(addNewBatch.ExpirationDate <= DateTime.Now)
                throw new ValidationException("This batch is expired and can't be sold.");

            if(addNewBatch.AvailableQuantity < 0)
                throw new ValidationException("Available quantity must be greater than or equal zero.");

            var batch = new Batch
            {
                BatchNumber = addNewBatch.BatchNumber,
                StorageLocation = addNewBatch.StorageLocation,
                PurchasePrice = addNewBatch.PurchasePrice,
                ManufacturingDate = addNewBatch.ManufacturingDate,
                ExpirationDate = addNewBatch.ExpirationDate,
                AvailableQuantity = addNewBatch.AvailableQuantity,
                MedicineID = addNewBatch.MedicineID
            };

            await _dbContext.Batches.AddAsync(batch);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<BatchToReturnDTO>(batch);
        }

        public async Task<string> DeleteBatchAsync(int batchId)
        {
            var specifiedBatch = await _dbContext.Batches.FirstOrDefaultAsync(b => b.ID == batchId);
            if (specifiedBatch is null)
                throw new ValidationException("This batch isn't exist.");

            _dbContext.Batches.Remove(specifiedBatch);
            await _dbContext.SaveChangesAsync();

            return "Batch deleted successfully";
        }
        public async Task<BatchToReturnDTO> UpdateBatchInformationAsync(int batchId , UpdateBatchInformationModel updateBatchInformation)
        {
            var specifiedBatch = await _dbContext.Batches.FirstOrDefaultAsync(b => b.ID == batchId);
            if (specifiedBatch is null)
                throw new ValidationException("This batch isn't exist.");

            var existsMedicine = await _dbContext.Medicines.FirstOrDefaultAsync(m => m.ID == updateBatchInformation.MedicineID);
            if (existsMedicine is null)
                throw new NotFoundException("This medicine isn't exist.");

            if (updateBatchInformation.ExpirationDate <= DateTime.Now)
                throw new ValidationException("This batch is expired and can't be sold.");

            if (updateBatchInformation.AvailableQuantity < 0)
                throw new ValidationException("Available quantity must be greater than or equal zero.");

            specifiedBatch.BatchNumber = updateBatchInformation.BatchNumber;
            specifiedBatch.StorageLocation = updateBatchInformation.StorageLocation;
            specifiedBatch.PurchasePrice = updateBatchInformation.PurchasePrice;
            specifiedBatch.ManufacturingDate = updateBatchInformation.ManufacturingDate;
            specifiedBatch.ExpirationDate = updateBatchInformation.ExpirationDate;
            specifiedBatch.AvailableQuantity = updateBatchInformation.AvailableQuantity;
            specifiedBatch.MedicineID = updateBatchInformation.MedicineID;

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<BatchToReturnDTO>(specifiedBatch);
        }
    }
}
