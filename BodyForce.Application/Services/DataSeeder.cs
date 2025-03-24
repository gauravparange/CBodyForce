using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyForce
{
    public class DataSeeder
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;

        public DataSeeder(RoleManager<Role> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task SeedRolesAndAdminAsync()
        {
            // Define roles
            string[] roles = { "Administrator"};

            // Create roles if they don't exist
            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new Role()
                    {
                        Name = role,
                    });
                }
            }                
       
            var adminUser = await _userManager.FindByNameAsync("Administrator");
            if (adminUser == null)
            {
                adminUser = new User
                {
                    UserName = "Administrator",
                    Email = "administrator@bodyforce.in",
                    FirstName = "Administrator",
                    LastName = ""
                };

                var result = await _userManager.CreateAsync(adminUser, "Admin@123");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(adminUser, "Administrator");
                }
            }
        }
    }
}
