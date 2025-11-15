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
        }
    }
}
