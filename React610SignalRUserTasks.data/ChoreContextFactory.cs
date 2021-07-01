using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Design;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace React610SignalRUserTasks.data
{
    public class ChoreContextFactory : IDesignTimeDbContextFactory<ChoreContext>
    {
        public ChoreContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), $"..{Path.DirectorySeparatorChar}React610SignalRUserTasks.web"))
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true).Build();

            var constr = new ChoreContext(config.GetConnectionString("ConStr"));
            return new ChoreContext(config.GetConnectionString("ConStr"));
        }

    }
}
