using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseBrokerApplication.Application.DTOs
{
    public class ListingDto
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Location { get; set; }
        public decimal Price { get; set; }
        public string? PropertyType { get; set; }
        public string? Description { get; set; }
        public string? BrokerName { get; set; }
        public List<string> ImageUrls { get; set; } = new();
    }

}
