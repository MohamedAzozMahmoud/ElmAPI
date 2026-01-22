using Elm.Application.Contracts.Features.Images.DTOs;
using MediatR;

namespace Elm.Application.Contracts.Features.Images.Queries
{
    public record GetAllImageQuery(string path) : IRequest<Result<List<ImageDto>>>;
}
