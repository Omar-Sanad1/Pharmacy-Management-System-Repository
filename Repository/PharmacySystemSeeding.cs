using Core.Entities;
using Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Repository
{
    public class PharmacySystemSeeding
    {
        public static async Task SeedAsync(PharmacyManagementDbContext dbContext)
        {
            // Branches
            if (!dbContext.Branches.Any())
            {
                var branches = File.ReadAllText("../Repository/DataSeed/branches.json");
                var branchesData = JsonSerializer.Deserialize<List<Branch>>(branches);
                if(branchesData?.Count > 0)
                {
                    foreach(var branch in branchesData)
                    {
                        await dbContext.Branches.AddAsync(branch);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }

            // Suppliers
            if (!dbContext.Suppliers.Any())
            {
                var suppliers = File.ReadAllText("../Repository/DataSeed/suppliers.json");
                var suppliersData = JsonSerializer.Deserialize<List<Supplier>>(suppliers);
                if (suppliersData?.Count > 0)
                {
                    foreach (var supplier in suppliersData)
                    {
                        await dbContext.Suppliers.AddAsync(supplier);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }

            // PurchaseOrders
            if (!dbContext.PurchaseOrders.Any())
            {
                var purchaseorders = File.ReadAllText("../Repository/DataSeed/purchaseorders.json");
                var purchaseordersData = JsonSerializer.Deserialize<List<PurchaseOrder>>(purchaseorders);
                if (purchaseordersData?.Count > 0)
                {
                    foreach (var purchaseOrder in purchaseordersData)
                    {
                        await dbContext.PurchaseOrders.AddAsync(purchaseOrder);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }

            // Medicines
            if (!dbContext.Medicines.Any())
            {
                var medicines = File.ReadAllText("../Repository/DataSeed/medicines.json");
                var medicinesData = JsonSerializer.Deserialize<List<Medicine>>(medicines);
                if (medicinesData?.Count > 0)
                {
                    foreach (var medicine in medicinesData)
                    {
                        await dbContext.Medicines.AddAsync(medicine);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }

            // Batches
            if (!dbContext.Batches.Any())
            {
                var batches = File.ReadAllText("../Repository/DataSeed/batches.json");
                var batchesData = JsonSerializer.Deserialize<List<Batch>>(batches);
                if (batchesData?.Count > 0)
                {
                    foreach (var batch in batchesData)
                    {
                        await dbContext.Batches.AddAsync(batch);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }

            // Doctors
            if (!dbContext.Doctors.Any())
            {
                var doctors = File.ReadAllText("../Repository/DataSeed/doctors.json");
                var doctorsData = JsonSerializer.Deserialize<List<Doctor>>(doctors);
                if (doctorsData?.Count > 0)
                {
                    foreach (var doctor in doctorsData)
                    {
                        await dbContext.Doctors.AddAsync(doctor);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }

            // Roles
            if (!dbContext.Roles.Any())
            {
                var roles = File.ReadAllText("../Repository/DataSeed/roles.json");
                var rolesData = JsonSerializer.Deserialize<List<Role>>(roles);
                if (rolesData?.Count > 0)
                {
                    foreach (var role in rolesData)
                    {
                        await dbContext.Roles.AddAsync(role);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }

            // Users
            if (!dbContext.Users.Any())
            {
                var users = File.ReadAllText("../Repository/DataSeed/users.json");
                var usersData = JsonSerializer.Deserialize<List<User>>(users);
                if (usersData?.Count > 0)
                {
                    foreach (var user in usersData)
                    {
                        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
                        await dbContext.Users.AddAsync(user);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }

            // Employees
            if (!dbContext.Employees.Any())
            {
                var employees = File.ReadAllText("../Repository/DataSeed/employees.json");
                var employeesData = JsonSerializer.Deserialize<List<Employee>>(employees);
                if (employeesData?.Count > 0)
                {
                    foreach (var employee in employeesData)
                    {
                        await dbContext.Employees.AddAsync(employee);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }

            // Customers
            if (!dbContext.Customers.Any())
            {
                var customers = File.ReadAllText("../Repository/DataSeed/customers.json");
                var customersData = JsonSerializer.Deserialize<List<Customer>>(customers);
                if (customersData?.Count > 0)
                {
                    foreach (var customer in customersData)
                    {
                        await dbContext.Customers.AddAsync(customer);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }

            // Sales
            if (!dbContext.Sales.Any())
            {
                var sales = File.ReadAllText("../Repository/DataSeed/sales.json");
                var salesData = JsonSerializer.Deserialize<List<Sale>>(sales);
                if (salesData?.Count > 0)
                {
                    foreach (var sale in salesData)
                    {
                        await dbContext.Sales.AddAsync(sale);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }

            // Prescriptions
            if (!dbContext.Prescriptions.Any())
            {
                var prescriptions = File.ReadAllText("../Repository/DataSeed/prescriptions.json");
                var prescriptionsData = JsonSerializer.Deserialize<List<Prescription>>(prescriptions);
                if (prescriptionsData?.Count > 0)
                {
                    foreach (var prescription in prescriptionsData)
                    {
                        await dbContext.Prescriptions.AddAsync(prescription);
                    }
                    await dbContext.SaveChangesAsync();
                }
            }

        }
    }
}
