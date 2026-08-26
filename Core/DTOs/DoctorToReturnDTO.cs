using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class DoctorToReturnDTO
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Specialization { get; set; }
        public string LicenseNumber { get; set; }
        public string MedicalFacility { get; set; }
        public string Status { get; set; }
        public string BranchName { get; set; } 
    }
}
