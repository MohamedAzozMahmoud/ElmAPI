using Elm.Application.Contracts;
using Elm.Application.Contracts.Abstractions.Files;
using Elm.Application.Contracts.Abstractions.Realtime;
using Elm.Application.Contracts.Features.Files.DTOs;
using Elm.Application.Contracts.Features.Images.DTOs;
using Elm.Domain.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Elm.Infrastructure.Services.Files
{
    public class FileStorageService : IFileStorageService
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly AppDbContext context;

        private readonly INotificationService notificationService;
        private const string NotFoundMessage = "File not found";

        public FileStorageService(IWebHostEnvironment webHostEnvironment, AppDbContext context, INotificationService notificationService)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.context = context;
            this.notificationService = notificationService;
        }

        public async Task<Result<bool>> DeleteFile(string fileName, string folderName)
        {
            var result = await context.Files.SingleOrDefaultAsync(f => f.StorageName == fileName);
            if (result == null)
            {
                return Result<bool>.Failure(NotFoundMessage, 404);
            }
            using (var tr = await context.Database.BeginTransactionAsync())
            {
                try
                {
                    var uploadsFolderPath = Path.Combine(webHostEnvironment.WebRootPath, folderName);
                    var filePath = Path.Combine(uploadsFolderPath, fileName);
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                        context.Files.Remove(result);
                        await context.SaveChangesAsync();
                        await tr.CommitAsync();
                        return Result<bool>.Success(true);
                    }
                    return Result<bool>.Failure(NotFoundMessage, 404);
                }
                catch
                {
                    await tr.RollbackAsync();
                    return Result<bool>.Failure("An error occurred while deleting the file.");
                }

            }
        }



        public async Task<Result<string>> UploadFileAsync(int curriculumId, int uploadedById, string description, IFormFile file, string folderName)
        {
            using var tr = await context.Database.BeginTransactionAsync();
            try
            {
                var student = await context.Students.SingleOrDefaultAsync(s => s.Id == uploadedById);
                if (student == null)
                {
                    return Result<string>.Failure("Student not found.");
                }
                var curriculum = await context.Curriculums.FindAsync(curriculumId);
                if (curriculum == null)
                {
                    return Result<string>.Failure("Failed to add file.");
                }
                var uploadsFolderPath = Path.Combine(webHostEnvironment.WebRootPath, folderName);
                if (!Directory.Exists(uploadsFolderPath))
                {
                    Directory.CreateDirectory(uploadsFolderPath);
                }
                var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
                var filePath = Path.Combine(uploadsFolderPath, uniqueFileName);
                var currerum = await context.Curriculums.SingleOrDefaultAsync(c => c.Id == curriculumId);
                if (currerum == null)
                {
                    return Result<string>.Failure("Curriculum not found");
                }
                await context.Files.AddAsync(new Domain.Entities.Files
                {
                    Name = file.FileName,
                    CurriculumId = curriculumId,
                    ContentType = file.ContentType,
                    Size = file.Length,
                    Description = description,
                    StorageName = uniqueFileName,
                    ProfessorRating = DoctorRating.NotRated,
                    UploadedById = uploadedById
                });
                context.Curriculums.Update(currerum);
                await context.SaveChangesAsync();
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                var doctor = await context.Doctors.SingleOrDefaultAsync(d => d.Id == currerum.DoctorId);
                await tr.CommitAsync();
                if (doctor != null && doctor.AppUserId != null)
                {
                    await notificationService.SendNotificationToUser(doctor.AppUserId, $"تم رفع الملخص من قبل طالب", $"اسم الملخص: {file.FileName}");
                }
                return Result<string>.Success(uniqueFileName);
            }
            catch
            {
                await tr.RollbackAsync();
                return Result<string>.Failure("An error occurred while uploading the file.");
            }
        }

        private static string GetContentType(string fileName)
        {
            var ext = Path.GetExtension(fileName).ToLowerInvariant();
            return ext switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".bmp" => "image/bmp",
                ".webp" => "image/webp",
                ".svg" => "image/svg+xml",
                ".pdf" => "application/pdf",
                ".doc" => "application/msword",
                ".txt" => "text/plain",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                _ => "application/octet-stream", // القيمة الافتراضية بدلاً من NULL
            };
        }

        public async Task<Result<ImageDto>> GetFileAsync(string fileName, string folderName)
        {
            var uploadsFolderPath = Path.Combine(webHostEnvironment.WebRootPath, folderName);
            var filePath = Path.Combine(uploadsFolderPath, fileName);
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(NotFoundMessage, fileName);
            }
            var fileData = await File.ReadAllBytesAsync(filePath);
            var contentType = GetContentType(fileName);
            return Result<ImageDto>.Success(new ImageDto { Content = fileData, ContentType = contentType });
        }

        public async Task<Result<bool>> DeleteAllFilesByCurriculumId(int curriculumId, string folderName)
        {
            using var tr = await context.Database.BeginTransactionAsync();
            try
            {
                var files = await context.Files.Where(f => f.CurriculumId == curriculumId).ToListAsync();
                if (files.Count == 0)
                {
                    return Result<bool>.Failure("No files found for the specified curriculum.");
                }
                var uploadsFolderPath = Path.Combine(webHostEnvironment.WebRootPath, folderName);
                foreach (var file in files)
                {
                    var filePath = Path.Combine(uploadsFolderPath, file.StorageName);
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }
                context.Files.RemoveRange(files);
                await context.SaveChangesAsync();
                await tr.CommitAsync();
                return Result<bool>.Success(true);
            }
            catch
            {
                await tr.RollbackAsync();
                return Result<bool>.Failure("An error occurred while deleting the files.");
            }
        }

        public async Task<Result<string>> UploadImageAsync(IFormFile file, string folderName)
        {
            try
            {
                var uploadsFolderPath = Path.Combine(webHostEnvironment.WebRootPath, folderName);
                if (!Directory.Exists(uploadsFolderPath))
                {
                    Directory.CreateDirectory(uploadsFolderPath);
                }
                var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
                var filePath = Path.Combine(uploadsFolderPath, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                return Result<string>.Success(uniqueFileName);
            }
            catch
            {
                return Result<string>.Failure("An error occurred while uploading the image.");
            }
        }

        public async Task<Result<bool>> DeleteUniversityImageAsync(int universityId, int id, string folderName)
        {
            var result = await context.Universities.FirstOrDefaultAsync(f => f.Id == universityId);
            if (result == null)
            {
                return Result<bool>.Failure("University not found", 404);
            }
            var image = await context.Images.FirstOrDefaultAsync(i => i.Id == id);
            using var tr = await context.Database.BeginTransactionAsync();
            try
            {

                var uploadsFolderPath = Path.Combine(webHostEnvironment.WebRootPath, folderName);
                string filePath = string.Empty;
                context.Universities.Remove(result);
                if (image != null)
                {
                    filePath = Path.Combine(uploadsFolderPath, image.StorageName);
                    context.Images.Remove(image);
                }
                await context.SaveChangesAsync();
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                await tr.CommitAsync();
                return Result<bool>.Success(true);
            }
            catch
            {
                await tr.RollbackAsync();
                return Result<bool>.Failure("An error occurred while deleting the image.", 500);
            }
        }

        public async Task<Result<bool>> DeleteCollegeImageAsync(int collegeId, int id, string folderName)
        {
            var result = await context.Colleges.FirstOrDefaultAsync(f => f.Id == collegeId);
            if (result == null)
            {
                return Result<bool>.Failure("College not found", 404);
            }
            var image = await context.Images.FirstOrDefaultAsync(i => i.Id == id);
            using var tr = await context.Database.BeginTransactionAsync();
            try
            {
                var uploadsFolderPath = Path.Combine(webHostEnvironment.WebRootPath, folderName);
                context.Colleges.Remove(result);
                string filePath = string.Empty;
                if (image != null)
                {
                    filePath = Path.Combine(uploadsFolderPath, image.StorageName);
                    context.Images.Remove(image);
                }
                await context.SaveChangesAsync();
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                await tr.CommitAsync();
                return Result<bool>.Success(true);
            }
            catch
            {
                await tr.RollbackAsync();
                return Result<bool>.Failure("An error occurred while deleting the image.", 500);
            }
        }

        public async Task<Result<FileResponse>> DownloadFileAsync(string fileName, string folderName)
        {
            var result = await context.Files.SingleOrDefaultAsync(f => f.StorageName == fileName);
            var uploadsFolderPath = Path.Combine(webHostEnvironment.WebRootPath, folderName);
            var filePath = Path.Combine(uploadsFolderPath, fileName);
            if (!File.Exists(filePath) || result == null)
            {
                return Result<FileResponse>.Failure(NotFoundMessage, 404);
            }
            var fileData = await File.ReadAllBytesAsync(filePath);
            var contentType = GetContentType(fileName);
            return Result<FileResponse>.Success(new FileResponse { Content = fileData, ContentType = contentType, FileName = result.Name });
        }

        public async Task<Result<List<FileView>>> GetAllFilesByCurriculumIdAsync(int curriculumId, string folderName)
        {
            var files = await context.Files
                .Where(f => f.CurriculumId == curriculumId)
                .Select(f => new FileView
                {
                    Id = f.Id,
                    Name = f.Name,
                    StorageName = f.StorageName,
                    DoctorRatedName = f.ProfessorRating.ToString(),
                    comment = f.ProfessorComment,
                    RatedAt = f.RatedAt
                })
                .AsNoTracking()
                .ToListAsync();
            return Result<List<FileView>>.Success(files);
        }

        public async Task<Result<bool>> RatingFileAsync(int curriculumId, int ratedByDoctorId, int fileId, DoctorRating rating, string comment)
        {
            var file = await context.Files.Include(s => s.UploadedBy).SingleOrDefaultAsync(f => f.Id == fileId && f.CurriculumId == curriculumId);
            if (file == null)
            {
                return Result<bool>.Failure(NotFoundMessage, 404);
            }
            file.ProfessorRating = rating;
            file.ProfessorComment = comment;
            file.RatedAt = DateTime.UtcNow;
            file.RatedByDoctorId = ratedByDoctorId;
            context.Files.Update(file);
            var result = await context.SaveChangesAsync();
            if (result != 0)
            {
                await notificationService.SendNotificationToUser(file.UploadedBy.AppUserId, comment, $"{file.Name} تم تقييم الملخص");
            }
            return Result<bool>.Success(true);
        }
    }
}
