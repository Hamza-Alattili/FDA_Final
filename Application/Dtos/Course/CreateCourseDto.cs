using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Course
{
    public class CreateCourseDto
    {
        
        public int Id { get; set; }
        [Required]
        public string CourseTitle { get; set; }
        public string CourseDescription { get; set; }
        public int Price { get; set; }
        public DateOnly StartCourse { get; set; }
        public DateOnly EndCourse { get; set; }
        public int CategoryId { get; set; }

    }
}
