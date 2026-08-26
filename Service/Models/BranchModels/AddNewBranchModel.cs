using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Models.BranchModels
{
    public class AddNewBranchModel
    {
        public string BranchName { get; set; }
        public string BranchCode { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string Location { get; set; }
        public string Manager { get; set; }
        public int OperatingHours { get; set; }
    }
}
