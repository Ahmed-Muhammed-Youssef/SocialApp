namespace Application.Features.DirectChats.SendMessage;

public record SendMessageResult(MessageDTO MessageDTO, UserDTO UserDTO, string GroupName);
