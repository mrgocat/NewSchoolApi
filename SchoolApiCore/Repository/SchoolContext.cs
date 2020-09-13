using Microsoft.EntityFrameworkCore;
using SchoolApi.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace SchoolApi.Core.Repository
{
    public class SchoolContext : DbContext
    {
        public SchoolContext(DbContextOptions<SchoolContext> options) : base(options)
        {
        
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<EnrollmentPoco>().HasKey(e => new { e.StudentId, e.CourseId });
            modelBuilder.Entity<EnrollmentPoco>().HasOne(e => e.Student).WithMany(e => e.Enrollments).HasForeignKey(e => e.StudentId);
            modelBuilder.Entity<EnrollmentPoco>().HasOne(e => e.Course).WithMany(e => e.Enrollments).HasForeignKey(e => e.CourseId);
        }

        public DbSet<StudentPoco> Students { get; set; }
        public DbSet<CoursePoco> Courses { get; set; }
        public DbSet<EnrollmentPoco> Enrollments { get; set; }
        
    }
}
