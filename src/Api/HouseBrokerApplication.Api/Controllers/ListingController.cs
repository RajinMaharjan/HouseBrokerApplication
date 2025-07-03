using HouseBrokerApplication.Application.DTOs;
using HouseBrokerApplication.Application.Interface;
using HouseBrokerApplication.Domain.Entities;
using HouseBrokerApplication.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HouseBrokerApplication.Api.Controllers
{
    [Authorize(Roles = "Broker,Seeker")]
    [Route("api/[controller]")]
    [ApiController]
    public class ListingController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ICommissionService _commissionService;

        public ListingController(ApplicationDbContext context, ICommissionService commissionService)
        {
            _context = context;
            _commissionService = commissionService;
        }

        [HttpPost]
        [Authorize(Roles = "Broker")]
        public async Task<IActionResult> CreateListing([FromBody] HouseListing listing)
        {
            listing.Commission = _commissionService.CalculateCommission(listing.Price);
            _context.HouseListings.Add(listing);

            await _context.SaveChangesAsync().ConfigureAwait(false);

            return CreatedAtAction(nameof(GetById), new { id = listing.Id }, listing);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ListingDto>>> GetAllListings()
        {
            var listings = await _context.HouseListings
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

            return Ok(listings);
        }

        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid id)
        {
            var listing = await _context.HouseListings
                .Include(l => l.Images)
                .Include(l => l.Broker)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (listing == null) return NotFound();

            var result = new
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

            return Ok(result);
        }

        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<IActionResult> Search(
         [FromQuery] string? location,
         [FromQuery] decimal? minPrice,
         [FromQuery] decimal? maxPrice,
         [FromQuery] string? propertyType,
         [FromQuery] string sortBy = "price",
         [FromQuery] string sortDirection = "asc",
         [FromQuery] int page = 1,
         [FromQuery] int pageSize = 10)
        {
            var query = _context.HouseListings
                .Include(l => l.Images)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(location))
                query = query.Where(l => l.Location.Contains(location));

            if (minPrice.HasValue)
                query = query.Where(l => l.Price >= minPrice.Value);

            if (maxPrice.HasValue)
                query = query.Where(l => l.Price <= maxPrice.Value);

            if (!string.IsNullOrWhiteSpace(propertyType))
                query = query.Where(l => l.PropertyType == propertyType);

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

            return Ok(new { totalCount, listings });
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Broker")]
        public async Task<IActionResult> UpdateListing(Guid id, [FromBody] HouseListing updatedListing)
        {
            var existingListing = await _context.HouseListings.FindAsync(id);
            if (existingListing == null) return NotFound();

            existingListing.Title = updatedListing.Title;
            existingListing.Location = updatedListing.Location;
            existingListing.Price = updatedListing.Price;
            existingListing.PropertyType = updatedListing.PropertyType;
            existingListing.Description = updatedListing.Description;
            
            existingListing.Commission = _commissionService.CalculateCommission(updatedListing.Price);

            _context.HouseListings.Update(existingListing);
            await _context.SaveChangesAsync();

            return Ok("Updated");
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Broker")]
        public async Task<IActionResult> DeleteListing(Guid id)
        {
            var listing = await _context.HouseListings
                .Include(l => l.Images)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (listing == null) return NotFound();

            _context.ListingImages.RemoveRange(listing.Images);

            _context.HouseListings.Remove(listing);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
