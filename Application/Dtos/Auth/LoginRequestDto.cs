using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Auth
{
    public class LoginRequestDto 
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
