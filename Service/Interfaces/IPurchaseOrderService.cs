using Core.DTOs;
using Core.Entities;
using Service.Models.PrescriptionModels;
using Service.Models.PurchaseOrderModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces
{
    public interface IPurchaseOrderService
    {
        public Task<IEnumerable<PurchaseOrderToReturnDTO>> GetAllPurchaseOrdersPagedAsync(int pageNumber, int pageSize);
        public IEnumerable<PurchaseOrderToReturnDTO> GetAllPurchaseOrdersFiltered(Func<PurchaseOrder, bool> Filter);
        public Task<PurchaseOrderToReturnDTO> GetPurchaseOrderByIDAsync(int purchaseOrderId);
        public Task<IEnumerable<PurchaseOrderToReturnDTO>> GetBranchPurchaseOrdersAsync(int branchId);
        public Task<IEnumerable<PurchaseOrderToReturnDTO>> GetSupplierPurchaseOrdersAsync(int supplierId);
        public Task<PurchaseOrderToReturnDTO> AddNewPurchaseOrderAsync(AddNewPurchaseOrderModel addNewPurchaseOrder);
        public Task<PurchaseOrderToReturnDTO> UpdatePurchaseOrderInformationAsync(int purchaseOrderId , UpdatePurchaseOrderInformationModel updatePurchaseOrderInformation);
        public Task<PurchaseOrderToReturnDTO> UpdatePurchaseOrderStatusAsync(int purchaseOrderId, string purchaseOrderStatus);
        public Task<PurchaseOrderToReturnDTO> UpdateApprovalStatusAsync(int purchaseOrderId, string approvalStatus);
        public Task<PurchaseOrderToReturnDTO> UpdatePaymentStatusAsync(int purchaseOrderId, string paymentStatus);
        public Task<string> DeletePurchaseOrderByIDAsync(int purchaseOrderId);
    }
}
