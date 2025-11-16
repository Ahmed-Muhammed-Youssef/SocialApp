namespace Application.Features.Posts.GetById;

public class GetPostByIdHandler(IUnitOfWork unitOfWork) : IQueryHandler<GetPostByIdQuery, Result<Post>>
{
    public async ValueTask<Result<Post>> Handle(GetPostByIdQuery query, CancellationToken cancellationToken)
    {
        Post? post = await unitOfWork.PostRepository.GetByIdAsync(query.PostId, cancellationToken);

        if (post == null)
        {
            return Result<Post>.NotFound($"Post with ID {query.PostId} was not found.");
        }

        return Result<Post>.Success(post);
    }
}
