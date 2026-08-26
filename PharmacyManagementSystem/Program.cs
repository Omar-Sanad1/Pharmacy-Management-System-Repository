using Core.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PharmacyManagementSystem.CreateTokenService;
using PharmacyManagementSystem.ExceptionMiddlewares;
using PharmacyManagementSystem.Helper;
using PharmacyManagementSystem.Services;
using Repository;
using Repository.Context;
using Repository.Repository;
using Service.Interfaces;
using Service.Services;
using System.Text;

namespace PharmacyManagementSystem
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter JWT Token"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            builder.Services.AddDbContext<PharmacyManagementDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("PharmacyConnection")));

            builder.Services.AddAutoMapper(typeof(MappingProfile));

            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped(typeof(ITokenService), typeof(TokenService));
            builder.Services.AddScoped(typeof(IAuthService), typeof(AuthService));

            builder.Services.AddScoped(typeof(IBatchService), typeof(BatchService));
            builder.Services.AddScoped(typeof(IBranchService), typeof(BranchService));
            builder.Services.AddScoped(typeof(ICustomerService), typeof(CustomerService));
            builder.Services.AddScoped(typeof(IDoctorService), typeof(DoctorService));
            builder.Services.AddScoped(typeof(IEmployeeService), typeof(EmployeeService));
            builder.Services.AddScoped(typeof(IMedicineService), typeof(MedicineService));
            builder.Services.AddScoped(typeof(IPrescriptionService), typeof(PrescriptionService));
            builder.Services.AddScoped(typeof(IPurchaseOrderService), typeof(PurchaseOrderService));
            builder.Services.AddScoped(typeof(ISaleService), typeof(SaleService));
            builder.Services.AddScoped(typeof(ISupplierService), typeof(SupplierService));
            builder.Services.AddScoped(typeof(IUserService), typeof(UserService));


            builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,

                    ValidIssuer = builder.Configuration["JWT:Issuer"],
                    ValidAudience = builder.Configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
                };
            });

            var app = builder.Build();

            using(var scope = app.Services.CreateScope())
            {
                var service = scope.ServiceProvider;
                var LoggerFactory = service.GetRequiredService<ILoggerFactory>();
                var dbContext = service.GetRequiredService<PharmacyManagementDbContext>();
                try
                {
                    await dbContext.Database.MigrateAsync();
                    await PharmacySystemSeeding.SeedAsync(dbContext);
                }
                catch(Exception ex)
                {
                    var logger = LoggerFactory.CreateLogger<Program>();
                    logger.LogError(ex, "Error happen until migration");
                }
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();


            app.UseMiddleware<ExceptionMiddleware>();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
