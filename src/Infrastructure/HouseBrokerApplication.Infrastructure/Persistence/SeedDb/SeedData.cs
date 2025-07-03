using HouseBrokerApplication.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseBrokerApplication.Infrastructure.Persistence.SeedDb
{
    public class SeedData
    {
        private static Guid brokerId = Guid.NewGuid();
        private static Guid seekerId = Guid.NewGuid();

        private static Guid listingId1 = Guid.NewGuid();
        private static Guid listingId2 = Guid.NewGuid();

        public static void SeedRate(ModelBuilder builder)
        {
            //directly seed data
            builder.Entity<CommissionRate>().HasData(
                new CommissionRate { Id = 1, MinPrice = 0, MaxPrice = 500, Percentage = 0.02 },
                new CommissionRate { Id = 2, MinPrice = 500, MaxPrice = 1000, Percentage = 0.0175 },
                new CommissionRate { Id = 3, MinPrice = 1000, MaxPrice = 2000, Percentage = 0.015 }
            );
        }

        public static void SeedIdentityRole(ModelBuilder builder)
        {
            //directly seed data
            builder.Entity<ApplicationRole>().HasData(
                new ApplicationRole { Id = Guid.NewGuid(), Name = "Broker", NormalizedName = "BROKER" },
                new ApplicationRole { Id = Guid.NewGuid(), Name = "Seeker", NormalizedName = "SEEKER" }
            );
        }

        public static void SeedUser(ModelBuilder builder)
        {
            var hasher = new PasswordHasher<ApplicationUser>();

            var broker1 = new ApplicationUser
            {
                Id = brokerId,
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
                Id = seekerId,
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
        }
        public static void SeedHouseImages(ModelBuilder builder)
        {
            var image1 = new ListingImage
            {
                Id = Guid.NewGuid(),
                Url = "https://www.pexels.com/photo/brown-and-gray-painted-house-in-front-of-road-1396122/",
                HouseListingId = listingId1
            };

            var image2 = new ListingImage
            {
                Id = Guid.NewGuid(),
                Url = "https://www.pexels.com/photo/house-lights-turned-on-106399/",
                HouseListingId = listingId2
            };

            builder.Entity<ListingImage>().HasData(image1, image2);
        }
        public static void SeedHouseListing(ModelBuilder builder)
        {
            var listing1 = new HouseListing
            {
                Id = listingId1,
                Title = "House in Kathmandu",
                Location = "Kathmandu",
                Price = 750,
                PropertyType = "House",
                Description = "2 BHK modern apartment in heart of the city.",
                BrokerId = brokerId,
                Commission = 131
            };

            var listing2 = new HouseListing
            {
                Id = listingId2,
                Title = "Luxury Villa in Kathmandu",
                Location = "Kathmandu",
                Price = 1250,
                PropertyType = "Villa",
                Description = "Luxurious villa with city view.",
                BrokerId = brokerId,
                Commission = 187
            };

            builder.Entity<HouseListing>().HasData(listing1, listing2);
        }
    }
}
