using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models.PrescriptionModels
{
    public class UpdatePrescriptionInformationModel
    {
        public string Notes { get; set; }
        public DateTime PrescriptionDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public int DoctorID { get; set; } 
        public int EmployeeID { get; set; } 
        public int CustomerID { get; set; } 
    }
}
