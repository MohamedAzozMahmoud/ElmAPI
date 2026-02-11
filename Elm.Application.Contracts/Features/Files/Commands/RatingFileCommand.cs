using Elm.Domain.Enums;
using MediatR;

namespace Elm.Application.Contracts.Features.Files.Commands
{
    public record RatingFileCommand(int fileId, string userId, string comment, DoctorRating rating) : IRequest<Result<bool>>;
}
