using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Talabat.Core.Entities.Identity;

namespace Talabat.APIS.Extensions
{
    public static class UserManagerExtentions
    {
        public static async Task<AppUser> FindUserWithAddressByEmailAsync(this UserManager<AppUser> userManager ,  ClaimsPrincipal claims)
        {
            var email = claims.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.Users.Include(u=>u.Address).FirstOrDefaultAsync(u=>u.NormalizedEmail == email.ToUpper());
            return user;
        }
    }
}
