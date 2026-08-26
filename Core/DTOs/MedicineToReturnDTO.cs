using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class MedicineToReturnDTO
    {
        public string MedicineName { get; set; }
        public string ScientificName { get; set; }
        public string BrandName { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string DosageForm { get; set; }
        public string Strength { get; set; }
        public string Barcode { get; set; }
        public string Manufacturer { get; set; }
        public string PrescriptionRequirement { get; set; }
        public string AvailabilityStatus { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal PurchasePrice { get; set; }
        public int MinimumStockLevel { get; set; }
    }
}
