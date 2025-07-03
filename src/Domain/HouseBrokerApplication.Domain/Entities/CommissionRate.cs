using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseBrokerApplication.Domain.Entities
{
    public class CommissionRate
    {
        public int Id { get; set; }
        public double MinPrice { get; set; }
        public double MaxPrice { get; set; }
        public double Percentage { get; set; }
    }
}
