using Core.DTOs;
using Core.Entities;
using Service.Models.EmployeeModels;
using Service.Models.MedicineModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IMedicineService
    {
        public Task<IEnumerable<MedicineToReturnDTO>> GetAllMedicinesPagedAsync(int pageNumber, int pageSize);
        public IEnumerable<MedicineToReturnDTO> GetAllMedicinesFiltered(Func<Medicine, bool> Filter);
        public Task<MedicineToReturnDTO> GetMedicineByIDAsync(int medicineId);
        public Task<MedicineToReturnDTO> GetMedicineByNameAsync(string medicineName);
        public Task<MedicineToReturnDTO> AddNewMedicineAsync(AddNewMedicineModel addNewMedicine);
        public Task<MedicineToReturnDTO> UpdateMedicineInformationAsync(int medicineId , UpdateMedicineInformationModel updateMedicineInformation);
        public Task<MedicineToReturnDTO> UpdateMedicineStatusAsync(int medicineId, string status);
        public Task<MedicineToReturnDTO> UpdateMedicineSellingPriceAsync(int medicineId, decimal sellingPrice);
        public Task<MedicineToReturnDTO> UpdateMedicinePurchasePriceAsync(int medicineId, decimal purchasePrice);
        public Task<MedicineToReturnDTO> UpdateMedicineMinimumStockLevelAsync(int medicineId, int MinimumStockLevel);
        public Task<string> DeleteMedicineByIDAsync(int medicineId);
    }
}
