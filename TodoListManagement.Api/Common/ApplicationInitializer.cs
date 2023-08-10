using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoListManagement.Api.Common.Constants;
using TodoListManagement.Data.Data;
using TodoListManagement.Data.Models;

namespace TodoListManagement.Api.Common
{
    public class ApplicationInitializer
    {
        private readonly TodoItemDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ApplicationInitializer(
            TodoItemDbContext dbContext,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task InitializeAsync()
        {
            await SeedAsync();
        }

        private async Task SeedAsync()
        {
            _dbContext.Database.EnsureCreated();
            await SeedPrioritiesAsync();
            await SeedRolesAsync();
            await SeedUsersAsync();
        }

        private async Task SeedRolesAsync()
        {
            if (await _dbContext.Roles.AnyAsync())
            {
                return;
            }

            await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
        }

        private async Task SeedUsersAsync()
        {
            if (await _dbContext.Users.AnyAsync())
            {
                return;
            }

            var adminUser = new IdentityUser
            {
                Email = "admin@gmail.com",
                UserName = "admin@gmail.com",
            };

            await _userManager.CreateAsync(adminUser, "Admin123!");
            await _userManager.AddToRoleAsync(adminUser, UserRoles.Admin);

            var regularUser = new IdentityUser
            {
                Email = "user@gmail.com",
                UserName = "user@gmail.com",
            };

            await _userManager.CreateAsync(regularUser, "User123!");
        }

        private async Task SeedPrioritiesAsync()
        {
            if (await _dbContext.Priorities.AnyAsync())
            {
                return;
            }

            var priorities = new List<Priority>
            {
                new Priority { Title = "Low" },
                new Priority { Title = "Medium" },
                new Priority { Title = "High" }
            };

            _dbContext.Priorities.AddRange(priorities);
            await _dbContext.SaveChangesAsync();
        }
    }
}
