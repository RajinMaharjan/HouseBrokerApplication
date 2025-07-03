using HouseBrokerApplication.Application.DTOs;
using HouseBrokerApplication.Application.Interface;
using HouseBrokerApplication.Domain.Entities;
using HouseBrokerApplication.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseBrokerApplication.Infrastructure.Services
{
    public class HouseListingService : IHouseListingService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommissionService _commissionService;

        public HouseListingService(ApplicationDbContext context, ICommissionService commissionService)
        {
            _context = context;
            _commissionService = commissionService;
        }

        public async Task<HouseListing> CreateListingAsync(HouseListing listing)
        {
            listing.Commission = _commissionService.CalculateCommission(listing.Price);
            _context.HouseListings.Add(listing);
            await _context.SaveChangesAsync();
            return listing;
        }

        public async Task<IEnumerable<ListingDto>> GetAllListingsAsync()
        {
            return await _context.HouseListings
                .Include(l => l.Broker)
                .Include(l => l.Images)
                .Select(l => new ListingDto
                {
                    Id = l.Id,
                    Title = l.Title,
                    Location = l.Location,
                    Price = l.Price,
                    PropertyType = l.PropertyType,
                    Description = l.Description,
                    BrokerName = l.Broker != null ? l.Broker.UserName : "N/A",
                    ImageUrls = l.Images.Select(img => img.Url).ToList()
                })
                .ToListAsync();
        }

        public async Task<object?> GetListingByIdAsync(Guid id)
        {
            var listing = await _context.HouseListings
                .Include(l => l.Images)
                .Include(l => l.Broker)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (listing == null)
            {
                return null;
            }

            return new
            {
                listing.Id,
                listing.Title,
                listing.Location,
                listing.Price,
                listing.PropertyType,
                listing.Description,
                Images = listing.Images.Select(i => i.Url),
                Broker = listing.Broker == null ? null : new
                {
                    listing.Broker.FullName,
                    listing.Broker.Email,
                    listing.Broker.PhoneNumber
                }
            };
        }

        public async Task<object> SearchListingsAsync(string? location, double? minPrice, double? maxPrice,
            string? propertyType, string sortBy, string sortDirection, int page, int pageSize)
        {
            var query = _context.HouseListings.Include(l => l.Images).AsQueryable();

            if (!string.IsNullOrWhiteSpace(location))
            {
                query = query.Where(l => l.Location.Contains(location));
            }

            if (minPrice.HasValue)
            {
                query = query.Where(l => l.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                query = query.Where(l => l.Price <= maxPrice.Value);
            }

            if (!string.IsNullOrWhiteSpace(propertyType))
            {
                query = query.Where(l => l.PropertyType == propertyType);
            }

            query = (sortBy.ToLower(), sortDirection.ToLower()) switch
            {
                ("price", "desc") => query.OrderByDescending(l => l.Price),
                ("price", "asc") => query.OrderBy(l => l.Price),
                ("location", "desc") => query.OrderByDescending(l => l.Location),
                ("location", "asc") => query.OrderBy(l => l.Location),
                _ => query.OrderBy(l => l.Price)
            };

            var totalCount = await query.CountAsync();
            var listings = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new { totalCount, listings };
        }

        public async Task<bool> UpdateListingAsync(Guid id, HouseListing updatedListing)
        {
            var existingListing = await _context.HouseListings.FindAsync(id);
            if (existingListing == null)
            {
                return false;
            }

            existingListing.Title = updatedListing.Title;
            existingListing.Location = updatedListing.Location;
            existingListing.Price = updatedListing.Price;
            existingListing.PropertyType = updatedListing.PropertyType;
            existingListing.Description = updatedListing.Description;
            existingListing.Commission = _commissionService.CalculateCommission(updatedListing.Price);

            _context.HouseListings.Update(existingListing);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteListingAsync(Guid id)
        {
            var listing = await _context.HouseListings
                .Include(l => l.Images)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (listing == null)
            {
                return false;
            }

            _context.ListingImages.RemoveRange(listing.Images);
            _context.HouseListings.Remove(listing);
            await _context.SaveChangesAsync();

            return true;
        }
    }

}
