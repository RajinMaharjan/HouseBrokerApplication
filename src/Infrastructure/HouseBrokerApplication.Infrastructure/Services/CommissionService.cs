using HouseBrokerApplication.Application.Interface;
using HouseBrokerApplication.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseBrokerApplication.Infrastructure.Services
{
    public class CommissionService : ICommissionService
    {
        private readonly ApplicationDbContext _context;

        public CommissionService(ApplicationDbContext context) => _context = context;

        public double CalculateCommission(double price)
        {
            var rate = _context.CommissionRates.FirstOrDefault(r =>
            price >= r.MinPrice && price <= r.MaxPrice);

            return rate != null ? price * rate.Percentage : 0;
        }
    }
}
