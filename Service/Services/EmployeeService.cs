using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Exceptions;
using Microsoft.EntityFrameworkCore;
using Repository.Context;
using Service.Interfaces;
using Service.Models.EmployeeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Service.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly PharmacyManagementDbContext _dbContext;
        private readonly IMapper _mapper;
        public EmployeeService(PharmacyManagementDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public IEnumerable<EmployeeToReturnDTO> GetAllEmployeesFiltered(Func<Employee, bool> Filter)
        {
            var employees = _dbContext.Employees
                            .Include(e=>e.Branch)
                            .Where(Filter)
                            .ToList();

            return _mapper.Map<IEnumerable<EmployeeToReturnDTO>>(employees);
        }

        public async Task<IEnumerable<EmployeeToReturnDTO>> GetAllEmployeesPagedAsync(int pageNumber, int pageSize)
        {
            var employees = await _dbContext.Employees
                            .Include(e => e.Branch)
                            .Skip((pageNumber - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();

            return _mapper.Map<IEnumerable<EmployeeToReturnDTO>>(employees);
        }
        public async Task<IEnumerable<EmployeeToReturnDTO>> GetAllEmployeesByBranchId(int branchId)
        {
            var specifiedBranch = await _dbContext.Branches
                                  .Include(b=>b.Employees)
                                  .FirstOrDefaultAsync(b => b.ID == branchId);

            if (specifiedBranch is null)
                throw new NotFoundException("This branch isn't exist.");

            var branchEmployees = specifiedBranch.Employees;

            return _mapper.Map<IEnumerable<EmployeeToReturnDTO>>(branchEmployees);
        }
        public async Task<EmployeeToReturnDTO> GetEmployeeByIDAsync(int employeeId)
        {
            var specificEmployee = await _dbContext.Employees
                                   .Include(e => e.Branch)
                                   .FirstOrDefaultAsync(e=>e.ID == employeeId);

            if(specificEmployee is null)
                throw new NotFoundException("This employee isn't exist.");

            return _mapper.Map<EmployeeToReturnDTO>(specificEmployee);
        }

        public async Task<EmployeeToReturnDTO> GetEmployeeByNameAsync(string employeeName)
        {
            var specifiedEmployee = await _dbContext.Employees
                                    .Include(e => e.Branch)
                                    .FirstOrDefaultAsync(e => e.FullName == employeeName);

            if (specifiedEmployee is null)
                throw new NotFoundException("This employee isn't exist.");

            return _mapper.Map<EmployeeToReturnDTO>(specifiedEmployee);
        }


        public async Task<string> DeleteEmployeeAsync(int employeeId)
        {
            var specifiedEmployee = await _dbContext.Employees.FirstOrDefaultAsync(e => e.ID == employeeId);
            if (specifiedEmployee is null)
                throw new NotFoundException("This employee isn't exist.");

            _dbContext.Employees.Remove(specifiedEmployee);
            await _dbContext.SaveChangesAsync();

            return "Employee deleted successfully.";
        }
        public async Task<EmployeeToReturnDTO> UpdateEmoployeeStatusAsync(int employeeId, string status)
        {
            var specifiedEmployee = await _dbContext.Employees.FirstOrDefaultAsync(e => e.ID == employeeId);
            if (specifiedEmployee is null)
                throw new NotFoundException("This employee isn't exist.");

            var validStatuses = new[] { "Active", "Inactive" };
            if (!validStatuses.Contains(status))
                throw new ValidationException("This status isn't valid. Valid statuses(Active , and Inactive).");

            specifiedEmployee.EmploymentStatus = status;

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<EmployeeToReturnDTO>(specifiedEmployee);
        }
        public async Task<EmployeeToReturnDTO> UpdateEmoployeeShiftAsync(int employeeId, string shift)
        {
            var specifiedEmployee = await _dbContext.Employees.FirstOrDefaultAsync(e => e.ID == employeeId);
            if (specifiedEmployee is null)
                throw new NotFoundException("This employee isn't exist.");

            var validShifts = new[] { "Morning" , "Evening" , "Night"};
            if(!validShifts.Contains(shift))
                throw new ValidationException("This shift isn't valid. Valid shifts(Morning,Evening , and Night).");

            specifiedEmployee.Shift = shift;

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<EmployeeToReturnDTO>(specifiedEmployee);
        }

        public async Task<EmployeeToReturnDTO> UpdateEmoployeeInformationAsync(int employeeId, UpdateEmoployeeInformationModel updateEmoployeeInformation)
        {
            var specifiedEmployee = await _dbContext.Employees
                                    .Include(e => e.Branch)
                                    .FirstOrDefaultAsync(e => e.ID == employeeId);
            if (specifiedEmployee is null)
                throw new NotFoundException("This employee isn't exist.");

            var existsLicenseNumber = await _dbContext.Employees.AnyAsync(e => e.LicenseNumber == updateEmoployeeInformation.LicenseNumber);
            if (existsLicenseNumber)
                throw new ValidationException("This employee is already exist.");

            var employee = new Employee
            {
                FullName = updateEmoployeeInformation.FullName,
                PhoneNumber = updateEmoployeeInformation.PhoneNumber,
                LicenseNumber = updateEmoployeeInformation.LicenseNumber,
                EmployeePosition = updateEmoployeeInformation.EmployeePosition,
                Salary = updateEmoployeeInformation.Salary,
                UserID = updateEmoployeeInformation.UserID
            };

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<EmployeeToReturnDTO>(specifiedEmployee);
        }
        public async Task<EmployeeToReturnDTO> TransferEmployeeFromBranchToBranchAsync(int employeeId, int branchId)
        {
            var specifiedEmployee = await _dbContext.Employees
                                    .Include(e => e.Branch)
                                    .FirstOrDefaultAsync(e => e.ID == employeeId);

            if (specifiedEmployee is null)
                throw new NotFoundException("This employee isn't exist.");

            var specifiedBranch = await _dbContext.Branches.FirstOrDefaultAsync(b=>b.ID == branchId);
            if(specifiedBranch is null) 
                throw new NotFoundException("This branch isn't exist.");

            specifiedEmployee.BranchID = branchId;
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<EmployeeToReturnDTO>(specifiedEmployee);
        }

    }
}
