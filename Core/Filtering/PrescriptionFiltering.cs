using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Filtering
{
    public class PrescriptionFiltering
    {
        public string? PrescriptionStatus { get; set; }

        // Sorting
        public string? SortBy { get; set; }
        public bool isDescending { get; set; }
    }
}
