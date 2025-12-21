using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Student
    {
        public int Id { get; set; }
        public DateOnly BirthDate { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string University { get; set; }
        [Required]
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }

    }
}
