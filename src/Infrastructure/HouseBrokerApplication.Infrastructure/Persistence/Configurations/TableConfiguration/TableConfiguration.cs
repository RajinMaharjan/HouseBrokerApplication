using HouseBrokerApplication.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HouseBrokerApplication.Infrastructure.Persistence.Configurations.TableConfiguration
{
    public static class TableConfiguration
    {
        public static void ConfigureTables(this ModelBuilder builder)
        {
            builder.Entity<HouseListing>().ToTable("Houses", schema: "HBA");
            builder.Entity<ListingImage>().ToTable("Images", schema: "HBA");
            builder.Entity<CommissionRate>().ToTable("Rates", schema: "HBA");
        }
    }
}
