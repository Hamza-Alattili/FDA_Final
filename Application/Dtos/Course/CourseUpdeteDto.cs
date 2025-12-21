using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Course
{
    public class CourseUpdeteDto
    {
        [Required]
        public int CourseId { get; set; }
        [Required]
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int Price { get; set; }
        public DateOnly StartCourse { get; set; }
        public DateOnly EndCourse { get; set; }
        public string Category { get; set; }
        public DateOnly CourseStarted { get; set; }

    }
}
