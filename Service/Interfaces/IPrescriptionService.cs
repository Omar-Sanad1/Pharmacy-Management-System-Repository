using Core.DTOs;
using Core.Entities;
using Service.Models.MedicineModels;
using Service.Models.PrescriptionModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IPrescriptionService
    {
        public Task<IEnumerable<PrescriptionToReturnDTO>> GetAllPrescriptionsPagedAsync(int pageNumber, int pageSize);
        public IEnumerable<PrescriptionToReturnDTO> GetAllPrescriptionsFiltered(Func<Prescription, bool> Filter);
        public Task<PrescriptionToReturnDTO> GetPrescriptionByIDAsync(int prescriptionId);
        public Task<IEnumerable<PrescriptionToReturnDTO>> GetAllCustomerPrescriptionsAsync(int customerId);
        public Task<IEnumerable<PrescriptionToReturnDTO>> GetAllDoctorPrescriptionsAsync(int doctorId);
        public Task<PrescriptionToReturnDTO> AddNewPrescriptionAsync(AddNewPrescriptionModel addNewPrescription);
        public Task<PrescriptionToReturnDTO> UpdatePrescriptionInformationAsync(int prescriptionId , UpdatePrescriptionInformationModel updatePrescriptionInformation);
        public Task<PrescriptionToReturnDTO> UpdatePrescriptionStatusAsync(int prescriptionId, string status);
        public Task<string> DeletePrescriptionByIDAsync(int prescriptionId);
    }
}
