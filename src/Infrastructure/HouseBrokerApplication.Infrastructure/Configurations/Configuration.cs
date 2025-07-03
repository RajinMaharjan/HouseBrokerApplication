using HouseBrokerApplication.Application.Interface;
using HouseBrokerApplication.Domain.Entities;
using HouseBrokerApplication.Infrastructure.Persistence;
using HouseBrokerApplication.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseBrokerApplication.Infrastructure.Configurations
{
    public static class Configuration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            //Adding DbContext
            var connectionString = configuration.GetConnectionString("DbConnection");
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

            //Adding Services
            services.AddScoped<ICommissionService, CommissionService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IHouseListingService, HouseListingService>();

            //Adding Users with Roles
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders(); 
            return services;
        }
        
    }
}
