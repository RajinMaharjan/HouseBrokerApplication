using HouseBrokerApplication.Application.DTOs;
using HouseBrokerApplication.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseBrokerApplication.Application.Interface
{
    public interface IAccountService
    {
        Task<IdentityResult> RegisterAsync(RegisterDto model);
        Task<ApplicationUser> LoginAsync(LoginDto model);
        string GenerateToken(ApplicationUser user);
    }

}
