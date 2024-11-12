using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Data.Identity
{
    public class AppIdentityDbContextSeeding
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if (userManager.Users.Count() == 0)
            {
                var user = new AppUser()
                {
                    DisplayName = "MostafaAfify",
                    Email = "mostafamohamed@gmail.com",
                    UserName = "mostafamohamed@gmail.com".Split('@')[0],
                    PhoneNumber = "01080550580"
                };
                await userManager.CreateAsync(user,"Pa$$w0rd");
            }
        }
    }
}
