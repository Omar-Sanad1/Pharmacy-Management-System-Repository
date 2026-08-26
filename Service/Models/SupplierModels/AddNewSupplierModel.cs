using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models.SupplierModels
{
    public class AddNewSupplierModel
    {
        public string CompanyName { get; set; }
        public string ContactPerson { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string Address { get; set; }
        public string TaxInformation { get; set; }
        public string PaymentTerms { get; set; }
        public string SupplierStatus { get; set; }
        public int SupplierRating { get; set; }
    }
}
