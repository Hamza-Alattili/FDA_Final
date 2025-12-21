using Application.Dtos.Auth;
using Application.Dtos.Student;
using Application.Services.Interfaces;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FDAcademy.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }
        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> StudentReg([FromBody] StudentregistrationDto student)
        {
            await _studentService.StudentReg(student);
            return Ok("Student registered successfully");
        }
        [Authorize(Roles = FDAConst.STUDENT_ROLE)]
        [HttpPost("StudentUpdate/{id}")]
        public async Task<IActionResult> StudentUpdate(int id, [FromBody] StudentUpdateDto student)
        {
            await _studentService.StudentUpdate(id, student);
            return Ok("Student updated successfully");
        }
        [Authorize(Roles = FDAConst.ADMIN_ROLE)]
        [HttpGet("GetStudentById/{id}")]
        public async Task<IActionResult> GetStudentById(int id)
        {
            var student = await _studentService.GetStudentById(id);
            if (student == null) return NotFound("Student not found");
            return Ok(student);

        }
        [Authorize(Roles = FDAConst.ADMIN_ROLE)]
        [HttpGet("GetStudentList")]
        public async Task<ActionResult<List<StudentListDto>>> GetList()
        {
            var students = await _studentService.GetStudentList();
            return Ok(students);
        }
        [Authorize(Roles = FDAConst.STUDENT_ROLE)]
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPassword)
        {
            await _studentService.ResetPassword(resetPassword);
            return Ok("Password reset successfully");
        }
        [Authorize(Roles = FDAConst.ADMIN_ROLE)]
        [HttpDelete("DeleteStudent/{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            await _studentService.DeleteStudent(id);
            return Ok("Student deleted successfully");
        }
    }
}
