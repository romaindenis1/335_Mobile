using _4Crud.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _4Crud.Services
{
    public class AladdinContext : DbContext
    {
        public DbSet<Wish> Wishes { get; set; }
        public AladdinContext() { 
        this.Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "aladdin.sqlite");
            options.UseSqlite($"Filename={ dbPath}");
        }
    }
}
