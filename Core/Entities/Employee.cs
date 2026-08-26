using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Employee : BaseEntity
    {
        public string FullName { get; set; }
        public string EmployeePosition { get; set; }
        public string LicenseNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string Shift { get; set; }
        public string EmploymentStatus { get; set; }
        public decimal Salary { get; set; }
        public DateTime HiringDate { get; set; }
        public int UserID { get; set; } // ==> FK
        public int BranchID { get; set; } // ==> FK
        public User User { get; set; } // ==> Navigation Property
        public Branch Branch { get; set; } // ==> Navigation Property
        public List<Prescription> Prescriptions { get; set; } = new();
        public List<Sale> Sales { get; set; } = new();

    }
}
