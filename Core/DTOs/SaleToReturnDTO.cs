using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class SaleToReturnDTO
    {
        public decimal Subtotal { get; set; }
        public decimal Discount { get; set; }
        public decimal Taxes { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentStatus { get; set; }
        public string SaleStatus { get; set; }
        public DateTime SaleDate { get; set; }
        public string CustomerName { get; set; } 
        public string BranchName { get; set; } 
        public string EmployeeName { get; set; } 
    }
}
