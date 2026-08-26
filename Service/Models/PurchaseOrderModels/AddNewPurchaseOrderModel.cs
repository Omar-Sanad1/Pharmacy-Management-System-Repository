using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models.PurchaseOrderModels
{
    public class AddNewPurchaseOrderModel
    {
        public string ApprovalStatus { get; set; }
        public string PaymentStatus { get; set; }
        public string PurchaseOrderStatus { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ExpectedDeliveryDate { get; set; }
        public int SupplierID { get; set; } 
        public int BranchID { get; set; } 
    }
}
