using Core.DTOs;
using Core.Entities;
using Service.Models.BatchModels;
using Service.Models.EmployeeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IEmployeeService
    {
        public Task<IEnumerable<EmployeeToReturnDTO>> GetAllEmployeesPagedAsync(int pageNumber, int pageSize);
        public IEnumerable<EmployeeToReturnDTO> GetAllEmployeesFiltered(Func<Employee, bool> Filter);
        public Task<IEnumerable<EmployeeToReturnDTO>> GetAllEmployeesByBranchId(int branchId);
        public Task<EmployeeToReturnDTO> GetEmployeeByIDAsync(int employeeId);
        public Task<EmployeeToReturnDTO> GetEmployeeByNameAsync(string employeeName);
        public Task<EmployeeToReturnDTO> UpdateEmoployeeInformationAsync(int employeeId , UpdateEmoployeeInformationModel updateEmoployeeInformation);
        public Task<EmployeeToReturnDTO> UpdateEmoployeeStatusAsync(int employeeId , string status);
        public Task<EmployeeToReturnDTO> UpdateEmoployeeShiftAsync(int employeeId, string shift);
        public Task<EmployeeToReturnDTO> TransferEmployeeFromBranchToBranchAsync(int employeeId, int branchId);
        public Task<string> DeleteEmployeeAsync(int employeeId);
    }
}
