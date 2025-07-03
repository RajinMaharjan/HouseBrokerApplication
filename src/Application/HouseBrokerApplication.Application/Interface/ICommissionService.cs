using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseBrokerApplication.Application.Interface
{
    public interface ICommissionService
    {
        decimal CalculateCommission(decimal price);
    }
}
