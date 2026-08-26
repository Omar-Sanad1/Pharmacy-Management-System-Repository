using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class BatchToReturnDTO
    {
        public string BatchNumber { get; set; }
        public string StorageLocation { get; set; }
        public decimal PurchasePrice { get; set; }
        public DateTime ManufacturingDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int AvailableQuantity { get; set; }
        public string MedicineName { get; set; }
    }
}
