using Mediator;
using Shared.Results;

namespace Application.Features.Pictures.Delete;

public record DeletePictureCommand(int PictureId) : ICommand<Result<object?>>;
