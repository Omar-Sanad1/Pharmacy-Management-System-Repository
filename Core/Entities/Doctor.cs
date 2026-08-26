using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Doctor : BaseEntity
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Specialization { get; set; }
        public string LicenseNumber { get; set; }
        public string MedicalFacility { get; set; }
        public string Status { get; set; }
        public int BranchID { get; set; } // ==> FK
        public Branch Branch { get; set; } // ==> Navigation Property
        public List<Prescription> Prescriptions { get; set; } = new();
    }
}
