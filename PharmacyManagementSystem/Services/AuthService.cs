using Core.Entities;
using Core.Exceptions;
using Microsoft.EntityFrameworkCore;
using PharmacyManagementSystem.CreateTokenService;
using PharmacyManagementSystem.Models;
using Repository.Context;

namespace PharmacyManagementSystem.Services
{
    public class AuthService : IAuthService
    {
        private readonly PharmacyManagementDbContext _dbContext;
        private readonly ITokenService _tokenService;
        public AuthService(PharmacyManagementDbContext dbContext, ITokenService tokenService)
        {
            _dbContext = dbContext;
            _tokenService = tokenService;
        }
        public async Task<string> RegisterCustomerAsync(RegisterCustomerModel registerCustomer)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var existedUserName = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == registerCustomer.UserName);
                if (existedUserName is not null)
                    throw new ValidationException("This user is already exist.");

                var existedEmail = await _dbContext.Users.FirstOrDefaultAsync(u => u.EmailAddress == registerCustomer.EmailAddress);
                if (existedEmail is not null)
                    throw new ValidationException("This email is already exist.");

                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerCustomer.PasswordHash);

                var user = new User
                {
                    UserName = registerCustomer.UserName,
                    EmailAddress = registerCustomer.EmailAddress,
                    PasswordHash = hashedPassword,
                    CreatedAt = DateTime.Now,
                    RoleID = 8
                };

                await _dbContext.Users.AddAsync(user);
                await _dbContext.SaveChangesAsync();

                var customer = new Customer
                {
                    FullName = registerCustomer.FullName,
                    Address = registerCustomer.Address,
                    PhoneNumber = registerCustomer.PhoneNumber,
                    MedicalNotes = registerCustomer.MedicalNotes,
                    AccountStatus = "Active",
                    DateOfBirth = registerCustomer.DateOfBirth,
                    RegistrationDate = DateTime.Now,
                    UserID = user.ID
                };

                await _dbContext.Customers.AddAsync(customer);
                await _dbContext.SaveChangesAsync();

                await transaction.CommitAsync();

                return "Customer registered successfully.";
            }

            catch(Exception ex)
            {
                await transaction.RollbackAsync();
                return ex.Message;
            }
        }

        public async Task<string> RegisterEmployeeAsync(RegisterEmployeeModel registerEmployee)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var existedUserName = await _dbContext.Users.FirstOrDefaultAsync(u=>u.UserName == registerEmployee.UserName);
                if (existedUserName is not null)
                    throw new ValidationException("This user is already exist.");

                var existedEmail = await _dbContext.Users.FirstOrDefaultAsync(u => u.EmailAddress == registerEmployee.EmailAddress);
                if (existedEmail is not null)
                    throw new ValidationException("This email is already exist.");

                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerEmployee.PasswordHash);

                var user = new User
                {
                    UserName = registerEmployee.UserName,
                    EmailAddress = registerEmployee.EmailAddress,
                    PasswordHash = hashedPassword,
                    CreatedAt = DateTime.Now,
                    RoleID = registerEmployee.RoleID
                };

                await _dbContext.Users.AddAsync(user);
                await _dbContext.SaveChangesAsync();

                var employee = new Employee
                {
                    FullName = registerEmployee.FullName,
                    EmployeePosition = registerEmployee.EmployeePosition,
                    LicenseNumber = registerEmployee.LicenseNumber,
                    PhoneNumber = registerEmployee.PhoneNumber,
                    Shift = registerEmployee.Shift,
                    EmploymentStatus = "Active",
                    Salary = registerEmployee.Salary,
                    HiringDate = registerEmployee.HiringDate,
                    BranchID = registerEmployee.BranchID,
                    UserID = user.ID
                };

                await _dbContext.Employees.AddAsync(employee);
                await _dbContext.SaveChangesAsync();

                await transaction.CommitAsync();

                return "Employee registered successfully";
            }

            catch(Exception ex)
            {
                await transaction.RollbackAsync();
                return ex.Message;
            }
        }

        public async Task<string> LoginAsync(LoginModel loginModel)
        {
            var checkEmail = await _dbContext.Users
                            .Include(u=>u.Role)
                            .FirstOrDefaultAsync(u => u.EmailAddress == loginModel.EmailAddress);

            if (checkEmail is null)
                throw new ValidationException("This email or password isn't correct.");

            var checkPassword = BCrypt.Net.BCrypt.Verify(loginModel.Password, checkEmail.PasswordHash);
            if(!checkPassword)
                throw new ValidationException("This email or password isn't correct.");

            var token = await _tokenService.CreateTokenAsync(checkEmail);

            return $"Token : {token}";
        }

    }
}
