using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice_On_EFCore.Models
{
    public class Student
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = "";
        
        public int? Age { get; set; }

        public int DepartmentId { get; set; }
        public Department? Department { get; set; }

        public ICollection<Course> Courses { get; set; } = new HashSet<Course>();
        public ICollection<StudentCourse> StudentCourses { get; set; } = new HashSet<StudentCourse>();

    }

}
