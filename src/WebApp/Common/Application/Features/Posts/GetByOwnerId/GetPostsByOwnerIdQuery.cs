using Domain.Entities;
using Mediator;
using Shared.Results;

namespace Application.Features.Posts.GetByOwnerId;

public record GetPostsByOwnerIdQuery(int UserId) : IQuery<Result<IEnumerable<Post>>>;
