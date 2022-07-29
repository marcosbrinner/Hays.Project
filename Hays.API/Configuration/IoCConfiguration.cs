using FluentValidation;
using Hays.Data.DataAccess;
using Hays.Data.Repositories;
using Hays.Domain.Abstraction.DataAcess;
using Hays.Domain.Abstraction.Repository;
using Hays.Domain.Abstraction.Services;
using Hays.Domain.Entities;
using Hays.Domain.Services;
using Hays.Domain.Validators;
using Microsoft.EntityFrameworkCore;

namespace Hays.API.Configuration
{
    public static class IoCConfiguration
    {
        public static void AddHaysDependency(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<Context>(x =>
            {
                x.UseSqlServer(config.GetConnectionString("HaysTest"));
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICustomersRepository, CustomersRepository>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IValidator<Customers>, CustomerValidator>();
        }
    }
}
