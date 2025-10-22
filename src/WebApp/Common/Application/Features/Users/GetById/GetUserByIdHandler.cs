using Application.Interfaces;
using Mediator;
using Shared.Results;

namespace Application.Features.Users.GetById;

public class GetUserByIdHandler(IUnitOfWork _unitOfWork) : IQueryHandler<GetUserByIdQuery, Result<UserDTO>>
{
    public async ValueTask<Result<UserDTO>> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
    {
        UserDTO? user = await _unitOfWork.ApplicationUserRepository.GetDtoByIdAsync(query.Id);

        if (user == null)
        {
            return Result<UserDTO>.NotFound();
        }

        return Result<UserDTO>.Success(user);
    }
}
