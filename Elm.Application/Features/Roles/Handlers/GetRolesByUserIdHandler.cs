using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Roles.Queries;
using Elm.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Elm.Application.Features.Roles.Handlers
{
    public sealed class GetRolesByUserIdHandler : IRequestHandler<GetRolesByUserIdQuery, Result<IEnumerable<string>>>
    {
        private readonly UserManager<AppUser> userManager;
        public GetRolesByUserIdHandler(UserManager<AppUser> _userManager)
        {
            userManager = _userManager;
        }
        public async Task<Result<IEnumerable<string>>> Handle(GetRolesByUserIdQuery request, CancellationToken cancellationToken)
        {
            var user = userManager.Users.SingleOrDefault(x => x.Id == request.userId);
            if (user == null)
            {
                return Result<IEnumerable<string>>.Failure("Not found user");
            }
            var strings = userManager.GetRolesAsync(user).Result.ToList();
            if (strings == null)
            {
                return Result<IEnumerable<string>>.Failure("Not found roles");
            }
            return Result<IEnumerable<string>>.Success(strings);
        }
    }
}
