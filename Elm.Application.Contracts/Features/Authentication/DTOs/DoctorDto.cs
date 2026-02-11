namespace Elm.Application.Contracts.Features.Authentication.DTOs
{
    public record DoctorDto
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Title { get; set; }
        public int DoctorId { get; set; }
        public bool IsActived { get; set; }
    }
}
