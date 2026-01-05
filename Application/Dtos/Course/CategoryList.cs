using Domain.Entities.Enum;

namespace Application.Dtos.Course
{
    public class CategoryList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CategoryEnum Code { get; set; }
    }
}
