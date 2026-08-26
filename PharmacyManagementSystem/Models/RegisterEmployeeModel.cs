namespace PharmacyManagementSystem.Models
{
    public class RegisterEmployeeModel
    {
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string PasswordHash { get; set; }
        public int RoleID { get; set; }

        public string FullName { get; set; }
        public string EmployeePosition { get; set; }
        public string LicenseNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string Shift { get; set; }
        public decimal Salary { get; set; }
        public DateTime HiringDate { get; set; }
        public int BranchID { get; set; } 
    }
}
