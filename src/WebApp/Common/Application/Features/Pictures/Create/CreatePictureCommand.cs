using Mediator;
using Microsoft.AspNetCore.Http;
using Shared.Results;

namespace Application.Features.Pictures.Create;

public record CreatePictureCommand(IFormFile File) : ICommand<Result<int>>;
