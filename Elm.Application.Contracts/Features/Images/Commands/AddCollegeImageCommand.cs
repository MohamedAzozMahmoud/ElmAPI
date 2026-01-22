using MediatR;
using Microsoft.AspNetCore.Http;

namespace Elm.Application.Contracts.Features.Images.Commands
{
    public record AddCollegeImageCommand(int id, IFormFile File) : IRequest<Result<bool>>;

}
