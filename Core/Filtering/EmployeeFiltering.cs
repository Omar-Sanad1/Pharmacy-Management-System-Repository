using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Filtering
{
    public class EmployeeFiltering
    {
        public string? Shift { get; set; }
        public string? EmploymentStatus { get; set; }
        public DateTime? HiringDate { get; set; }

        // Sorting
        public string? SortBy { get; set; }
        public bool isDescending { get; set; }
    }
}
