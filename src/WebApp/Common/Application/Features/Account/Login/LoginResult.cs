using Application.DTOs.User;

namespace Application.Features.Account.Login;

public record LoginResult(UserDTO UserData, string Token);
