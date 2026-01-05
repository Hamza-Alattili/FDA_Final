namespace Application.Dtos.Student
{
    public class StudentListDto
    {
        public int StudentId { get; set; }
        public DateTime BirthDate { get; set; }
        public string University { get; set; }
        public UserListDto User { get; set; }
    }

    public class UserListDto
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
