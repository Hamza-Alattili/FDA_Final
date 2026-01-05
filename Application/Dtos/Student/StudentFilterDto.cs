namespace Application.Dtos.Student
{
    public class StudentFilterDto
    {
        public DateTime? BirthDate { get; set; }
        public string? University { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
