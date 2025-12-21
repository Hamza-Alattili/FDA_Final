using Application.Dtos.Course;
using Domain.Entities;

namespace Application.Services.Interfaces
{
    public interface ICourseServicecs
    {
        Task CreateCourse(CreateCourseDto course);
        Task UpdateCourse(int Id,CourseUpdeteDto course);
        Task DeleteCourse(int id);
        Task<List<CourseFilterDto>> GetCourse(int id,CourseFilterDto filter);
        Task<CourseListDto> GetCourseById(int id);
    }
}
