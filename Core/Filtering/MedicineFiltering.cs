using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Filtering
{
    public class MedicineFiltering
    {
        public string? Category { get; set; }
        public string? AvailabilityStatus { get; set; }
        public decimal? PurchasePrice { get; set; }

        // Sorting
        public string? SortBy { get; set; }
        public bool isDescending { get; set; }

    }
}
