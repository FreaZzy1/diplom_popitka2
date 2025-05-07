using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Diplom_Cheremnykh.Models;
using Microsoft.EntityFrameworkCore;

namespace Diplom_Cheremnykh.Data
{
    public class AppDbContext:DbContext
    {

        public DbSet<User> Users { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<FraudCase> FraudCases { get; set; }

        public AppDbContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=127.0.0.1;port=3306;database=FraudDetectionDB;user=root", new MySqlServerVersion(new Version(8, 0, 11)));
        }
    }
}
