namespace Elm.Application.Contracts.Features.Roles.DTOs
{
    public record UpdateRoleDto
    {
        public string OldName { get; set; }
        public string NewName { get; set; }
    }
}
