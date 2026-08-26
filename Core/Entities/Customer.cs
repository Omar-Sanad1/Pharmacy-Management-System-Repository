using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Customer : BaseEntity
    {
        public string FullName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string MedicalNotes { get; set; }
        public string AccountStatus { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime RegistrationDate { get; set; }
        public int UserID { get; set; } // ==> FK
        public User User { get; set; } // ==> Navigation Property
        public List<Prescription> Prescriptions { get; set; } = new();
        public List<Customer_Medicine> customer_Medicines { get; set; } = new();
        public List<Sale> Sales { get; set; } = new();
    }
}
