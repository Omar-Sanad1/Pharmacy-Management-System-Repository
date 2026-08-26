using AutoMapper;
using Core.DTOs;
using Core.Entities;

namespace PharmacyManagementSystem.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Batch, BatchToReturnDTO>()
                .ForMember(b => b.MedicineName, b => b.MapFrom(b => b.Medicine.MedicineName));
            ////////////////////////////////////////////////////////////////////////////////////
            CreateMap<Branch, BranchToReturnDTO>();
            ////////////////////////////////////////////////////////////////////////////////////
            CreateMap<Customer, CustomerToReturnDTO>();
            ////////////////////////////////////////////////////////////////////////////////////
            CreateMap<Doctor, DoctorToReturnDTO>()
                .ForMember(d => d.BranchName, d => d.MapFrom(d => d.Branch.BranchName));
            ////////////////////////////////////////////////////////////////////////////////////
            CreateMap<Employee, EmployeeToReturnDTO>()
                .ForMember(e => e.BranchName, e => e.MapFrom(e => e.Branch.BranchName));
            ////////////////////////////////////////////////////////////////////////////////////
            CreateMap<Medicine, MedicineToReturnDTO>();
            ////////////////////////////////////////////////////////////////////////////////////
            CreateMap<Prescription, PrescriptionToReturnDTO>()
                .ForMember(p => p.CustomerName, p => p.MapFrom(p => p.Customer.FullName))
                .ForMember(p => p.DoctorName, p => p.MapFrom(p => p.Doctor.FullName))
                .ForMember(p => p.EmployeeName, p => p.MapFrom(p => p.Employee.FullName));
            ////////////////////////////////////////////////////////////////////////////////////
            CreateMap<PurchaseOrder, PurchaseOrderToReturnDTO>()
                .ForMember(p => p.BranchName, p => p.MapFrom(p => p.Branch.BranchName))
                .ForMember(p => p.SupplierName, p => p.MapFrom(p => p.Supplier.CompanyName));
            ////////////////////////////////////////////////////////////////////////////////////
            CreateMap<Role, RoleToReturnDTO>();
            ////////////////////////////////////////////////////////////////////////////////////
            CreateMap<Sale, SaleToReturnDTO>()
                .ForMember(s => s.CustomerName, s => s.MapFrom(s => s.Customer.FullName))
                .ForMember(s => s.BranchName, s => s.MapFrom(s => s.Branch.BranchName))
                .ForMember(s => s.EmployeeName, s => s.MapFrom(s => s.Employee.FullName));
            ////////////////////////////////////////////////////////////////////////////////////
            CreateMap<Supplier, SupplierToReturnDTO>();
            ////////////////////////////////////////////////////////////////////////////////////
            CreateMap<User, UserToReturnDTO>();
        }
    }
}
