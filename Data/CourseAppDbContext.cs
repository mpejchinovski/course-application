using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CourseApp.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CourseApp.Data
{
    public class CourseAppDbContext : DbContext
    {
        public CourseAppDbContext(DbContextOptions<CourseAppDbContext> options) : base(options)
        { }

        public DbSet<Course> Course { get; set; }
        public DbSet<CourseDate> CourseDate { get; set; }
        public DbSet<Application> Application { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<CourseDate>()
                .HasOne<Course>(cd => cd.Course)
                .WithMany(cd => cd.Dates)
                .HasForeignKey(cd => cd.CourseId);
        }
    }
}
