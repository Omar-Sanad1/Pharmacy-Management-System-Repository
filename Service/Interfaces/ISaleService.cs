using Core.DTOs;
using Core.Entities;
using Service.Models.MedicineModels;
using Service.Models.SaleModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface ISaleService
    {
        public Task<IEnumerable<SaleToReturnDTO>> GetAllSalesPagedAsync(int pageNumber, int pageSize);
        public IEnumerable<SaleToReturnDTO> GetAllSalesFiltered(Func<Sale, bool> Filter);
        public Task<IEnumerable<SaleToReturnDTO>> GetAllBranchSalesAsync(int branchId);
        public Task<IEnumerable<SaleToReturnDTO>> GetAllCustomerSalesAsync(int customerId);
        public Task<IEnumerable<SaleToReturnDTO>> GetAllEmployeeSalesAsync(int employeeId);
        public Task<SaleToReturnDTO> GetSaleByIDAsync(int saleId);
        public Task<SaleToReturnDTO> AddNewSaleAsync(AddNewSaleModel addNewSale);
        public Task<SaleToReturnDTO> UpdateSaleInformationAsync(int saleId , UpdateSaleInformationModel updateSaleInformation);
        public Task<SaleToReturnDTO> UpdateSaleStatusAsync(int saleId, string saleStatus);
        public Task<SaleToReturnDTO> UpdateSalePaymentStatusAsync(int saleId, string paymentStatus);
        public Task<SaleToReturnDTO> UpdateSaleSubtotalAsync(int saleId, decimal subTotal);
        public Task<SaleToReturnDTO> UpdateSaleDiscountAsync(int saleId, decimal discount);
        public Task<SaleToReturnDTO> UpdateSaleTaxesAsync(int saleId, decimal taxes);
        public Task<SaleToReturnDTO> UpdateSaleTotalAmountAsync(int saleId, decimal totalAmount);
        public Task<string> DeleteSaleByIDAsync(int saleId);
    }
}
