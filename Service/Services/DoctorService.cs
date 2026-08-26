using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Exceptions;
using Microsoft.EntityFrameworkCore;
using Repository.Context;
using Service.Interfaces;
using Service.Models.DoctorModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Service.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly PharmacyManagementDbContext _dbContext;
        private readonly IMapper _mapper;
        public DoctorService(PharmacyManagementDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public IEnumerable<DoctorToReturnDTO> GetAllDoctorsFiltered(Func<Doctor, bool> Filter)
        {
            var doctors = _dbContext.Doctors
                          .Include(d => d.Branch)
                          .Where(Filter)
                          .ToList();

            return _mapper.Map<IEnumerable<DoctorToReturnDTO>>(doctors);
        }

        public async Task<IEnumerable<DoctorToReturnDTO>> GetAllDoctorsPagedAsync(int pageNumber, int pageSize)
        {
            var doctors = await _dbContext.Doctors
                         .Include(d => d.Branch)
                         .Skip((pageNumber - 1) * pageSize)
                         .Take(pageSize)
                         .ToListAsync();

            return _mapper.Map<IEnumerable<DoctorToReturnDTO>>(doctors);
        }

        public async Task<DoctorToReturnDTO> GetDoctorByIDAsync(int doctorId)
        {
            var specifiedDoctor = await _dbContext.Doctors
                                  .Include(d => d.Branch)
                                  .FirstOrDefaultAsync(d=>d.ID == doctorId);
            if (specifiedDoctor is null)
                throw new NotFoundException("This doctor isn't exist.");

            return _mapper.Map<DoctorToReturnDTO>(specifiedDoctor);
        }

        public async Task<DoctorToReturnDTO> GetDoctorByNameAsync(string doctorName)
        {
            var specifiedDoctor = await _dbContext.Doctors
                                  .Include(d => d.Branch)
                                  .FirstOrDefaultAsync(d => d.FullName == doctorName);

            if (specifiedDoctor is null)
                throw new NotFoundException("This doctor isn't exist.");

            return _mapper.Map<DoctorToReturnDTO>(specifiedDoctor);
        }
        public async Task<DoctorToReturnDTO> AddNewDoctorAsync(AddNewDoctorModel addNewDoctor)
        {
            var existsPhoneNumber = await _dbContext.Doctors.AnyAsync(d => d.PhoneNumber == addNewDoctor.PhoneNumber);
            if (existsPhoneNumber)
                throw new ValidationException("This doctor is already exist.");

            var existsLicenseNumber = await _dbContext.Doctors.AnyAsync(d => d.LicenseNumber == addNewDoctor.LicenseNumber);
            if (existsLicenseNumber)
                throw new ValidationException("This doctor is already exist.");

            var existsBranch = await _dbContext.Branches.FirstOrDefaultAsync(b => b.ID == addNewDoctor.BranchID);
            if (existsBranch is null)
                throw new NotFoundException("This branch isn't exist.");

            var doctor = new Doctor
            {
                FullName = addNewDoctor.FullName,
                PhoneNumber = addNewDoctor.PhoneNumber,
                LicenseNumber = addNewDoctor.LicenseNumber,
                Specialization = addNewDoctor.Specialization,
                MedicalFacility = addNewDoctor.MedicalFacility,
                Status = "Active",
                BranchID = addNewDoctor.BranchID
            };

            await _dbContext.Doctors.AddAsync(doctor);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<DoctorToReturnDTO>(doctor);
        }

        public async Task<string> DeleteDoctorAsync(int doctorId)
        {
            var specifiedDoctor = await _dbContext.Doctors
                                  .Include(d => d.Branch)
                                  .FirstOrDefaultAsync(d => d.ID == doctorId);
            if (specifiedDoctor is null)
                throw new NotFoundException("This doctor isn't exist.");

            _dbContext.Doctors.Remove(specifiedDoctor);
            await _dbContext.SaveChangesAsync();

            return "Doctor deleted successfully.";
        }


        public async Task<DoctorToReturnDTO> UpdateDoctorInformationAsync(int doctorId, UpdateDoctorInformationModel updateDoctorInformation)
        {
            var specifiedDoctor = await _dbContext.Doctors
                                  .Include(d => d.Branch)
                                  .FirstOrDefaultAsync(d => d.ID == doctorId);
            if (specifiedDoctor is null)
                throw new NotFoundException("This doctor isn't exist.");

            var existsPhoneNumber = await _dbContext.Doctors.AnyAsync(d => d.PhoneNumber == updateDoctorInformation.PhoneNumber);
            if (existsPhoneNumber)
                throw new ValidationException("This doctor is already exist.");

            var existsLicenseNumber = await _dbContext.Doctors.AnyAsync(d => d.LicenseNumber == updateDoctorInformation.LicenseNumber);
            if (existsLicenseNumber)
                throw new ValidationException("This doctor is already exist.");

            var existsBranch = await _dbContext.Branches.FirstOrDefaultAsync(b => b.ID == updateDoctorInformation.BranchID);
            if (existsBranch is null)
                throw new NotFoundException("This branch isn't exist.");

            specifiedDoctor.FullName = updateDoctorInformation.FullName;
            specifiedDoctor.PhoneNumber = updateDoctorInformation.PhoneNumber;
            specifiedDoctor.Specialization = updateDoctorInformation.Specialization;
            specifiedDoctor.LicenseNumber = updateDoctorInformation.LicenseNumber;
            specifiedDoctor.MedicalFacility = updateDoctorInformation.MedicalFacility;
            specifiedDoctor.BranchID = updateDoctorInformation.BranchID;

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<DoctorToReturnDTO>(specifiedDoctor);
        }

        public async Task<DoctorToReturnDTO> UpdateDoctorStatusAsync(int doctorId, string status)
        {
            var specifiedDoctor = await _dbContext.Doctors
                                   .Include(d => d.Branch)
                                   .FirstOrDefaultAsync(d => d.ID == doctorId);
            if (specifiedDoctor is null)
                throw new NotFoundException("This doctor isn't exist.");

            var validStatuses = new[] { "Active", "Inactive" };
            if (!validStatuses.Contains(status))
                throw new NotFoundException("This status isn't valid. Valid statuses(Active ,and Inactive).");

            specifiedDoctor.Status = status;

            return _mapper.Map<DoctorToReturnDTO>(specifiedDoctor);
        }
    }
}
