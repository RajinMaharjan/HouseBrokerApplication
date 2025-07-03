using HouseBrokerApplication.Application.DTOs;
using HouseBrokerApplication.Application.Interface;
using HouseBrokerApplication.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HouseBrokerApplication.Api.Controllers
{
    [Authorize(Roles = "Broker,Seeker")]
    [ApiController]
    [Route("api/[controller]")]
    public class HouseListingController : ControllerBase
    {
        private readonly IHouseListingService _houseListingService;

        public HouseListingController(IHouseListingService houseListingService)
        {
            _houseListingService = houseListingService;
        }

        [HttpPost]
        [Authorize(Roles = "Broker")]
        public async Task<IActionResult> CreateListing([FromBody] HouseListing listing)
        {
            var result = await _houseListingService.CreateListingAsync(listing);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ListingDto>>> GetAllListings()
        {
            var listings = await _houseListingService.GetAllListingsAsync();
            return Ok(listings);
        }

        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _houseListingService.GetListingByIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<IActionResult> Search(
            [FromQuery] string? location,
            [FromQuery] double? minPrice,
            [FromQuery] double? maxPrice,
            [FromQuery] string? propertyType,
            [FromQuery] string sortBy = "price",
            [FromQuery] string sortDirection = "asc",
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _houseListingService.SearchListingsAsync(location, minPrice, maxPrice, propertyType, sortBy, sortDirection, page, pageSize);
            return Ok(result);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Broker")]
        public async Task<IActionResult> UpdateListing(Guid id, [FromBody] HouseListing updatedListing)
        {
            var success = await _houseListingService.UpdateListingAsync(id, updatedListing);
            if (!success) return NotFound();
            return Ok("Updated");
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Broker")]
        public async Task<IActionResult> DeleteListing(Guid id)
        {
            var success = await _houseListingService.DeleteListingAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }

}
