using Microsoft.EntityFrameworkCore;
using RegistrationClinik.Models;
using System;

namespace RegistrationClinik.Infras
{
    public class ApplicationConnect : DbContext
    {


        public ApplicationConnect()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=localhost;user=root;password=;database=EcolifeDB;",
                 new MySqlServerVersion(new Version(5, 7, 29))
             );
        }
    }
}
