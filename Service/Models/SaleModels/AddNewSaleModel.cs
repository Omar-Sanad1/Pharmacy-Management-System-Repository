using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models.SaleModels
{
    public class AddNewSaleModel
    {
        public decimal Subtotal { get; set; }
        public decimal Discount { get; set; }
        public decimal Taxes { get; set; }
        public string PaymentStatus { get; set; }
        public string SaleStatus { get; set; }
        public DateTime SaleDate { get; set; }
        public int CustomerID { get; set; } 
        public int BranchID { get; set; } 
        public int EmployeeID { get; set; } 
    }
}
