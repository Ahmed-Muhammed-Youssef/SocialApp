using Domain.Entities;
using Mediator;
using Shared.Results;

namespace Application.Features.Posts.GetById;

public record GetPostByIdQuery(ulong PostId) : IQuery<Result<Post>>;
