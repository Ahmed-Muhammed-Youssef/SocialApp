namespace Application.Features.Auth.Login;

public record LoginDTO(UserDTO UserData, string Token, string RefreshToken, DateTime RefreshTokenExpiresAtUtc);
