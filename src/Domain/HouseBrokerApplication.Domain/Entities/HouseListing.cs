﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseBrokerApplication.Domain.Entities
{
    public class HouseListing
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Location { get; set; }
        public double Price { get; set; }
        public string? PropertyType { get; set; }
        public string? Description { get; set; }
        public Guid? BrokerId { get; set; }
        public ApplicationUser Broker { get; set; }
        public double Commission { get; set; }
        public List<ListingImage> Images { get; set; } = new();
    }
}
