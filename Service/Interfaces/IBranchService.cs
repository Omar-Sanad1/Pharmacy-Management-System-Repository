using Core.DTOs;
using Core.Entities;
using Service.Models.BatchModels;
using Service.Models.BranchModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IBranchService
    {
        public Task<IEnumerable<BranchToReturnDTO>> GetAllBranchesPagedAsync(int pageNumber, int pageSize);
        public IEnumerable<BranchToReturnDTO> GetAllBranchesFiltered(Func<Branch, bool> Filter);
        public Task<BranchToReturnDTO> GetBranchByIDAsync(int branchId);
        public Task<BranchToReturnDTO> GetBranchByBranchCodeAsync(string branchCode);
        public Task<BranchToReturnDTO> AddNewBranchAsync(AddNewBranchModel addNewBranch);
        public Task<BranchToReturnDTO> UpdateBranchInformationAsync(int branchId , UpdateBranchInformationModel updateBranchInformation);
        public Task<BranchToReturnDTO> UpdateBranchStatusAsync(int branchId, string status);
        public Task<string> DeleteBranchAsync(int branchId);
    }
}
