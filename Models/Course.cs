using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practice_On_EFCore.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public int? StdId { get; set; }
        [ForeignKey("StdId")]
        public Student? Student { get; set; }
    }

}
