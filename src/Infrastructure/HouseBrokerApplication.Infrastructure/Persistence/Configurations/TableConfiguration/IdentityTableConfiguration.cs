using HouseBrokerApplication.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseBrokerApplication.Infrastructure.Persistence.Configurations.TableConfiguration
{
    public static class IdentityTableConfiguration
    {
        public static void ConfigureIdentityTables(this ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>().ToTable("Users", schema: "Identity");
            builder.Entity<IdentityRole<Guid>>().ToTable("Roles", schema: "Identity");
            builder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles", schema: "Identity");
            builder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims", schema: "Identity");
            builder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins", schema: "Identity");
            builder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims", schema: "Identity");
            builder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens", schema: "Identity");

        }
    }
}
