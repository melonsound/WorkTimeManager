using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkTimeManager.Security.Data;

namespace WorkTimeManager.Security
{
    public static class StartupSetup
    {
        public static IServiceCollection AddSecurityDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContextPool<AppSecurityDbContext>(option => option.UseNpgsql(connectionString));
            return services;
        }
    }
}
