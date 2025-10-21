namespace Application.DTOs.User
{
    public class UserWithRolesDTO
    {
        public required string Email { get; set; }
        public List<string> Roles { get; set; } = [];
    }
}
