using Application.Dtos.Auth;
using Application.Dtos.Student;
using Application.Repositories.Interface;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Entities.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace Application.Services
{
    public class StudentService : IStudentService
    {
        private readonly IGenericRepository<Student> _studentRepo;
        private readonly IGenericRepository<User> _userRepo;
        private readonly IGenericRepository<Role> _roleRepo;
        private readonly IGenericRepository<Course> _courseRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public StudentService(IGenericRepository<Student> studentRepo, IGenericRepository<User> userRepo, IGenericRepository<Role> roleRepo, IGenericRepository<Course> courseRepo, IHttpContextAccessor httpContextAccessor)
        {
            _studentRepo = studentRepo;
            _userRepo = userRepo;
            _roleRepo = roleRepo;
            _courseRepo = courseRepo;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task StudentReg(StudentregistrationDto student)
        {
            string passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
            bool passwordValidate = Regex.IsMatch(student.Password, passwordPattern);
            if (!passwordValidate)
            {
                throw new Exception("Passowrd is weaks");
            }

            string emailPattern = @"^[a-zA-Z0-9._%+\-]+@[a-zA-Z0-9.\-]+\.[A-Za-z]{2,}$";
            bool emailValidate = Regex.IsMatch(student.Email, emailPattern);
            if (!emailValidate)
            {
                throw new Exception("Email is not valid");
            }
            string mobilePattern = @"^(?:\+?962|00962)?0?7[7-9]\d{7}$";
            bool mobileValidate = Regex.IsMatch(student.PhoneNumber, mobilePattern);
            if (!mobileValidate)
            {
                throw new Exception("phone is Wrong");
            }


            var studentRoleId = (await _roleRepo.GetAll()
              .FirstOrDefaultAsync(s => s.Code == RoleEnum.Student))?.Id;

            var userObj = new User();
            userObj.Name = student.FullName;
            userObj.FullName = student.FullName;
            userObj.Email = student.Email;
            userObj.PhoneNumber = student.PhoneNumber;
            userObj.RoleId = studentRoleId.Value;

            var passwordHasher = new PasswordHasher<User>();
            userObj.Password = passwordHasher.HashPassword(userObj, student.Password);

            await _userRepo.Insert(userObj);
            await _userRepo.SaveChanges();

            await _studentRepo.Insert(new Student
            {
                UserId = userObj.Id,
                BirthDate = student.Birthdate,
                University = student.University,
                Password = userObj.Password
            });
            await _studentRepo.SaveChanges();


        }
        public async Task StudentUpdate(int Id, StudentUpdateDto student)
        {
            var students = await _userRepo.GetById(Id);
            if (students == null)
            {
                throw new Exception("Student not found");
            }
            if (students.Role.Code == RoleEnum.Admin)
            {
                throw new UnauthorizedAccessException("Students cannot edit admin data");
            }
            var currentUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (currentUserId != Id.ToString())
            {
                throw new UnauthorizedAccessException("You can only edit your own data");
            }

            students.Name = student.FullName;
            students.BirthDate = student.BirthDate;
            students.University = student.University;
            students.Email = student.Email;
            students.PhoneNumber = student.PhoneNumber;



            _userRepo.Update(students);
            await _userRepo.SaveChanges();
        }


        public async Task<StudentListDto> GetStudentById(int Id)
        {
            var student = await _userRepo.GetById(Id);

            return student == null ? null : new StudentListDto
            {
                StudentId = student.Id,
                FullName = student.FullName,
                Email = student.Email,
                PhoneNumber = student.PhoneNumber,
                BirthDate = student.BirthDate
            };
        }

        public Task<List<StudentListDto>> GetStudentList()
        {
            var students = _studentRepo.GetAll().Select
                (students => new StudentListDto
                {
                    StudentId = students.Id,
                    FullName = students.FullName,
                    Email = students.Email,
                    PhoneNumber = students.PhoneNumber,
                    BirthDate = students.BirthDate,
                }
                ).ToListAsync();
            return students;
        }

        public async Task ResetPassword(ResetPasswordDto input)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null || httpContext.User == null)
                throw new Exception("No HttpContext or User found.");

            var studentIdClaim = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(studentIdClaim))
                throw new Exception("No studentId claim found in token.");

            if (!int.TryParse(studentIdClaim, out var stuId))
                throw new Exception("Invalid studentId claim.");

            var student = await _studentRepo.GetById(stuId);
            if (student == null)
                throw new Exception("Student not found.");

            if (string.IsNullOrEmpty(student.Password))
                throw new Exception("Student password is not set.");

            var passwordHasher = new PasswordHasher<Student>();
            var passwordResult = passwordHasher.VerifyHashedPassword(student, student.Password, input.OldPassword);

            if (passwordResult != PasswordVerificationResult.Success)
                throw new Exception("Old password is incorrect.");

           

            student.Password = passwordHasher.HashPassword(student, input.NewPassword);

            _studentRepo.Update(student);
            await _studentRepo.SaveChanges();

        }



        public async Task DeleteStudent(int Id)
        {
            var student = await _studentRepo.GetById(Id);
            if (student == null)
            {
                throw new Exception("Student not found");
            }
            await _studentRepo.Delete(student);
            await _studentRepo.SaveChanges();
        }
    }
}
