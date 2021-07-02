using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;


namespace React610SignalRUserTasks.data
{
        public class ChoreContext : DbContext
        {

            private readonly string _conn;

            public ChoreContext(string connectionString)
            {
                _conn = connectionString;
            }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlServer(_conn);
            }

            public DbSet<User> Users { get; set; }
            public DbSet<Chore> Tasks { get; set; }
 
    }
}
