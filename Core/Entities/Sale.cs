using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Sale : BaseEntity
    {
        public decimal Subtotal { get; set; }
        public decimal Discount { get; set; }
        public decimal Taxes { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentStatus { get; set; }
        public string SaleStatus { get; set; }
        public DateTime SaleDate { get; set; }
        public int CustomerID { get; set; } // ==> FK
        public int BranchID { get; set; } // ==> FK
        public int EmployeeID { get; set; } // ==> FK
        public Customer Customer { get; set; } // ==> Navigation Property
        public Branch Branch { get; set; } // ==> Navigation Property
        public Employee Employee { get; set; } // ==> Navigation Property

    }
}
