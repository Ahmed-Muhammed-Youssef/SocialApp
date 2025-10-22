using Application.Features.Users;

namespace Application.Features.Account.Login;

public record LoginDTO(UserDTO UserData, string Token);
