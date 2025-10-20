using Application.DTOs.User;

namespace Application.Features.Account.Login;

public record LoginDTO(UserDTO UserData, string Token);
