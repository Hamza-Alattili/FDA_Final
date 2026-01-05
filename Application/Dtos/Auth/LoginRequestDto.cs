using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Auth
{
    public class LoginRequestDto 
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
