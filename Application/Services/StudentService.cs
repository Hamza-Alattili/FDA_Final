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
                University = student.University
            });
            await _studentRepo.SaveChanges();
        }
        public async Task StudentUpdate(StudentUpdateDto student)
        {
            var currentUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var studentUser = await _userRepo.GetById(Convert.ToInt32(currentUserId));
            if (studentUser == null)
            {
                throw new Exception("User not found");
            }

            var studentObj = await _studentRepo.GetAll().FirstOrDefaultAsync(x => x.UserId == Convert.ToInt32(currentUserId));
            if (studentObj == null)
            {
                throw new Exception("User not found");
            }

            studentUser.Name = student.FullName;
            studentUser.Email = student.Email;
            studentUser.PhoneNumber = student.PhoneNumber;

            studentObj.BirthDate = student.BirthDate;
            studentObj.University = student.University;

            _userRepo.Update(studentUser);
            _studentRepo.Update(studentObj);
            await _userRepo.SaveChanges();
        }


        public async Task<StudentListDto> GetStudentById(int Id)
        {
            var student = await _studentRepo.GetAll().Include(x => x.User).FirstOrDefaultAsync();

            return student == null ? null : new StudentListDto
            {
                StudentId = student.Id,
                FullName = student.User.FullName,
                Email = student.User.Email,
                PhoneNumber = student.User.PhoneNumber,
                BirthDate = student.BirthDate
            };
        }

        public async Task<List<StudentListDto>> GetStudentList()
        {
            var students = await _studentRepo.GetAll().Include(x => x.User).Select
                (students => new StudentListDto
                {
                    StudentId = students.Id,
                    FullName = students.User.FullName,
                    Email = students.User.Email,
                    PhoneNumber = students.User.PhoneNumber,
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

            var userIdClaim = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdClaim))
                throw new Exception("No userId claim found in token.");

            if (!int.TryParse(userIdClaim, out var stuId))
                throw new Exception("Invalid userId claim.");

            var user = await _userRepo.GetById(stuId);
            if (user == null)
                throw new Exception("user not found.");

            if (string.IsNullOrEmpty(user.Password))
                throw new Exception("user password is not set.");

            var passwordHasher = new PasswordHasher<User>();
            var passwordResult = passwordHasher.VerifyHashedPassword(user, user.Password, input.OldPassword);

            if (passwordResult == PasswordVerificationResult.Failed)
                throw new Exception("Old password is incorrect.");



            user.Password = passwordHasher.HashPassword(user, input.NewPassword);

            _userRepo.Update(user);
            await _userRepo.SaveChanges();

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
