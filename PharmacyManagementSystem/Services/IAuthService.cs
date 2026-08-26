using PharmacyManagementSystem.Models;

namespace PharmacyManagementSystem.Services
{
    public interface IAuthService
    {
        public Task<string> RegisterCustomerAsync(RegisterCustomerModel registerCustomer);
        public Task<string> RegisterEmployeeAsync(RegisterEmployeeModel registerEmployee);
        public Task<string> LoginAsync(LoginModel loginModel);
    }
}
