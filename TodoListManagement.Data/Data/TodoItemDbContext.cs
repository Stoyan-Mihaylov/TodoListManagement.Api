using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TodoListManagement.Data.EntityConfigurations;
using TodoListManagement.Data.Models;

namespace TodoListManagement.Data.Data
{
    public class TodoItemDbContext : IdentityDbContext<IdentityUser>
    {
        public TodoItemDbContext(DbContextOptions<TodoItemDbContext> options) : base(options) { }

        public DbSet<Priority> Priorities { get; set; }

        public DbSet<Item> Items { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new ItemConfiguration());
            builder.ApplyConfiguration(new PriorityConfiguration());

            base.OnModelCreating(builder);
        }
    }
}
