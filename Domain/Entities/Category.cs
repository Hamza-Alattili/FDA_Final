using Domain.Entities.Enum;

namespace Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CategoryEnum Code { get; set; }
    }
}
