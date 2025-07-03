using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseBrokerApplication.Domain.Entities
{
    public class ListingImage
    {
        public Guid Id { get; set; }
        public string? Url { get; set; }
        public Guid HouseListingId { get; set; }
        public HouseListing? HouseListing { get; set; }
    }
}
