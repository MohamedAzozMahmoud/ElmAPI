using Elm.Application.Contracts.Features.Files.DTOs;
using Elm.Application.Contracts.Features.Images.DTOs;
using Elm.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Elm.Application.Contracts.Abstractions.Files
{
    public interface IFileStorageService
    {
        public Task<Result<string>> UploadFileAsync(int curriculumId, int uploadedById, string description, IFormFile file, string folderName);

        public Task<Result<string>> UploadImageAsync(IFormFile file, string folderName);
        // ميثود لحذف ملف
        public Task<Result<bool>> DeleteFile(string fileName, string folderName);
        public Task<Result<bool>> DeleteUniversityImageAsync(int universityId, int id, string folderName);
        public Task<Result<bool>> DeleteCollegeImageAsync(int collegeId, int id, string folderName);
        public Task<Result<bool>> DeleteAllFilesByCurriculumId(int curriculumId, string folderName);

        public Task<Result<ImageDto>> GetFileAsync(string fileName, string folderName);
        public Task<Result<FileResponse>> DownloadFileAsync(string fileName, string folderName);

        public Task<Result<List<FileView>>> GetAllFilesByCurriculumIdAsync(int curriculumId, string folderName);

        public Task<Result<bool>> RatingFileAsync(int curriculumId, int ratedByDoctorId, int fileId, DoctorRating rating, string comment);

    }
}
