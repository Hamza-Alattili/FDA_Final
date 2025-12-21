using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        [MaxLength(200)]
        [MinLength(3)]
        [Required]
        public string Name { get; set; }
        [MinLength(8)]
        [Required]
        public string Password { get; set; }
        [MaxLength(200)]
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(10)]
        public string PhoneNumber { get; set; }
        public int RoleId { get; set; }
        [ForeignKey("RoleId")]
        public Role Role { get; set; }
        [Required]
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public string? University { get; set; }
    }
}
