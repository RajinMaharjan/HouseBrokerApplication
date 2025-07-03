using HouseBrokerApplication.Domain.Entities;
using HouseBrokerApplication.Infrastructure.Persistence.Configurations.TableConfiguration;
using HouseBrokerApplication.Infrastructure.Persistence.SeedDb;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace HouseBrokerApplication.Infrastructure.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole,Guid>
    {
        public DbSet<HouseListing> HouseListings { get; set; }
        public DbSet<ListingImage> ListingImages { get; set; }
        public DbSet<CommissionRate> CommissionRates { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //seed data
            SeedData.SeedIdentityRole(builder);
            SeedData.SeedRate(builder);
            SeedData.SeedUser(builder);
            SeedData.SeedHouseListing(builder);
            SeedData.SeedHouseImages(builder); 
            
            //configure table
            builder.ConfigureIdentityTables();
            builder.ConfigureTables();

        }
    }
}
