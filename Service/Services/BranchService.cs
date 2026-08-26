using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Exceptions;
using Microsoft.EntityFrameworkCore;
using Repository.Context;
using Service.Interfaces;
using Service.Models.BranchModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class BranchService : IBranchService
    {
        private readonly PharmacyManagementDbContext _dbContext;
        private readonly IMapper _mapper;
        public BranchService(PharmacyManagementDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public IEnumerable<BranchToReturnDTO> GetAllBranchesFiltered(Func<Branch, bool> Filter)
        {
            var branches = _dbContext.Branches
                           .Where(Filter)
                           .ToList();

            return _mapper.Map<IEnumerable<BranchToReturnDTO>>(branches);
        }

        public async Task<IEnumerable<BranchToReturnDTO>> GetAllBranchesPagedAsync(int pageNumber, int pageSize)
        {
            var branches = await _dbContext.Branches
                           .Skip((pageNumber - 1) * pageSize)
                           .Take(pageSize)
                           .ToListAsync();

            return _mapper.Map<IEnumerable<BranchToReturnDTO>>(branches);
        }

        public async Task<BranchToReturnDTO> GetBranchByBranchCodeAsync(string branchCode)
        {
            var specifiedBranch = await _dbContext.Branches.FirstOrDefaultAsync(b => b.BranchCode == branchCode);
            if (specifiedBranch is null)
                throw new NotFoundException("This branch isn't exist.");

            return _mapper.Map<BranchToReturnDTO>(specifiedBranch);
        }

        public async Task<BranchToReturnDTO> GetBranchByIDAsync(int branchId)
        {
            var specifiedBranch = await _dbContext.Branches.FirstOrDefaultAsync(b => b.ID == branchId);
            if (specifiedBranch is null)
                throw new NotFoundException("This branch isn't exist.");

            return _mapper.Map<BranchToReturnDTO>(specifiedBranch);
        }

        public async Task<BranchToReturnDTO> AddNewBranchAsync(AddNewBranchModel addNewBranch)
        {
            var existsBranchCode = await _dbContext.Branches.AnyAsync(b => b.BranchCode == addNewBranch.BranchCode);
            if (existsBranchCode)
                throw new ValidationException("This branch is already exist.");

            var existsEmail = await _dbContext.Branches.AnyAsync(b => b.EmailAddress == addNewBranch.EmailAddress);
            if(existsEmail)
                throw new ValidationException("This branch is already exist.");

            var existsPhoneNumber = await _dbContext.Branches.AnyAsync(b => b.PhoneNumber == addNewBranch.PhoneNumber);
            if (existsPhoneNumber)
                throw new ValidationException("This branch is already exist.");

            if(addNewBranch.OperatingHours <= 0)
                throw new ValidationException("Operating hours must be greater than zero.");

            var branch = new Branch
            {
                BranchName = addNewBranch.BranchName,
                BranchCode = addNewBranch.BranchCode,
                EmailAddress = addNewBranch.EmailAddress,
                PhoneNumber = addNewBranch.PhoneNumber,
                Location = addNewBranch.Location,
                Manager = addNewBranch.Manager,
                OperationalStatus = "Active",
                OperatingHours = addNewBranch.OperatingHours
            };

            await _dbContext.Branches.AddAsync(branch);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<BranchToReturnDTO>(branch);
        }

        public async Task<string> DeleteBranchAsync(int branchId)
        {
            var specifiedBranch = await _dbContext.Branches.FirstOrDefaultAsync(b => b.ID == branchId);
            if (specifiedBranch is null)
                throw new NotFoundException("This branch isn't exist.");

            _dbContext.Branches.Remove(specifiedBranch);
            await _dbContext.SaveChangesAsync();

            return "Branch deleted successfully.";
        }
        public async Task<BranchToReturnDTO> UpdateBranchInformationAsync(int branchId, UpdateBranchInformationModel updateBranchInformation)
        {
            var specifiedBranch = await _dbContext.Branches.FirstOrDefaultAsync(b => b.ID == branchId);
            if (specifiedBranch is null)
                throw new NotFoundException("This branch isn't exist.");

            var existsBranchCode = await _dbContext.Branches.AnyAsync(b => b.BranchCode == updateBranchInformation.BranchCode);
            if (existsBranchCode)
                throw new ValidationException("This branch is already exist.");

            var existsEmail = await _dbContext.Branches.AnyAsync(b => b.EmailAddress == updateBranchInformation.EmailAddress);
            if (existsEmail)
                throw new ValidationException("This branch is already exist.");

            var existsPhoneNumber = await _dbContext.Branches.AnyAsync(b => b.PhoneNumber == updateBranchInformation.PhoneNumber);
            if (existsPhoneNumber)
                throw new ValidationException("This branch is already exist.");

            if (updateBranchInformation.OperatingHours <= 0)
                throw new ValidationException("Operating hours must be greater than zero.");

            specifiedBranch.BranchName = updateBranchInformation.BranchName;
            specifiedBranch.BranchCode = updateBranchInformation.BranchCode;
            specifiedBranch.PhoneNumber = updateBranchInformation.PhoneNumber;
            specifiedBranch.EmailAddress = updateBranchInformation.EmailAddress;
            specifiedBranch.Location = updateBranchInformation.Location;
            specifiedBranch.Manager = updateBranchInformation.Manager;
            specifiedBranch.OperatingHours = updateBranchInformation.OperatingHours;

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<BranchToReturnDTO>(specifiedBranch);
        }
        public async Task<BranchToReturnDTO> UpdateBranchStatusAsync(int branchId, string status)
        {
            var specifiedBranch = await _dbContext.Branches.FirstOrDefaultAsync(b => b.ID == branchId);
            if (specifiedBranch is null)
                throw new NotFoundException("This branch isn't exist.");

            var validStatuses = new [] { "Active" , "Inactive" , "Temporarily Closed" , "Under Maintenance" , "Suspended" };
            if (!validStatuses.Contains(status))
                throw new NotFoundException("This status isn't valid. Valid statuses(Active,Inactive,Temporarily Closed,Under Maintenance,Suspended)");

            specifiedBranch.OperationalStatus = status;
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<BranchToReturnDTO>(specifiedBranch);
        }
    }
}
