using Application.Common.Interfaces;
using Mediator;
using Shared.Results;

namespace Application.Features.Messages.Delete;

public class MessageDeleteHandler(IUnitOfWork unitOfWork, ICurrentUserService currentUserService) : ICommandHandler<MessageDeleteCommand, Result<object?>>
{
    public async ValueTask<Result<object?>> Handle(MessageDeleteCommand command, CancellationToken cancellationToken)
    {
        var message = await unitOfWork.MessageRepository.GetByIdAsync(command.Id, cancellationToken);
        if (message == null)
        {
            return Result<object?>.NotFound();
        }
        var issuerId = currentUserService.GetPublicId();

        throw new NotImplementedException();

        // Checks if the message is related to the issuer
        //if (message.SenderId != issuerId && message.RecipientId != issuerId)
        //{
        //    return Result<object?>.Error("Failed to delete the message");
        //}

        //unitOfWork.MessageRepository.DeleteMessage(message, issuerId);

        //await unitOfWork.SaveChangesAsync();

        //return Result<object?>.NoContent();
    }
}
