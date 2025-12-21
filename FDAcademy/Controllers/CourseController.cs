using Application.Dtos.Course;
using Application.Services.Interfaces;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FDAcademy.Controllers
{
    [Authorize(Roles = FDAConst.ADMIN_ROLE)]
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : Controller
    {
        private readonly ICourseServicecs _courseService;

        public CourseController(ICourseServicecs courseService)
        {
            _courseService = courseService;
        }
        [HttpPost("CreateCourse")]
        public async Task<IActionResult> CreateCourse([FromBody] CreateCourseDto course)
        {
            await _courseService.CreateCourse(course);
            return Ok("Course created successfully");
        }
        [HttpPost("UpdateCourse/{Id}")]
        public async Task<IActionResult> UpdateCourse(int Id, [FromBody] CourseUpdeteDto course)
        {
            await _courseService.UpdateCourse(Id, course);
            return Ok("Course updated successfully");
        }
        [HttpDelete("DeleteCourse/{Id}")]
        public async Task<IActionResult> DeleteCourse(int Id)
        {
            await _courseService.DeleteCourse(Id);
            return Ok("Course deleted successfully");
        }
        [Authorize(Roles = $"{FDAConst.ADMIN_ROLE},{FDAConst.STUDENT_ROLE}")]
        [HttpGet("GetCoursrfilter")]
        public async Task<ActionResult<List<CourseFilterDto>>> GetCourses(int categoryCode,[FromBody] CourseFilterDto filter)
        {
            var courses = await _courseService.GetCourse(categoryCode, filter);
            return Ok(courses);
        }
        [Authorize(Roles = $"{FDAConst.ADMIN_ROLE},{FDAConst.STUDENT_ROLE}")]
        [HttpGet("GetCoursrById/{Id}")]
        public async Task<IActionResult> GetCoursrById(int Id)
        {
            var course = await _courseService.GetCourseById(Id);
            return Ok(course);

        }
    }
}
