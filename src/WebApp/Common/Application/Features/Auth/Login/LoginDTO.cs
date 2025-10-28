using Application.Features.Users;

namespace Application.Features.Auth.Login;

public record LoginDTO(UserDTO UserData, string Token);
