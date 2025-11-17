using Microsoft.EntityFrameworkCore;
using Practice_On_EFCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice_On_EFCore.Data
{
    internal class AppDbContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Department> Departments { get; set; }

        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public AppDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=EfCorePracticeDb;Trusted_Connection=True;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Course>()
                .HasOne(c => c.Student)
                .WithMany(s => s.Courses)
                .HasForeignKey(c => c.StdId)
                .OnDelete(DeleteBehavior.Restrict);
            
            // Composite key for join table
            modelBuilder.Entity<StudentCourse>()
                .HasKey(sc => new { sc.StudentId, sc.CourseId });


            // Relations
            modelBuilder.Entity<StudentCourse>()
                .HasOne(sc => sc.Student)
                .WithMany(s => s.StudentCourses)
                .HasForeignKey(sc => sc.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StudentCourse>()
                .HasOne(sc => sc.Course)
                .WithMany(c => c.CoursesStudents)
                .HasForeignKey(sc => sc.CourseId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Student>()
                .HasOne(d => d.Department)
                .WithMany(s => s.Students)
                .HasForeignKey(s => s.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Course>()
                .HasOne(d => d.Department)
                .WithMany(s => s.Courses)
                .HasForeignKey(s => s.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            //seeding data
            modelBuilder.Entity<Department>().HasData(
               new Department { Id = 1, Name = "Computer Science" },
               new Department { Id = 2, Name = "Information Systems" },
               new Department { Id = 3, Name = "AI Department" });


            // Seed 10 students
            var students = new List<Student>();
            for (int i = 1; i <= 10; i++)
            {
                students.Add(new Student
                {
                    Id = i,
                    Name = $"Student {i}",
                    Age = 18 + i,
                    DepartmentId = (i % 3) + 1
                });
            }
            
            modelBuilder.Entity<Student>().HasData(students);

            // Seed 6 courses
            modelBuilder.Entity<Course>().HasData(
                new Course { Id = 1, Title = "C#", DepartmentId = 1 },
                new Course { Id = 2, Title = "SQL", DepartmentId = 1 },
                new Course { Id = 3, Title = "Networks", DepartmentId = 2 },
                new Course { Id = 4, Title = "AI Intro", DepartmentId = 3 },
                new Course { Id = 5, Title = "ML Basics", DepartmentId = 3 },
                new Course { Id = 6, Title = "Algorithms", DepartmentId = 1 }
            );

            modelBuilder.Entity<StudentCourse>().HasData(
           new { StudentId = 1, CourseId = 1 },
           new { StudentId = 1, CourseId = 2 },
           new { StudentId = 2, CourseId = 2 },
           new { StudentId = 3, CourseId = 3 } );

            base.OnModelCreating(modelBuilder);
        }
    }
}
