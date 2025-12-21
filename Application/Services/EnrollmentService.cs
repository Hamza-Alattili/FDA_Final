using Application.Dtos.Enrollment;
using Application.Repositories.Interface;
using Application.Services.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly IGenericRepository<Enrollment> _enrollmentRepo;
        private readonly IGenericRepository<Student> _studentRepo;
        private readonly IGenericRepository<Course> _courseRepo;

        public EnrollmentService(
            IGenericRepository<Enrollment> enrollmentRepo,
            IGenericRepository<Student> studentRepo,
            IGenericRepository<Course> courseRepo)
        {
            _enrollmentRepo = enrollmentRepo;
            _studentRepo = studentRepo;
            _courseRepo = courseRepo;
        }
        public async Task<EnrollmentDto> EnrollStudentAsync(int userId, int courseId)
        {
            var student = await _studentRepo.GetAll()
        .FirstOrDefaultAsync(s => s.UserId == userId);


            if (student == null)
                throw new Exception("Student not found.");

            var course = await _courseRepo.GetById(courseId);
            if (course == null)
                throw new Exception("Course not found.");

            var existingEnrollment = await _enrollmentRepo.GetAll()
                .FirstOrDefaultAsync(e => e.StudentId == student.Id && e.CourseId == courseId);

            if (existingEnrollment != null)
            {
                return new EnrollmentDto
                {
                    StudentId = student.Id,
                    StudentName = student.FullName,
                    CourseId = courseId,
                    CourseTitle = course.CourseTitle,
                    EnrollmentDate = existingEnrollment.EnrollmentDate
                };
            }

            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            if (course.EndDate < today)
                throw new Exception("Cannot enroll, course has already ended.");

            if (course.StartDate > today)
                throw new Exception("Cannot enroll, course has not started yet.");
            var enrollment = new Enrollment
            {
                StudentId = student.Id,
                CourseId = courseId,
                EnrollmentDate = today
            };

            await _enrollmentRepo.Insert(enrollment);
            await _enrollmentRepo.SaveChanges();

            return new EnrollmentDto
            {
                StudentId = student.Id,
                StudentName = student.FullName,
                CourseId = courseId,
                CourseTitle = course.CourseTitle,
                EnrollmentDate = enrollment.EnrollmentDate
            };
        }
        public async Task<StudentCoursesDto> GetEnrolByStu(int userId)
        {
            var student = await _studentRepo.GetAll()
                .FirstOrDefaultAsync(s => s.UserId == userId);


            if (student == null)
                throw new Exception("Student not found.");

            var courses = await _enrollmentRepo.GetAll()
                .Where(e => e.StudentId == student.Id)
                .Include(e => e.Course)
                .Select(e => e.Course.CourseTitle)
                .ToListAsync();

            return new StudentCoursesDto
            {
                StudentId = student.Id,
                StudentName = student.FullName,
                Courses = courses
            };
        }

        public async Task<CourseStudentsDto> GetEnrolByCor(int courseId)
        {
            var course = await _courseRepo.GetById(courseId);
            if (course == null)
                throw new Exception("Course not found.");

            var students = await _enrollmentRepo.GetAll()
                .Where(e => e.CourseId == courseId)
                .Include(e => e.Student)
                .Select(e => e.Student.FullName)
                .ToListAsync();

            return new CourseStudentsDto
            {
                CourseId = courseId,
                CourseTitle = course.CourseTitle,
                Students = students
            };
        }
    }
}