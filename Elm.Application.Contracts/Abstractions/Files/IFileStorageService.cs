using Elm.Application.Contracts.Features.Files.DTOs;
using Elm.Application.Contracts.Features.Images.DTOs;
using Elm.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Elm.Application.Contracts.Abstractions.Files
{
    public interface IFileStorageService
    {
        public Task<Result<string>> UploadFileAsync(int curriculumId, int uploadedById, string description, IFormFile file, string folderName);
        public Task<Result<bool>> DeleteFile(string fileName);
        public Task<Result<bool>> DeleteAllFilesByCurriculumId(int curriculumId);
        public Task<Result<ImageDto>> GetFileAsync(string fileName, string folderName);
        public Task<Result<FileResponse>> DownloadFileAsync(string fileName, string folderName);
        public Task<Result<List<FileView>>> GetAllFilesByCurriculumIdAsync(int curriculumId, string folderName);
        public Task<Result<bool>> RatingFileAsync(int curriculumId, int ratedByDoctorId, int fileId, DoctorRating rating, string comment);

        public Task<Result<string>> UploadImageAsync(IFormFile file, string folderName);
        public Task<Result<bool>> DeleteUniversityAsync(int universityId);
        public Task<Result<bool>> DeleteCollegeAsync(int collegeId);
        public Task<Result<bool>> DeleteImageAsync(string StorageName);
    }
}
