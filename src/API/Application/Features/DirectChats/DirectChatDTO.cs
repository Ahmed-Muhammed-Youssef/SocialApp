namespace Application.Features.DirectChats;

public record DirectChatDTO(int UserId, string UserFirstName, string UserLastName, MessageDTO LastMessage);