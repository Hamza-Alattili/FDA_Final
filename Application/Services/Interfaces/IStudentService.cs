using Application.Dtos.Auth;
using Application.Dtos.Course;
using Application.Dtos.Student;

namespace Application.Services.Interfaces
{
    public interface IStudentService
    {
        Task StudentReg(StudentregistrationDto student);
        Task StudentUpdate(StudentUpdateDto student);
        Task ResetPassword(ResetPasswordDto input);
        Task<StudentListDto> GetStudentById(int Id);
        Task<List<StudentListDto>> GetStudentList();
        Task DeleteStudent(int Id);


    }
}
