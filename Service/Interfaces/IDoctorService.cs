using Core.DTOs;
using Core.Entities;
using Service.Models.BranchModels;
using Service.Models.DoctorModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IDoctorService
    {
        public Task<IEnumerable<DoctorToReturnDTO>> GetAllDoctorsPagedAsync(int pageNumber, int pageSize);
        public IEnumerable<DoctorToReturnDTO> GetAllDoctorsFiltered(Func<Doctor, bool> Filter);
        public Task<DoctorToReturnDTO> GetDoctorByIDAsync(int doctorId);
        public Task<DoctorToReturnDTO> GetDoctorByNameAsync(string doctorName);
        public Task<DoctorToReturnDTO> AddNewDoctorAsync(AddNewDoctorModel addNewDoctor);
        public Task<DoctorToReturnDTO> UpdateDoctorInformationAsync(int doctorId , UpdateDoctorInformationModel updateDoctorInformation);
        public Task<DoctorToReturnDTO> UpdateDoctorStatusAsync(int doctorId, string status);
        public Task<string> DeleteDoctorAsync(int doctorId);
    }
}
