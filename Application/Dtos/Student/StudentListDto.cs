using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Student
{
    public class StudentListDto
    {
        public int StudentId { get; set; }
        public string FullName { get; set; }
        public string Email{ get; set; }
        public string PhoneNumber { get; set; }
        public DateTime BirthDate { get; set; }

    }
}
