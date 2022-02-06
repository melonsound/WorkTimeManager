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
            optionsBuilder.UseNpgsql(Environment.GetEnvironmentVariable("SECURITY_DB"));

            return new AppSecurityDbContext(optionsBuilder.Options);
        }
    }
}
