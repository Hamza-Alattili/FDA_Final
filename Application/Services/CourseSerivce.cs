using Application.Dtos.Course;
using Application.Repositories.Interface;
using Application.Services.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class CourseSerivce : ICourseServicecs
    {
        private readonly IGenericRepository<Course> _courseRepo;
        private readonly IGenericRepository<CategoryTypes> _categoryRepo;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IGenericRepository<Enrollment> _enrollment;
        private readonly IGenericRepository<Student> _student;
        public CourseSerivce(IGenericRepository<Course> courseRepo,
            IGenericRepository<CategoryTypes> categoryRepo,
            IHttpContextAccessor contextAccessor,
            IGenericRepository<Enrollment> enrollment,
         IGenericRepository<Student> student)
        {
            _courseRepo = courseRepo;
            _categoryRepo = categoryRepo;
            _contextAccessor = contextAccessor;
            _enrollment = enrollment;
            _student = student;
        }



        public async Task CreateCourse(CreateCourseDto course)
        {
            var IsCorNamExist = await _courseRepo.GetAll()
                 .AnyAsync(c => c.CourseTitle.ToLower() == course.CourseTitle.ToLower());
            if (IsCorNamExist)
            {
                throw new Exception("Course name already exists.");

            }
            if (course.Price < 0)
            {
                throw new Exception("Price must be greater than to 0");
            }
            if (course.EndCourse <= course.StartCourse)
                throw new Exception("EndDate must be after StartDate");

            var cor = new Course
            {
                Id = course.Id,
                CategoryId = course.CategoryId,
                CourseTitle = course.CourseTitle,
                CourseDescription = course.CourseDescription,
                StartDate = course.StartCourse,
                EndDate = course.EndCourse,
                Price = course.Price,
            };
            await _courseRepo.Insert(cor);
            await _courseRepo.SaveChanges();
        }
        public async Task UpdateCourse(int Id, CourseUpdeteDto course)
        {
            var corobj = await _courseRepo.GetById(Id);
            if (corobj == null)
            {
                throw new Exception("Course not found");
            }
            if (corobj.Price < 0)
            {
                throw new Exception("Price must be greater than to 0");
            }
            if (course.EndCourse <= course.StartCourse)
            {
                throw new Exception("EndDate must be after StartDate");
            }
            if (corobj.EndDate >= corobj.StartDate)
                throw new Exception("Cannot update course after it has started");

            corobj.StartDate = course.StartCourse;
            corobj.EndDate = course.EndCourse;
            corobj.CourseDescription = course.Description;
            corobj.CourseTitle = course.Title;
            corobj.Price = course.Price;
            _courseRepo.Update(corobj);
            await _courseRepo.SaveChanges();

        }
        public async Task DeleteCourse(int id)
        {
            var course = await _courseRepo.GetAll()
                 .Include(x => x.Enrollments)
                 .FirstOrDefaultAsync(x => x.Id == id);
            if (course != null)
            {
                if (!course.Enrollments.Any())
                {
                    await _courseRepo.Delete(course);
                    await _courseRepo.SaveChanges();
                }
                else
                {
                    throw new Exception("Cannot delete course already have a student.");
                }

            }
            else
            {
                throw new Exception("Course not found");
            }
        }
        public async Task<CourseListDto> GetCourseById(int id)
        {
            var course = await _courseRepo.GetById(id);
            if (course == null)
            {
                throw new Exception("Course not fount");
            }
            var courDetails = new CourseListDto
            {
                CourseId = course.Id,
                Title = course.CourseTitle,
                Description = course.CourseDescription,
                Price = course.Price,
                StartDate = course.StartDate,
                EndDate = course.EndDate,

            };
            return courDetails;
        }

        public async Task<List<CourseFilterDto>> GetCourse(int categoryCode, CourseFilterDto filter)
        {
            var courses = _courseRepo.GetAll()
         .Include(c => c.Enrollments).ThenInclude(e => e.Student)
         .Include(c => c.category)
         .Where(c =>
             (categoryCode > 0 ? c.CategoryId == categoryCode : true) &&
             (!string.IsNullOrEmpty(filter.Title)
                 ? c.CourseTitle.Trim().ToLower().Contains(filter.Title.Trim().ToLower())
                 : true) &&
             (!string.IsNullOrEmpty(filter.Category)
                 ? c.category.Name.Trim().ToLower().Equals(filter.Category.Trim().ToLower())
                 : true)
         )
         .Select(c => new CourseFilterDto
         {
             Id = c.Id,
             Title = c.CourseTitle,
             Category = c.category.Name,
             Price = c.Price
         });

            return await courses.ToListAsync();
        }








    }
}


