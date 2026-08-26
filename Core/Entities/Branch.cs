using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Branch : BaseEntity
    {
        public string BranchName { get; set; }
        public string BranchCode { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string Location { get; set; }
        public string Manager { get; set; }
        public string OperationalStatus { get; set; }
        public int OperatingHours { get; set; }
        public List<Doctor> Doctors { get; set; } = new();
        public List<PurchaseOrder> PurchaseOrders { get; set; } = new();
        public List<Employee> Employees { get; set; } = new();
        public List<Sale> Sales { get; set; } = new();
    }
}
