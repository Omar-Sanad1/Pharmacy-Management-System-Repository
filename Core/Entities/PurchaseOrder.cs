using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class PurchaseOrder : BaseEntity
    {
        public string ApprovalStatus { get; set; }
        public string PaymentStatus { get; set; }
        public string PurchaseOrderStatus { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ExpectedDeliveryDate { get; set; }
        public int SupplierID { get; set; } // ==> FK
        public int BranchID { get; set; } // ==> FK
        public Supplier Supplier { get; set; } // ==> Navigation Property
        public Branch Branch { get; set; } // ==> Navigation Property

    }
}
