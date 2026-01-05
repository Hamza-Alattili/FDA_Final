using Application.Dtos.Enrollment;

namespace Application.Services.Interfaces
{
    public interface IEnrollmentService
    {
        Task<EnrollmentDto> EnrollStudentAsync(int StudentId, int CourseId);
    }
}
