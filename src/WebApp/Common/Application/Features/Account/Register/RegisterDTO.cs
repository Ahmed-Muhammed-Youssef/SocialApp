using Application.DTOs.User;

namespace Application.Features.Account.Register;

public record RegisterDTO(UserDTO UserData, string Token);
