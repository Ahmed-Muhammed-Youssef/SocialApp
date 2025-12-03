namespace Application.Features.Auth.Register;

public record RegisterDTO(UserDTO UserData, string Token, string RefreshToken);
