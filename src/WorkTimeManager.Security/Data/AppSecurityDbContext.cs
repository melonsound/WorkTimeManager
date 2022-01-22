using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WorkTimeManager.Security.Models;

namespace WorkTimeManager.Security.Data
{
    public class AppSecurityDbContext : DbContext
    {
        public DbSet<AppUser> appUsers { get; set; }

        public AppSecurityDbContext(DbContextOptions<AppSecurityDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }
    }
}
