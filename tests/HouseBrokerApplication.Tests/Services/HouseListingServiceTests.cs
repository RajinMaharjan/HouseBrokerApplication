using HouseBrokerApplication.Application.Interface;
using HouseBrokerApplication.Infrastructure.Persistence;
using HouseBrokerApplication.Infrastructure.Services;
using HouseBrokerApplication.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace HouseBrokerApplication.Tests.Services
{
    public class HouseListingServiceTests
    {
        private readonly ApplicationDbContext _context;
        private readonly Mock<ICommissionService> _commissionServiceMock;
        private readonly HouseListingService _service;

        public HouseListingServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _commissionServiceMock = new Mock<ICommissionService>();

            _service = new HouseListingService(_context, _commissionServiceMock.Object);
        }

        [Fact]
        public async Task CreateListingAsync_Should_Add_Listing_With_Commission()
        {
            var listing = new HouseListing
            {
                Id = Guid.NewGuid(),
                Title = "Modern Apartment",
                Location = "Kathmandu",
                Price = 100000,
                PropertyType = "Flat",
                Description = "A nice flat in city center"
            };

            _commissionServiceMock.Setup(c => c.CalculateCommission(listing.Price)).Returns(5000);

            var result = await _service.CreateListingAsync(listing);

            Assert.Equals(5000, result.Commission);
            var savedListing = await _context.HouseListings.FindAsync(result.Id);
            Assert.NotNull(savedListing);
        }

        [Fact]
        public async Task GetAllListingsAsync_Should_Return_Empty_List_When_No_Data()
        {
            var listings = await _service.GetAllListingsAsync();
            Assert.IsEmpty(listings);
        }

        [Fact]
        public async Task UpdateListingAsync_Should_Return_False_If_Not_Exists()
        {
            var result = await _service.UpdateListingAsync(Guid.NewGuid(), new HouseListing());
            Assert.False(result);
        }

        [Fact]
        public async Task DeleteListingAsync_Should_Remove_Listing_If_Exists()
        {
            var listing = new HouseListing
            {
                Id = Guid.NewGuid(),
                Title = "House to Delete",
                Price = 200000
            };
            _context.HouseListings.Add(listing);
            await _context.SaveChangesAsync();

            var result = await _service.DeleteListingAsync(listing.Id);

            Assert.True(result);
            var deleted = await _context.HouseListings.FindAsync(listing.Id);
            Assert.Null(deleted);
        }
    }
}
