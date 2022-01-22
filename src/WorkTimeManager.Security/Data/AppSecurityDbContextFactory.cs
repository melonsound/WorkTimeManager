using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkTimeManager.Security.Data
{
    public class AppSecurityDbContextFactory : IDesignTimeDbContextFactory<AppSecurityDbContext>
    {
        public AppSecurityDbContext CreateDbContext(string[] args)
        {

            var optionsBuilder = new DbContextOptionsBuilder<AppSecurityDbContext>();
            optionsBuilder.UseNpgsql("User ID=postgres;Password=root;Host=localhost;Port=5432;Database=wtm_v2_security;Trust Server Certificate=true");

            return new AppSecurityDbContext(optionsBuilder.Options);
        }
    }
}
