using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Exceptions;
using Microsoft.EntityFrameworkCore;
using Repository.Context;
using Service.Interfaces;
using Service.Models.PrescriptionModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Service.Services
{
    public class PrescriptionService : IPrescriptionService
    {
        private readonly PharmacyManagementDbContext _dbContext;
        private readonly IMapper _mapper;
        public PrescriptionService(PharmacyManagementDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IEnumerable<PrescriptionToReturnDTO> GetAllPrescriptionsFiltered(Func<Prescription, bool> Filter)
        {
            var prescriptions = _dbContext.Prescriptions
                                .Include(p=>p.Doctor)
                                .Include(p=>p.Customer)
                                .Include(p=>p.Employee)
                                .Where(Filter)
                                .ToList();

            return _mapper.Map<IEnumerable<PrescriptionToReturnDTO>>(prescriptions);
        }

        public async Task<IEnumerable<PrescriptionToReturnDTO>> GetAllPrescriptionsPagedAsync(int pageNumber, int pageSize)
        {
            var prescriptions = await _dbContext.Prescriptions
                                .Include(p => p.Doctor)
                                .Include(p => p.Customer)
                                .Include(p => p.Employee)
                                .Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync();

            return _mapper.Map<IEnumerable<PrescriptionToReturnDTO>>(prescriptions);
        }

        public async Task<PrescriptionToReturnDTO> GetPrescriptionByIDAsync(int prescriptionId)
        {
            var specifiedPrescription = await _dbContext.Prescriptions
                                        .Include(p => p.Doctor)
                                        .Include(p => p.Customer)
                                        .Include(p => p.Employee)
                                        .FirstOrDefaultAsync(p => p.ID == prescriptionId);

            if (specifiedPrescription is null)
                throw new NotFoundException("This prescription isn't exist.");

            return _mapper.Map<PrescriptionToReturnDTO>(specifiedPrescription);
        }

        public async Task<IEnumerable<PrescriptionToReturnDTO>> GetAllCustomerPrescriptionsAsync(int customerId)
        {
            var specifiedCustomer = await _dbContext.Customers  
                                    .Include(c=>c.Prescriptions)
                                    .ThenInclude(c=>c.Doctor)
                                    .Include(c=>c.Prescriptions)
                                    .ThenInclude(c=>c.Employee)
                                    .FirstOrDefaultAsync(c => c.ID == customerId);

            if (specifiedCustomer is null)
                throw new NotFoundException("This customer isn't exist.");

            var prescriptions = specifiedCustomer.Prescriptions;

            return _mapper.Map<IEnumerable<PrescriptionToReturnDTO>>(prescriptions);
        }

        public async Task<IEnumerable<PrescriptionToReturnDTO>> GetAllDoctorPrescriptionsAsync(int doctorId)
        {
            var specifiedDoctor =   await _dbContext.Doctors
                                    .Include(d => d.Prescriptions)
                                    .ThenInclude(d=>d.Employee)
                                    .Include(d=>d.Prescriptions)
                                    .ThenInclude(d=>d.Customer)
                                    .FirstOrDefaultAsync(d => d.ID == doctorId);

            if (specifiedDoctor is null)
                throw new NotFoundException("This doctor isn't exist.");

            var prescriptions = specifiedDoctor.Prescriptions;

            return _mapper.Map<IEnumerable<PrescriptionToReturnDTO>>(prescriptions);
        }

        public async Task<PrescriptionToReturnDTO> AddNewPrescriptionAsync(AddNewPrescriptionModel addNewPrescription)
        {
            var specifiedDoctor = await _dbContext.Doctors.FirstOrDefaultAsync(d => d.ID == addNewPrescription.DoctorID);
            if (specifiedDoctor is null)
                throw new NotFoundException("This doctor isn't exist.");

            var specifiedCustomer = await _dbContext.Customers.FirstOrDefaultAsync(c => c.ID == addNewPrescription.CustomerID);
            if (specifiedCustomer is null)
                throw new NotFoundException("This customer isn't exist.");

            var specifiedEmployee = await _dbContext.Employees.FirstOrDefaultAsync(e => e.ID == addNewPrescription.EmployeeID);
            if (specifiedEmployee is null)
                throw new NotFoundException("This employee isn't exist.");

            var validStatuses = new[] { "Approved" , "Rejected" , "Pending" , "Expired"};
            if (!validStatuses.Contains(addNewPrescription.PrescriptionStatus))
                throw new ValidationException("This status isn't valid. Valid statuses(Approved , Rejected , Pending , and Expired).");

            if (addNewPrescription.ExpirationDate <= addNewPrescription.PrescriptionDate)
                throw new ValidationException("Expiration date must be after prescription date.");

            if (addNewPrescription.PrescriptionDate > DateTime.Now)
                throw new ValidationException("Prescription date cannot be in the future.");

            var prescription = new Prescription
            {
                PrescriptionStatus = addNewPrescription.PrescriptionStatus,
                Notes = addNewPrescription.Notes,
                PrescriptionDate = addNewPrescription.PrescriptionDate,
                ExpirationDate = addNewPrescription.ExpirationDate,
                DoctorID = addNewPrescription.DoctorID,
                CustomerID = addNewPrescription.CustomerID,
                EmployeeID = addNewPrescription.EmployeeID
            };

            await _dbContext.Prescriptions.AddAsync(prescription);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<PrescriptionToReturnDTO>(prescription);
        }

        public async Task<PrescriptionToReturnDTO> UpdatePrescriptionInformationAsync(int prescriptionId, UpdatePrescriptionInformationModel updatePrescriptionInformation)
        {
            var specifiedPrescription = await _dbContext.Prescriptions
                                        .Include(p => p.Doctor)
                                        .Include(p => p.Customer)
                                        .Include(p => p.Employee)
                                        .FirstOrDefaultAsync(p=>p.ID == prescriptionId);

            if (specifiedPrescription is null)
                throw new NotFoundException("This prescription isn't exist.");

            var specifiedDoctor = await _dbContext.Doctors.FirstOrDefaultAsync(d => d.ID == updatePrescriptionInformation.DoctorID);
            if (specifiedDoctor is null)
                throw new NotFoundException("This doctor isn't exist.");

            var specifiedCustomer = await _dbContext.Customers.FirstOrDefaultAsync(c => c.ID == updatePrescriptionInformation.CustomerID);
            if (specifiedCustomer is null)
                throw new NotFoundException("This customer isn't exist.");

            var specifiedEmployee = await _dbContext.Doctors.FirstOrDefaultAsync(e => e.ID == updatePrescriptionInformation.EmployeeID);
            if (specifiedEmployee is null)
                throw new NotFoundException("This employee isn't exist.");

            if (updatePrescriptionInformation.ExpirationDate <= updatePrescriptionInformation.PrescriptionDate)
                throw new ValidationException("Expiration date must be after prescription date.");

            if (updatePrescriptionInformation.PrescriptionDate > DateTime.Now)
                throw new ValidationException("Prescription date cannot be in the future.");

            specifiedPrescription.Notes = updatePrescriptionInformation.Notes;
            specifiedPrescription.PrescriptionDate = updatePrescriptionInformation.PrescriptionDate;
            specifiedPrescription.ExpirationDate = updatePrescriptionInformation.ExpirationDate;
            specifiedPrescription.DoctorID = updatePrescriptionInformation.DoctorID;
            specifiedPrescription.CustomerID = updatePrescriptionInformation.CustomerID;
            specifiedPrescription.EmployeeID = updatePrescriptionInformation.EmployeeID;

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<PrescriptionToReturnDTO>(specifiedPrescription);
        }

        public async Task<PrescriptionToReturnDTO> UpdatePrescriptionStatusAsync(int prescriptionId, string status)
        {
            var specifiedPrescription = await _dbContext.Prescriptions
                                        .Include(p => p.Doctor)
                                        .Include(p => p.Customer)
                                        .Include(p => p.Employee)
                                        .FirstOrDefaultAsync(p => p.ID == prescriptionId);

            if (specifiedPrescription is null)
                throw new NotFoundException("This prescription isn't exist.");

            var validStatuses = new[] { "Approved", "Rejected", "Pending", "Expired" };
            if (!validStatuses.Contains(status))
                throw new ValidationException("This status isn't valid. Valid statuses(Approved , Rejected , Pending , and Expired).");

            specifiedPrescription.PrescriptionStatus = status;

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<PrescriptionToReturnDTO>(specifiedPrescription);
        }
        public async Task<string> DeletePrescriptionByIDAsync(int prescriptionId)
        {
            var specifiedPrescription = await _dbContext.Prescriptions.FirstOrDefaultAsync(p => p.ID == prescriptionId);
            if (specifiedPrescription is null)
                throw new NotFoundException("This prescription isn't exist.");

            _dbContext.Prescriptions.Remove(specifiedPrescription);
            await _dbContext.SaveChangesAsync();

            return "Prescription deleted successfully.";
        }

        
    }
}
