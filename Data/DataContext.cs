using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Callories_Tracker.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Entity.Account> Accounts { get; set; }
        public DbSet<Entity.Stat> Stats { get; set; }
        public DbSet<Entity.Achievements> Achievements { get; set; }

        public DataContext() : base() { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string? connection_stroke = App.GetConfiguration("database:stroke");
            optionsBuilder.UseSqlServer($@"{connection_stroke}");
        }
    }
}
