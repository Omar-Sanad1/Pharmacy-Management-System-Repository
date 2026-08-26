using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class PrescriptionToReturnDTO
    {
        public string PrescriptionStatus { get; set; }
        public string Notes { get; set; }
        public DateTime PrescriptionDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string DoctorName { get; set; } 
        public string EmployeeName { get; set; } 
        public string CustomerName { get; set; } 
    }
}
