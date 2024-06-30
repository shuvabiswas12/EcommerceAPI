using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ShoppingBasketAPI.Domain;
using ShoppingBasketAPI.Utilities.ApplicationRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingBasketAPI.Services.DataSeederServices
{
    public static class DataSeeder
    {
        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
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
        }

        public static async Task SeedAdminAsync(IServiceProvider serviceProvider)
        {
            // Admin
            const string AdminEmail = "admin@gmail.com";
            const string AdminPassword = "Admin@1234";

            await SeedUsersAsync(serviceProvider, email: AdminEmail, password: AdminPassword);
            await Task.CompletedTask;
            return;
        }

        public static async Task SeedEmployeeAsync(IServiceProvider serviceProvider)
        {
            // Employee
            const string EmployeeEmail = "demoemployee@gmail.com";
            const string EmployeePassword = "DemoEmployee@1234";

            await SeedUsersAsync(serviceProvider, email: EmployeeEmail, password: EmployeePassword);
            await Task.CompletedTask;
            return;
        }

        public static async Task SeedWebUserAsync(IServiceProvider serviceProvider)
        {
            // Web User
            const string WebUserEmail = "demouser@gmail.com";
            const string WebUserPassword = "DemoUser@1234";

            await SeedUsersAsync(serviceProvider, email: WebUserEmail, password: WebUserPassword);
            await Task.CompletedTask;
            return;
        }

        private static async Task SeedUsersAsync(IServiceProvider serviceProvider, string email, string password)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            var adminUser = new ApplicationUser
            {
                UserName = email,
                Email = email,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var user = await userManager.FindByEmailAsync(adminUser.Email);

            if (user == null)
            {
                var createUser = await userManager.CreateAsync(adminUser, password);
                if (createUser.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, IApplicationRoles.ADMIN);
                }
            }
        }
    }
}
