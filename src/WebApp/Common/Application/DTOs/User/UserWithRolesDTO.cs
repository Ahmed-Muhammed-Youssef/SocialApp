namespace Application.DTOs.User
{
    public class UserWithRolesDTO
    {
        public string Email { get; set; }
        public List<string> Roles { get; set; }
    }
}
