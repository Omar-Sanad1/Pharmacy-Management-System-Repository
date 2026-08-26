using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models.PurchaseOrderModels
{
    public class UpdatePurchaseOrderInformationModel
    {
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ExpectedDeliveryDate { get; set; }
        public int SupplierID { get; set; } 
        public int BranchID { get; set; } 
    }
}
