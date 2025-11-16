using Microsoft.EntityFrameworkCore;
using Practice_On_EFCore.Data;
using Practice_On_EFCore.Models;

namespace Practice_On_EFCore
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using var context = new AppDbContext();

            //insert data on Students
            context.Students.Add(new Student { Name = "Hani Mohamed", Age = 32 });
            context.Students.Add(new Student { Name = "Khaled Reda", Age = 43 });
            context.Students.Add(new Student { Name = "Sheko abo Ahmed", Age = 43 });

            context.SaveChanges();

            var Result = context.Students.ToList();

            foreach (var res in Result)
            {
                Console.WriteLine($"StudentId:{res.Id} , StudentName:{res.Name} ");
            }



            var student = new Student { Name = "Ali", Age = 23 };
            student.Courses = new List<Course>
            {
                new Course { Title = "Math" },
                new Course { Title = "Physics" }
            };
            context.Students.Add(student);
            context.SaveChanges();


            var studentsWithCourses = context.Students.Include(s => s.Courses).ToList();

            //var Result = context.Courses.Include(x => x.Student).ToList();

            foreach (var res in studentsWithCourses)
            {
                Console.WriteLine($"StdId:{res.Id}  ,StdName: {res.Name}  ");
                foreach (var course in res.Courses)
                {
                    Console.WriteLine($"CourseName {course.Title}");
                }
            }

           // CRUD
             //Insert
            var newStudent = new Student
            {
                Age = 34,
                Name = "Hisham Selim",
                Courses = { new Course { Title = ".Net" },
                    new Course { Title = "PHP" } }
            };
            context.Students.Add(newStudent);

            var updateAge = context.Students.FirstOrDefault(x => x.Id == 5);
            updateAge.Age = 21;

            context.Students.Update(updateAge);


            //delete
            var deleteStudent = context.Students.FirstOrDefault(x => x.Id == 6);
            context.Students.Remove(deleteStudent);
            context.SaveChanges();

           // GetById
            var getStdById = context.Students.Find(1);
            Console.WriteLine($"StdId:{getStdById.Id},StdName:{getStdById.Name} ,SdtAge:{getStdById.Age}");


           // GetAll
           var allStudents = context.Students.ToList();

          //  Filter
           var filtered = context.Students.Where(s => s.Age > 20).ToList();

          ///  Sorting & Filtering
            var StdFilter = context.Students
                .Where(x => x.Age > 23)
                .OrderByDescending(x => x.Age)
                .Select(s => new { s.Name, s.Age, })
                .ToList();
            foreach (var s in StdFilter)
                Console.WriteLine(s);

            //pagination

            int page = 2;
            int pageSize = 3;
            var paging = context.Students
                .Skip((page-1) * pageSize)
                .Take(pageSize)
                .ToList();

            foreach ( var p in paging )
            {
                Console.WriteLine($"{p.Name} ,{p.Id} ,{p.Age} ");
            }

            int totalCount = context.Students.Count();
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            Console.WriteLine($"Total Pages = {totalPages}");


        }
    }
}
