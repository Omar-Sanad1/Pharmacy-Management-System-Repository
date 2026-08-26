using Core.DTOs;
using Core.Entities;
using Service.Models.BatchModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IBatchService
    {
        public Task<IEnumerable<BatchToReturnDTO>> GetAllBatchesPagedAsync(int pageNumber, int pageSize);
        public IEnumerable<BatchToReturnDTO> GetAllBatchesFiltered(Func<Batch, bool> Filter);
        public Task<BatchToReturnDTO> GetBatchByIDAsync(int batchId);
        public Task<BatchToReturnDTO> GetBatchByBatchNumberAsync(string batchNumber);
        public Task<BatchToReturnDTO> AddNewBatchAsync(AddNewBatchModel addNewBatch);
        public Task<BatchToReturnDTO> UpdateBatchInformationAsync(int batchId , UpdateBatchInformationModel updateBatchInformation);
        public Task<string> DeleteBatchAsync(int batchId);
    }
}
