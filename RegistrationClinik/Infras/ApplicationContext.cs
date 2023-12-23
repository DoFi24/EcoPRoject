using Microsoft.EntityFrameworkCore;
using RegistrationClinik.Models;
using System;

namespace RegistrationClinik.Infras
{
    public class ApplicationConnect : DbContext
    {
        public DbSet<DBFilter> DBFilter { get; set; }
        public DbSet<DBKartrij> DBKartrij { get; set; }
        public DbSet<DBKartrigList> DBKartrigList { get; set; }
        public DbSet<DBTable> DBTable { get; set; }

        public ApplicationConnect()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=localhost;user=root;password=;database=EcolifeDB;default command timeout = 60",
                 new MySqlServerVersion(new Version(5, 7, 29))
             );
        }
    }
}
