using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Filtering
{
    public class CustomerFiltering
    {
        public string? Address { get; set; }
        public string? AccountStatus { get; set; }
        public DateTime? RegistrationDate { get; set; }


        // Sorting
        public string? SortBy { get; set; }
        public bool isDescending { get; set; }
    }
}
