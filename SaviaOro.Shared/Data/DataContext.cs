using Microsoft.EntityFrameworkCore;
using SaviaOro.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaviaOro.Shared.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Content> Contents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Course>().HasIndex(c => c.Title).IsUnique();
            modelBuilder.Entity<Content>().HasIndex("Title", "CourseId").IsUnique();
        }

    }
}
