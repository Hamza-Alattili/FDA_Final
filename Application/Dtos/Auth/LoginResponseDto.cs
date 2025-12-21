using Domain.Entities.Enum;

namespace Application.Dtos.Auth
{
    public class LoginResponseDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public LoginRoleDto Role { get; set; }
    }

    public class LoginRoleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public RoleEnum Code { get; set; }
    }

}
