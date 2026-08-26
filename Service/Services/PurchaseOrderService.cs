using AutoMapper;
using Core.DTOs;
using Core.Entities;
using Core.Exceptions;
using Microsoft.EntityFrameworkCore;
using Repository.Context;
using Service.Interfaces;
using Service.Models.PurchaseOrderModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Service.Services
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly PharmacyManagementDbContext _dbContext;
        private readonly IMapper _mapper;
        public PurchaseOrderService(PharmacyManagementDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public IEnumerable<PurchaseOrderToReturnDTO> GetAllPurchaseOrdersFiltered(Func<PurchaseOrder, bool> Filter)
        {
            var purchaseOrders = _dbContext.PurchaseOrders
                                 .Include(p => p.Branch)
                                 .Include(p => p.Supplier)
                                 .Where(Filter)
                                 .ToList();

            return _mapper.Map<IEnumerable<PurchaseOrderToReturnDTO>>(purchaseOrders);
        }

        public async Task<IEnumerable<PurchaseOrderToReturnDTO>> GetAllPurchaseOrdersPagedAsync(int pageNumber, int pageSize)
        {
            var purchaseOrders = await _dbContext.PurchaseOrders
                                .Include(p => p.Branch)
                                .Include(p => p.Supplier)
                                .Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync();

            return _mapper.Map<IEnumerable<PurchaseOrderToReturnDTO>>(purchaseOrders);
        }

        public async Task<PurchaseOrderToReturnDTO> GetPurchaseOrderByIDAsync(int purchaseOrderId)
        {
            var specifiedPurchaseOrder = await _dbContext.PurchaseOrders
                                         .Include(p=>p.Branch)
                                         .Include(p=>p.Supplier)
                                         .FirstOrDefaultAsync(p=>p.ID ==  purchaseOrderId);

            if (specifiedPurchaseOrder is null)
                throw new NotFoundException("This purchase order isn't exist.");

            return _mapper.Map<PurchaseOrderToReturnDTO>(specifiedPurchaseOrder);
        }
        public async Task<IEnumerable<PurchaseOrderToReturnDTO>> GetBranchPurchaseOrdersAsync(int branchId)
        {
            var specifiedBranch = await _dbContext.Branches
                                  .Include(b=>b.PurchaseOrders)
                                  .ThenInclude(b=>b.Supplier)
                                  .FirstOrDefaultAsync(b=>b.ID ==  branchId);

            if (specifiedBranch is null)
                throw new NotFoundException("This branch isn't exist.");

            var purchaseOrders = specifiedBranch.PurchaseOrders;

            return _mapper.Map<IEnumerable<PurchaseOrderToReturnDTO>>(purchaseOrders);
        }
        public async Task<IEnumerable<PurchaseOrderToReturnDTO>> GetSupplierPurchaseOrdersAsync(int supplierId)
        {
            var specifiedSupplier = await _dbContext.Suppliers
                                 .Include(b => b.PurchaseOrders)
                                 .ThenInclude(b => b.Branch)
                                 .FirstOrDefaultAsync(s => s.ID == supplierId);

            if (specifiedSupplier is null)
                throw new NotFoundException("This supplier isn't exist.");

            var purchaseOrders = specifiedSupplier.PurchaseOrders;

            return _mapper.Map<IEnumerable<PurchaseOrderToReturnDTO>>(purchaseOrders);
        }
        public async Task<PurchaseOrderToReturnDTO> AddNewPurchaseOrderAsync(AddNewPurchaseOrderModel addNewPurchaseOrder)
        {
            var specifiedBranch = await _dbContext.Branches.FirstOrDefaultAsync(b => b.ID == addNewPurchaseOrder.BranchID);
            if (specifiedBranch is null)
                throw new NotFoundException("This branch isn't exist.");

            var specifiedSupplier = await _dbContext.Suppliers.FirstOrDefaultAsync(s => s.ID == addNewPurchaseOrder.SupplierID);
            if (specifiedSupplier is null)
                throw new NotFoundException("This supplier isn't exist.");

            var validApprovalStatuses = new[] { "Approved" , "Rejected" , "Pending" };
            if (!validApprovalStatuses.Contains(addNewPurchaseOrder.ApprovalStatus))
                throw new ValidationException("This approval status isn't valid. Valid statuses(Approved , Rejected , and Pending).");

            var validPaymentStatuses = new[] { "Paid", "Cancelled", "Pending" , "Partially Paid" };
            if (!validPaymentStatuses.Contains(addNewPurchaseOrder.PaymentStatus))
                throw new ValidationException("This payment status isn't valid. Valid statuses(Paid , Cancelled , Partially Paid , and Pending).");

            var validPurchaseOrderStatuses = new[] { "Received", "Cancelled", "Pending"};
            if (!validPurchaseOrderStatuses.Contains(addNewPurchaseOrder.PurchaseOrderStatus))
                throw new ValidationException("This purchase order status isn't valid. Valid statuses(Received , Cancelled , and Pending).");

            if (addNewPurchaseOrder.TotalAmount < 0)
                throw new ValidationException("Total amount must be greater than zero.");

            if(addNewPurchaseOrder.OrderDate < DateTime.Now)
                throw new ValidationException("Order date must be after today.");

            if(addNewPurchaseOrder.ExpectedDeliveryDate < addNewPurchaseOrder.OrderDate)
                throw new ValidationException("Expected delivery date must be after order date.");

            var purchaseOrder = new PurchaseOrder
            {
                ApprovalStatus = addNewPurchaseOrder.ApprovalStatus,
                PaymentStatus = addNewPurchaseOrder.PaymentStatus,
                PurchaseOrderStatus = addNewPurchaseOrder.PurchaseOrderStatus,
                TotalAmount = addNewPurchaseOrder.TotalAmount,
                OrderDate = addNewPurchaseOrder.OrderDate,
                ExpectedDeliveryDate = addNewPurchaseOrder.ExpectedDeliveryDate,
                BranchID = addNewPurchaseOrder.BranchID,
                SupplierID = addNewPurchaseOrder.SupplierID
            };

            await _dbContext.PurchaseOrders.AddAsync(purchaseOrder);
            await _dbContext.SaveChangesAsync();

            return _mapper.Map<PurchaseOrderToReturnDTO>(purchaseOrder);
        }

        public async Task<string> DeletePurchaseOrderByIDAsync(int purchaseOrderId)
        {
            var specifiedPurchaseOrder = await _dbContext.PurchaseOrders
                                         .Include(p => p.Branch)
                                         .Include(p => p.Supplier)
                                         .FirstOrDefaultAsync(p => p.ID == purchaseOrderId);

            if (specifiedPurchaseOrder is null)
                throw new NotFoundException("This purchase order isn't exist.");

            _dbContext.PurchaseOrders.Remove(specifiedPurchaseOrder);
            await _dbContext.SaveChangesAsync();

            return "Purchase order deleted successfully.";
        }


        public async Task<PurchaseOrderToReturnDTO> UpdatePurchaseOrderInformationAsync(int purchaseOrderId, UpdatePurchaseOrderInformationModel updatePurchaseOrderInformation)
        {
            var specifiedPurchaseOrder = await _dbContext.PurchaseOrders
                                         .Include(p => p.Branch)
                                         .Include(p => p.Supplier)
                                         .FirstOrDefaultAsync(p => p.ID == purchaseOrderId);

            if (specifiedPurchaseOrder is null)
                throw new NotFoundException("This purchase order isn't exist.");

            var specifiedBranch = await _dbContext.Branches.FirstOrDefaultAsync(b => b.ID == updatePurchaseOrderInformation.BranchID);
            if (specifiedBranch is null)
                throw new NotFoundException("This branch isn't exist.");

            var specifiedSupplier = await _dbContext.Suppliers.FirstOrDefaultAsync(s => s.ID == updatePurchaseOrderInformation.SupplierID);
            if (specifiedSupplier is null)
                throw new NotFoundException("This supplier isn't exist.");

            if (updatePurchaseOrderInformation.TotalAmount < 0)
                throw new ValidationException("Total amount must be greater than zero.");

            if (updatePurchaseOrderInformation.OrderDate < DateTime.Now)
                throw new ValidationException("Order date must be after today.");

            if (updatePurchaseOrderInformation.ExpectedDeliveryDate < updatePurchaseOrderInformation.OrderDate)
                throw new ValidationException("Expected delivery date must be after order date.");

            specifiedPurchaseOrder.TotalAmount = updatePurchaseOrderInformation.TotalAmount;
            specifiedPurchaseOrder.OrderDate = updatePurchaseOrderInformation.OrderDate;
            specifiedPurchaseOrder.ExpectedDeliveryDate = updatePurchaseOrderInformation.ExpectedDeliveryDate;
            specifiedPurchaseOrder.BranchID = updatePurchaseOrderInformation.BranchID;
            specifiedPurchaseOrder.SupplierID = updatePurchaseOrderInformation.SupplierID;

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<PurchaseOrderToReturnDTO>(specifiedPurchaseOrder);
        }

        public async Task<PurchaseOrderToReturnDTO> UpdatePurchaseOrderStatusAsync(int purchaseOrderId, string purchaseOrderStatus)
        {
            var specifiedPurchaseOrder = await _dbContext.PurchaseOrders
                                         .Include(p => p.Branch)
                                         .Include(p => p.Supplier)
                                         .FirstOrDefaultAsync(p => p.ID == purchaseOrderId);

            if (specifiedPurchaseOrder is null)
                throw new NotFoundException("This purchase order isn't exist.");

            var validPurchaseOrderStatuses = new[] { "Received", "Cancelled", "Pending" };
            if (!validPurchaseOrderStatuses.Contains(purchaseOrderStatus))
                throw new ValidationException("This purchase order status isn't valid. Valid statuses(Received , Cancelled , and Pending).");

            specifiedPurchaseOrder.PurchaseOrderStatus = purchaseOrderStatus;

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<PurchaseOrderToReturnDTO>(specifiedPurchaseOrder);
        }

        public async Task<PurchaseOrderToReturnDTO> UpdateApprovalStatusAsync(int purchaseOrderId, string approvalStatus)
        {
            var specifiedPurchaseOrder = await _dbContext.PurchaseOrders
                                         .Include(p => p.Branch)
                                         .Include(p => p.Supplier)
                                         .FirstOrDefaultAsync(p => p.ID == purchaseOrderId);

            if (specifiedPurchaseOrder is null)
                throw new NotFoundException("This purchase order isn't exist.");

            var validApprovalStatuses = new[] { "Approved", "Rejected", "Pending" };
            if (!validApprovalStatuses.Contains(approvalStatus))
                throw new ValidationException("This approval status isn't valid. Valid statuses(Approved , Rejected , and Pending).");


            specifiedPurchaseOrder.ApprovalStatus = approvalStatus;

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<PurchaseOrderToReturnDTO>(specifiedPurchaseOrder);
        }

        public async Task<PurchaseOrderToReturnDTO> UpdatePaymentStatusAsync(int purchaseOrderId, string paymentStatus)
        {
            var specifiedPurchaseOrder = await _dbContext.PurchaseOrders
                                        .Include(p => p.Branch)
                                        .Include(p => p.Supplier)
                                        .FirstOrDefaultAsync(p => p.ID == purchaseOrderId);

            if (specifiedPurchaseOrder is null)
                throw new NotFoundException("This purchase order isn't exist.");

            var validPaymentStatuses = new[] { "Paid", "Cancelled", "Pending", "Partially Paid" };
            if (!validPaymentStatuses.Contains(paymentStatus))
                throw new ValidationException("This payment status isn't valid. Valid statuses(Paid , Cancelled , Partially Paid , and Pending).");


            specifiedPurchaseOrder.PaymentStatus = paymentStatus;

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<PurchaseOrderToReturnDTO>(specifiedPurchaseOrder);
        }
    }
}
