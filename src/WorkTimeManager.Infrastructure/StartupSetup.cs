using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTimeManager.Infrastructure.Data;

namespace WorkTimeManager.Infrastructure
{
    public static class StartupSetup
    {
        public static IServiceCollection AddDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContextPool<AppDbContext>(option => option.UseNpgsql(connectionString));
            return services;
        } 
    }
}
