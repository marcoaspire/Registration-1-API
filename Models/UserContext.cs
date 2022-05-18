using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrationAPI.Models
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                .HasIndex(u => u.EmailAddress)
                .IsUnique();
            builder.Entity<User>(entity =>
            {
                entity.ToTable(name: "tbl_users");
            });
            builder.Entity<Employee>(entity =>
            {
                entity.ToTable(name: "tbl_employees");
            });
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Employee> Employees { get; set; }
    }
}
