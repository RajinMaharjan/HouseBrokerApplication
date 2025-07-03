using HouseBrokerApplication.Domain.Entities;
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
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<HouseListing> HouseListings { get; set; }
        public DbSet<ListingImage> ListingImages { get; set; }
        public DbSet<CommissionRate> CommissionRates { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<CommissionRate>().HasData(
                new CommissionRate { Id = 1, MinPrice = 0, MaxPrice = 500, Percentage = 0.02 },
                new CommissionRate { Id = 2, MinPrice = 500, MaxPrice = 1000, Percentage = 0.0175 },
                new CommissionRate { Id = 3, MinPrice = 1000, MaxPrice = decimal.MaxValue, Percentage = 0.015 }
            );

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "1", Name = "Broker", NormalizedName = "BROKER" },
                new IdentityRole { Id = "2", Name = "Seeker", NormalizedName = "SEEKER" }
            );

            var hasher = new PasswordHasher<ApplicationUser>();

            var broker1 = new ApplicationUser
            {
                Id = "Broker 1",
                UserName = "milan.chapagain@gmail.com",
                NormalizedUserName = "MILAN.CHAPAGAIN@GMAIL.COM",
                Email = "milan.chapagain@gmail.com",
                NormalizedEmail = "MILAN.CHAPAGAIN@GMAIL.COM",
                EmailConfirmed = true,
                FullName = "Milan Chapagain",
                Role = "Broker",
                PasswordHash = hasher.HashPassword(null, "M@m123456")
            };

            var seeker1 = new ApplicationUser
            {
                Id = "Seeker 1",
                UserName = "ram.shrestha@gmail.com",
                NormalizedUserName = "RAM.SHRESTHA@GMAIL.COM",
                Email = "ram.shrestha@gmail.com",
                NormalizedEmail = "RAM.SHRESTHA@GMAIL.COM",
                EmailConfirmed = true,
                FullName = "Ram Shrestha",
                Role = "Seeker",
                PasswordHash = hasher.HashPassword(null, "R@r123456")
            };

            builder.Entity<ApplicationUser>().HasData(broker1, seeker1);

            // Seed Listings
            var listing1 = new HouseListing
            {
                Id = Guid.NewGuid(),
                Title = "House in Kathmandu",
                Location = "Kathmandu",
                Price = 7500000,
                PropertyType = "House",
                Description = "2 BHK modern apartment in heart of the city.",
                BrokerId = broker1.Id,
                Commission = 131250m
            };

            var listing2 = new HouseListing
            {
                Id = Guid.NewGuid(),
                Title = "Luxury Villa in Kathmandu",
                Location = "Kathmandu",
                Price = 12500000,
                PropertyType = "Villa",
                Description = "Luxurious villa with city view.",
                BrokerId = broker1.Id,
                Commission = 187500m
            };

            builder.Entity<HouseListing>().HasData(listing1, listing2);

            // Seed Images
            var image1 = new ListingImage
            {
                Id = Guid.NewGuid(),
                Url = "https://www.pexels.com/photo/brown-and-gray-painted-house-in-front-of-road-1396122/",
                HouseListingId = listing1.Id
            };

            var image2 = new ListingImage
            {
                Id = Guid.NewGuid(),
                Url = "https://www.pexels.com/photo/house-lights-turned-on-106399/",
                HouseListingId = listing2.Id
            };

            builder.Entity<ListingImage>().HasData(image1, image2);

            builder.Entity<HouseListing>()
                .Property(h => h.Price)
                .HasColumnType("decimal(18,2)");

            builder.Entity<HouseListing>()
                .Property(h => h.Commission)
                .HasColumnType("decimal(18,2)");
        }
    }
}
