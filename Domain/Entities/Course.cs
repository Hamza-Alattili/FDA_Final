using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Course
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string CourseTitle { get; set; }
        public string CourseDescription { get; set; }
        public int Price { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public int CategoryId { get; set; }
        public CategoryTypes category { get; set; }
        public ICollection<Student> students { get; set; }
       // public DateTime CourseStarted { get; set; } = DateTime.Now;
        public ICollection<Enrollment> Enrollments { get; set; }

    }
}
