using Elm.Application.Contracts;
using Elm.Application.Contracts.Abstractions.Files;
using Elm.Application.Contracts.Abstractions.Realtime;
using Elm.Application.Contracts.Abstractions.Settings;
using Elm.Application.Contracts.Features.Files.DTOs;
using Elm.Application.Contracts.Features.Images.DTOs;
using Elm.Domain.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Elm.Infrastructure.Services.Files
{
    public class FileStorageService : IFileStorageService
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly AppDbContext context;
        private readonly INotificationService notificationService;
        private const string NotFoundMessage = "File not found";
        private readonly ISettingsService settingsService;
        private readonly ILogger<FileStorageService> logger;

        public FileStorageService(ILogger<FileStorageService> logger, ISettingsService settingsService,
            IWebHostEnvironment webHostEnvironment, AppDbContext context,
             INotificationService _notificationService)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.context = context;
            notificationService = _notificationService;
            this.settingsService = settingsService;
            this.logger = logger;

        }

        public async Task<Result<bool>> DeleteFile(string fileName)
        {
            var result = await context.Files.SingleOrDefaultAsync(f => f.StorageName == fileName);
            if (result == null)
            {
                return Result<bool>.Failure(NotFoundMessage, 404);
            }
            try
            {
                result.MarkAsDeleted();
                context.Files.Remove(result);
                await context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch
            {
                return Result<bool>.Failure("حدث خطأ أثناء حذف الملف.");
            }
        }

        public async Task<Result<string>> UploadFileAsync(int curriculumId, int uploadedById, string description, IFormFile file, string folderName)
        {
            using var tr = await context.Database.BeginTransactionAsync();
            try
            {
                // 1. التحققات الأساسية (استخدم AnyAsync للسرعة)
                if (!await context.Students.AnyAsync(s => s.Id == uploadedById))
                    return Result<string>.Failure("الطالب غير موجود.");

                var curriculum = await context.Curriculums.FindAsync(curriculumId);
                if (curriculum == null) return Result<string>.Failure("المنهج غير موجود", 404);

                // 2. فحص تكرار الاسم والقيود
                if (await context.Files.AnyAsync(f => f.Name == file.FileName && f.CurriculumId == curriculumId))
                    return Result<string>.Failure("الملف موجود بالفعل لهذا المنهج.");

                var maxFileSizeSetting = await settingsService.GetMaxStorageBytesAsync();
                var currentUsage = await context.Files.Where(f => f.CurriculumId == curriculumId).SumAsync(f => f.Size);

                if (file.Length + currentUsage > maxFileSizeSetting)
                    return Result<string>.Failure("حجم الملف يتجاوز الحد الأقصى المسموح به لهذا المنهج.");

                // 3. تجهيز الملف
                var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
                var uploadsFolderPath = Path.Combine(webHostEnvironment.WebRootPath, folderName);
                if (!Directory.Exists(uploadsFolderPath)) Directory.CreateDirectory(uploadsFolderPath);
                var filePath = Path.Combine(uploadsFolderPath, uniqueFileName);

                // 4. الحفظ في القاعدة
                await context.Files.AddAsync(new Domain.Entities.Files
                {
                    Name = file.FileName,
                    CurriculumId = curriculumId,
                    ContentType = file.ContentType,
                    Size = file.Length,
                    Description = description,
                    StorageName = uniqueFileName,
                    UploadedById = uploadedById
                });

                await context.SaveChangesAsync();

                // 5. كتابة الملف فيزيائياً
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                await tr.CommitAsync();

                // 6. الإشعار (Background Task)
                _ = Task.Run(async () =>
                {
                    var doctor = await context.Doctors.FirstOrDefaultAsync(d => d.Id == curriculum.DoctorId);
                    if (doctor?.AppUserId != null)
                        await notificationService.SendNotificationToUser(doctor.AppUserId, "تم رفع ملخص جديد", $"الملخص: {file.FileName}");
                });

                return Result<string>.Success(uniqueFileName);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error uploading file");
                await tr.RollbackAsync();
                // اختياري: حذف الملف من الهارد لو اتكريت وفشل الـ Transaction
                return Result<string>.Failure("حدث خطأ أثناء الرفع.", 500);
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
            // Validation
            if (string.IsNullOrWhiteSpace(fileName))
                return Result<ImageDto>.Failure("اسم الملف مطلوب", 400);

            if (string.IsNullOrWhiteSpace(folderName))
                return Result<ImageDto>.Failure("اسم المجلد مطلوب", 400);

            // Path Traversal Attack Prevention
            if (fileName.Contains("..") || fileName.Contains("/") || fileName.Contains("\\"))
                return Result<ImageDto>.Failure("اسم الملف غير صالح", 400);

            var uploadsFolderPath = Path.Combine(webHostEnvironment.WebRootPath, folderName);
            var filePath = Path.Combine(uploadsFolderPath, fileName);

            // Security: تأكد أن المسار النهائي داخل المجلد المسموح
            var fullPath = Path.GetFullPath(filePath);
            var allowedPath = Path.GetFullPath(uploadsFolderPath);

            if (!fullPath.StartsWith(allowedPath))
                return Result<ImageDto>.Failure("تم رفض الوصول", 403);

            if (!File.Exists(filePath))
                return Result<ImageDto>.Failure("الصورة غير موجودة", 404);

            try
            {
                var fileData = await File.ReadAllBytesAsync(filePath);
                var contentType = GetContentType(fileName);

                return Result<ImageDto>.Success(new ImageDto
                {
                    Content = fileData,
                    ContentType = contentType
                });
            }
            catch (Exception ex)
            {
                // Log the exception
                logger.LogError(ex, "Error reading file: {FileName}", fileName);
                return Result<ImageDto>.Failure($"حدث خطأ أثناء قراءة الملف: {ex.Message}", 500);
            }
        }

        public async Task<Result<bool>> DeleteAllFilesByCurriculumId(int curriculumId)
        {
            try
            {
                var files = await context.Files.Where(f => f.CurriculumId == curriculumId).ToListAsync();
                if (files.Count == 0)
                {
                    return Result<bool>.Failure("لا توجد ملفات للمناهج المحددة.", 404);
                }
                foreach (var file in files)
                {
                    file.MarkAsDeleted();
                }
                context.Files.RemoveRange(files);
                await context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch
            {
                return Result<bool>.Failure("حدث خطأ أثناء حذف الملفات.");
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

        public async Task<Result<bool>> DeleteUniversityAsync(int universityId)
        {
            var result = await context.Universities
                .Include(u => u.Img)
                .FirstOrDefaultAsync(f => f.Id == universityId);
            if (result == null)
            {
                return Result<bool>.Failure("الجامعة غير موجودة", 404);
            }
            try
            {
                if (result.ImgId is not null && result.Img is not null)
                {
                    result.Img.MarkAsDeleted();
                }
                context.Universities.Remove(result);
                await context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch
            {
                return Result<bool>.Failure("حدث خطأ أثناء حذف الصورة.", 500);
            }
        }

        public async Task<Result<bool>> DeleteCollegeAsync(int collegeId)
        {
            var result = await context.Colleges.Include(c => c.Img).FirstOrDefaultAsync(f => f.Id == collegeId);
            if (result == null)
            {
                return Result<bool>.Failure("الكلية غير موجودة", 404);
            }
            using var tr = await context.Database.BeginTransactionAsync();
            try
            {
                if (result.Img is not null && result.ImgId is not null)
                {
                    result.Img.MarkAsDeleted();
                }
                context.Colleges.Remove(result);
                await context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch
            {
                return Result<bool>.Failure("حدث خطأ أثناء حذف الصورة.", 500);
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
            await context.SaveChangesAsync();
            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> DeleteImageAsync(string StorageName)
        {
            // 1. جلب الصورة مع الكلية المرتبطة بها في طلب واحد
            var image = await context.Images
                .Include(i => i.College)
                .SingleOrDefaultAsync(i => i.StorageName == StorageName);

            if (image == null)
                return Result<bool>.Failure("لم يتم العثور على الصورة", 404);

            try
            {
                image.MarkAsDeleted();
                if (image.College != null)
                {
                    image.College.ImgId = null;
                }
                context.Images.Remove(image);
                await context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch
            {
                return Result<bool>.Failure("حدث خطأ أثناء حذف الصورة.", 500);
            }
        }

    }
}
