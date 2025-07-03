using HouseBrokerApplication.Application.DTOs;
using HouseBrokerApplication.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseBrokerApplication.Application.Interface
{
    public interface IHouseListingService
    {
        Task<HouseListing> CreateListingAsync(HouseListing listing);
        Task<IEnumerable<ListingDto>> GetAllListingsAsync();
        Task<object?> GetListingByIdAsync(Guid id);
        Task<object> SearchListingsAsync(string? location, double? minPrice, double? maxPrice, string? propertyType, string sortBy, string sortDirection, int page, int pageSize);
        Task<bool> UpdateListingAsync(Guid id, HouseListing updatedListing);
        Task<bool> DeleteListingAsync(Guid id);
    }
}
