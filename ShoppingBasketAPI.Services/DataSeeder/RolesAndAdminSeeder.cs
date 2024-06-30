using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ShoppingBasketAPI.Data;
using ShoppingBasketAPI.Domain;
using ShoppingBasketAPI.Utilities.ApplicationRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasketAPI.Services.DataSeeder
{
    /// <summary>
    /// Responsible for seeding roles and an admin user into the database.
    /// </summary>
    public class RolesAndAdminSeeder
    {
        private readonly IServiceProvider serviceProvider;

        public RolesAndAdminSeeder(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        /// <summary>
        /// This method will create Roles first and create one admin account later.
        /// </summary>
        /// <returns></returns>
        public async Task SeedRolesAndAdminAsync()
        {
            await this.SeedRolesAsync();
            await this.SeedAdminAsync();
        }

        private async Task SeedRolesAsync()
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roleNames = { IApplicationRoles.ADMIN, IApplicationRoles.EMPLOYEE, IApplicationRoles.WEB_USER };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
            await Task.CompletedTask;
            return;
        }

        private async Task SeedAdminAsync()
        {
            // Admin
            const string AdminEmail = "admin@gmail.com";
            const string AdminPassword = "Admin@1234";

            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var adminUser = new ApplicationUser
            {
                UserName = AdminEmail,
                Email = AdminEmail,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var user = await userManager.FindByEmailAsync(adminUser.Email);

            if (user == null)
            {
                var createUser = await userManager.CreateAsync(adminUser, AdminPassword);
                if (createUser.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, IApplicationRoles.ADMIN);
                }
            }
            await Task.CompletedTask;
            return;
        }
    }
}
