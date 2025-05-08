using HouseReservation.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseReservation.Infrastructure.Services
{
    public static class IdentityDataSeeder
    {
        private static readonly string[] Roles = [ "User", "Admin", "Manager" ];

        public static async Task SeedAsync(IServiceProvider services)
        {
            var roleMgr = services.GetRequiredService<RoleManager<IdentityRole<int>>>();
            foreach (var role in Roles)
            {
                if (!await roleMgr.RoleExistsAsync(role))
                    await roleMgr.CreateAsync(new IdentityRole<int>(role));
            }
        }
    }
}
