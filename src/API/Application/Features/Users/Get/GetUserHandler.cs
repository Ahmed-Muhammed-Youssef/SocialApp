namespace Application.Features.Users.Get;

public class GetUserHandler(IUnitOfWork _unitOfWork) : IQueryHandler<GetUserQuery, Result<UserDTO>>
{
    public async ValueTask<Result<UserDTO>> Handle(GetUserQuery query, CancellationToken cancellationToken)
    {
        UserDTO? user = await _unitOfWork.ApplicationUserRepository.GetDtoByIdAsync(query.Id);

        if (user == null)
        {
            return Result<UserDTO>.NotFound();
        }

        return Result<UserDTO>.Success(user);
    }
}
