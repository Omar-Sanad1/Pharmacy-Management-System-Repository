using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models.SaleModels
{
    public class UpdateSaleInformationModel
    {
        public DateTime SaleDate { get; set; }
        public int CustomerID { get; set; }
        public int BranchID { get; set; } 
        public int EmployeeID { get; set; } 
    }
}
