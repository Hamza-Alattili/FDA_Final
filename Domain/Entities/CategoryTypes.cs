using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class CategoryTypes
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Code { get; set; }
        public ICollection<Course> courses { get; set; }
    }
}
