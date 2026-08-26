using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Supplier_Medicine
    {
        public int SupplierID { get; set; } // ==> FK
        public Supplier Supplier { get; set; } // ==> Navigation Property
        public int MedicineID { get; set; } // ==> FK
        public Medicine Medicine { get; set; } // ==> Navigation Property
    }
}
