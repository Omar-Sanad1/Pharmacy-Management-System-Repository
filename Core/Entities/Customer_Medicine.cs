using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Customer_Medicine
    {
        public int CustomerID { get; set; } // ==> FK
        public Customer Customer { get; set; } // ==> Navigation Property
        public int MedicineID { get; set; } // ==> FK
        public Medicine Medicine { get; set; } // ==> Navigation Property
    }
}
