using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class PurchaseOrderToReturnDTO
    {
        public string ApprovalStatus { get; set; }
        public string PaymentStatus { get; set; }
        public string PurchaseOrderStatus { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ExpectedDeliveryDate { get; set; }
        public string SupplierName { get; set; } 
        public string BranchName { get; set; } 
    }
}
