using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseBrokerApplication.Domain.Entities
{
    public class ApplicationUser:IdentityUser<Guid>
    {
        public string? FullName { get; set; }
        public string? Role { get; set; }
        public ICollection<HouseListing> Listings { get; set; } = new List<HouseListing>();
    }
}
