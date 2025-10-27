using Application.DTOs.Pagination;
using Mediator;
using Shared.Results;

namespace Application.Features.Pictures.List;

public record GetPicturesQuery : IQuery<Result<List<PictureDTO>>>;
