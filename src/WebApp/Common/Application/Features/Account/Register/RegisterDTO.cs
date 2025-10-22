using Application.Features.Users;

namespace Application.Features.Account.Register;

public record RegisterDTO(UserDTO UserData, string Token);
