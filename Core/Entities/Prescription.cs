using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Prescription : BaseEntity
    {
        public string PrescriptionStatus { get; set; }
        public string Notes { get; set; }
        public DateTime PrescriptionDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int DoctorID { get; set; } // ==> FK
        public int EmployeeID { get; set; } // ==> FK
        public int CustomerID { get; set; } // ==> FK
        public Doctor Doctor { get; set; } // ==> Navigation Property
        public Employee Employee { get; set; } // ==> Navigation Property
        public Customer Customer { get; set; } // ==> Navigation Property
        public List<Prescription_Medicine> prescription_Medicines { get; set; } = new();
    }
}
