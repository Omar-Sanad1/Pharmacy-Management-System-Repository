using Core.DTOs;
using Core.Entities;
using Service.Models.SaleModels;
using Service.Models.SupplierModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface ISupplierService
    {
        public Task<IEnumerable<SupplierToReturnDTO>> GetAllSuppliersPagedAsync(int pageNumber, int pageSize);
        public IEnumerable<SupplierToReturnDTO> GetAllSuppliersFiltered(Func<Supplier, bool> Filter);
        public Task<SupplierToReturnDTO> GetSupplierByIDAsync(int supplierId);
        public Task<SupplierToReturnDTO> AddNewSupplierAsync(AddNewSupplierModel addNewSupplier);
        public Task<SupplierToReturnDTO> UpdateSupplierInformationAsync(int supplierId , UpdateSupplierInformationModel updateSupplierInformation);
        public Task<SupplierToReturnDTO> UpdateSupplierStatusAsync(int supplierId, string supplierStatus);
        public Task<SupplierToReturnDTO> UpdateSupplierRatingAsync(int supplierId, int rating);
        public Task<string> DeleteSupplierByIDAsync(int supplierId);
    }
}
