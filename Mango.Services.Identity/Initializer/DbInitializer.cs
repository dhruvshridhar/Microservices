using System;
using System.Security.Claims;
using IdentityModel;
using Mango.Services.Identity.DbContexts;
using Mango.Services.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace Mango.Services.Identity.Initializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(ApplicationDbContext db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public void Initialize()
        {
            //Check if we run it for 1st time
            if (_roleManager.FindByNameAsync(SD.Admin).Result == null)
            {
                // Since its async method we want to await here before execution moves ahead
                _roleManager.CreateAsync(new IdentityRole(SD.Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Customer)).GetAwaiter().GetResult();
            }
            else { return; }

            //Now we create default app users
            ApplicationUser adminUser = new()
            {
                UserName = "admin1@gmail.com",
                Email = "admin1@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "1234567890",
                FirstName = "Dhruv",
                LastName = "Admin"
            };

            //Create a new user
            _userManager.CreateAsync(adminUser, "Sample@pass123").GetAwaiter().GetResult();

            //Assigning role to new user
            _userManager.AddToRoleAsync(adminUser, SD.Admin).GetAwaiter().GetResult();

            //Adding claims for user
            var tempadmin = _userManager.AddClaimsAsync(adminUser, new Claim[]
            {
                new Claim(JwtClaimTypes.Name,adminUser.FirstName+" "+adminUser.LastName),
                new Claim(JwtClaimTypes.GivenName,adminUser.FirstName),
                new Claim(JwtClaimTypes.FamilyName, adminUser.LastName),
                new Claim(JwtClaimTypes.Role,SD.Admin)
            }).Result;

            //Now same for customer user
            ApplicationUser customerUser = new()
            {
                UserName = "cust1@gmail.com",
                Email = "cust1@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "1234567890",
                FirstName = "Dhruv",
                LastName = "Cust"
            };

            //Create a new user
            _userManager.CreateAsync(customerUser, "Sample@pass123").GetAwaiter().GetResult();

            //Assigning role to new user
            _userManager.AddToRoleAsync(customerUser, SD.Customer).GetAwaiter().GetResult();

            //Adding claims for user
            var tempcust = _userManager.AddClaimsAsync(customerUser, new Claim[]
            {
                new Claim(JwtClaimTypes.Name,customerUser.FirstName+" "+customerUser.LastName),
                new Claim(JwtClaimTypes.GivenName,customerUser.FirstName),
                new Claim(JwtClaimTypes.FamilyName, customerUser.LastName),
                new Claim(JwtClaimTypes.Role,SD.Customer)
            }).Result;
        }
    }
}

