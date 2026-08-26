using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Prescription_Medicine
    {
        public int PrescriptionID { get; set; } // ==> FK
        public Prescription Prescription { get; set; } // ==> Navigation Property
        public int MedicineID { get; set; } // ==> FK
        public Medicine Medicine { get; set; } // ==> Navigation Property
    }
}
