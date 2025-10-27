using Mediator;
using Shared.Results;

namespace Application.Features.Messages.Delete;

public record MessageDeleteCommand(int Id) : ICommand<Result<object?>>;