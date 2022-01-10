using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkTimeManager.Infrastructure.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseNpgsql("User ID=zstieltpkffxah;Password=c4929409b9ea8288a08612a3cb18d51ea5d75f4fd4b91329301b2a542b18bdc1;Host=ec2-54-229-47-120.eu-west-1.compute.amazonaws.com;Port=5432;Database=dc9bg3p40kclp4;SSL Mode=Require;Trust Server Certificate=true");

            return new AppDbContext(optionsBuilder.Options);
        }

    }
}
