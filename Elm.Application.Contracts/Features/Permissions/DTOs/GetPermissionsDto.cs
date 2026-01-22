namespace Elm.Application.Contracts.Features.Permissions.DTOs
{
    public record GetPermissionsDto
    {
        public int PermissionId { get; set; }
        public string PermissionName { get; set; }
    }
}
