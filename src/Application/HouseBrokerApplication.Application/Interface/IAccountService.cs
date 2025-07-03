using HouseBrokerApplication.Application.DTOs;
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
        Task<(bool Success, string? Role, string? Token, DateTime? Expiry)> LoginAsync(LoginDto model);
    }

}
