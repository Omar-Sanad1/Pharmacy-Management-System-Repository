using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class EmployeeToReturnDTO
    {
        public string FullName { get; set; }
        public string EmployeePosition { get; set; }
        public string LicenseNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string Shift { get; set; }
        public string EmploymentStatus { get; set; }
        public DateTime HiringDate { get; set; }
        public int UserID { get; set; } 
        public string BranchName { get; set; } 
    }
}
