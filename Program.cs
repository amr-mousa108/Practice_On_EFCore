using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Practice_On_EFCore.Data;
using Practice_On_EFCore.Models;
using System.Threading.Channels;

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
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            foreach (var p in paging)
            {
                Console.WriteLine($"{p.Name} ,{p.Id} ,{p.Age} ");
            }

            int totalCount = context.Students.Count();
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            Console.WriteLine($"Total Pages = {totalPages}");

           // stored Procedure
            var stds = context.Students
                .FromSqlRaw("Exec GetStdOverAge {0} ", 20)
                .ToList();

            foreach (var std in stds)
            {
                Console.WriteLine($"StdId:{std.Id}  ,StdName: {std.Name}  ");
                foreach (var course in std.Courses)
                {
                    Console.WriteLine($"CourseName {course.Title}");
                }

            }
        }

        //CRUD Operations 

        public void SearchByName(AppDbContext context)
        {
            Console.Write("Enter Student Name To Search about");
            var name = Console.ReadLine() ?? "";

            var std = context.Students.Where(s => s.Name.Contains(name))
                .Include(s => s.StudentCourses)
                .ThenInclude(c => c.Course)
                .ToList();

            foreach (var s in std)
            {
              var courses =string.Join("," , s.StudentCourses.Select(s => s.Course!.Title));
                Console.WriteLine($"{s.Id} | {s.Name} | {courses}");

            }
        }
        public void AddStudent(AppDbContext context)
        {
            Console.Write("Name"); var name = Console.ReadLine() ?? "";
            Console.Write("Age"); var age = int.Parse(Console.ReadLine() ?? "0");
            Console.Write("DepartmentId"); var deptId = int.Parse(Console.ReadLine() ?? "0");

            var newStd=new Student { Age = age,Name = name,DepartmentId = deptId };
            context.Students.Add(newStd);
            context.SaveChanges();
            Console.WriteLine("Added");


        }

        public void UpdateStudent(AppDbContext context)
        {
            Console.Write("Enter Student Id to Update: "); var id = int.Parse(Console.ReadLine() ?? "0");
            var std = context.Students.Find(id);
            if (std == null) { Console.WriteLine("Not found"); return; }

            Console.Write("New Name (leave empty to keep)");
            var name = Console.ReadLine();
            if (!string.IsNullOrEmpty(name)) std.Name = name;

            Console.Write("New Age (leave empty to keep)");
            var age = Console.ReadLine();
            if (!string.IsNullOrEmpty(age)) std.Age = int.Parse(age); 
            
            Console.Write("New DepartmentId (leave empty to keep)");
            var DeptId = Console.ReadLine();
            if (!string.IsNullOrEmpty(DeptId)) std.DepartmentId = int.Parse(DeptId);

            context.SaveChanges();
            Console.WriteLine("Updated");




        }

        public void DeleteStudent(AppDbContext context)
        {
            Console.Write("Enter Id: "); var id =int.Parse(Console.ReadLine() ?? "0");
            var std = context.Students.Include(s => s.StudentCourses)
                .FirstOrDefault(s => s.Id ==id);
            if( std == null) { Console.WriteLine("Not Fount"); return; }

            if(std.StudentCourses.Any() )
            {
                context.StudentCourses.RemoveRange(std.StudentCourses);
            }
            context.Students.Remove(std);
            context.SaveChanges();
            Console.WriteLine("Deleted");

        }
        public void AssignCourseToStudent(AppDbContext context)
        {
            Console.Write("Enter Student Id"); var S_id = int.Parse(Console.ReadLine() ?? "0");
            Console.Write("Enter Course Id");  var C_id = int.Parse(Console.ReadLine() ?? "0");

            var exists = context.StudentCourses.Find(S_id ,C_id);
            if(exists != null) { Console.WriteLine("Already Assigned"); return; }
            context.StudentCourses.Add(new StudentCourse { StudentId = S_id, CourseId = C_id });
            context.SaveChanges();
            Console.WriteLine("Assigned");
        }
    }
}
