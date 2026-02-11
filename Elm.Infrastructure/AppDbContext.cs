using Elm.Domain.Common;
using Elm.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Elm.Infrastructure
{
    public class AppDbContext : IdentityDbContext<AppUser, Role, string>
    {
        private readonly IMediator _mediator;

        public AppDbContext(DbContextOptions<AppDbContext> options, IMediator mediator) : base(options)
        {
            _mediator = mediator;
        }

        public DbSet<Settings> Settings { get; set; }

        #region Academic
        public DbSet<University> Universities { get; set; }
        public DbSet<College> Colleges { get; set; }
        public DbSet<Year> Years { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Curriculum> Curriculums { get; set; }

        #endregion

        #region Users
        public DbSet<Student> Students { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Elm.Domain.Entities.Notifications> Notifications { get; set; }

        #endregion

        #region Exams
        public DbSet<QuestionsBank> QuestionsBanks { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Option> Options { get; set; }

        #endregion

        #region Files   

        public DbSet<Files> Files { get; set; }
        public DbSet<Image> Images { get; set; }

        #endregion

        #region Authorization

        public DbSet<RefreshToken> RefreshToken { get; set; }
        public DbSet<Permissions> Permissions { get; set; }
        public DbSet<RolePermissions> RolePermissions { get; set; }
        public DbSet<UserPermissions> UserPermissions { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Settings>()
                .HasIndex(s => s.Key)
                .IsUnique();

            #region  University
            builder.Entity<University>()
                .HasOne(u => u.Img)
                .WithOne(i => i.University)
                .HasForeignKey<University>(u => u.ImgId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<University>()
                .HasIndex(u => u.Name)
                .IsUnique();
            #endregion

            #region College
            builder.Entity<College>()
                .HasOne(c => c.University)
                .WithMany(u => u.Colleges)
                .HasForeignKey(c => c.UniversityId);

            builder.Entity<College>()
                .HasOne(c => c.Img)
                .WithOne(i => i.College)
                .HasForeignKey<College>(c => c.ImgId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<College>()
                .HasIndex(c => c.Name)
                .IsUnique();

            #endregion

            #region Department
            builder.Entity<Department>()
                .HasOne(d => d.College)
                .WithMany(c => c.Departments)
                .HasForeignKey(d => d.CollegeId)
                .OnDelete(DeleteBehavior.Restrict);

            #endregion

            #region Subject 
            builder.Entity<Subject>()
                .HasIndex(s => s.Name)
                .IsUnique();

            builder.Entity<Subject>()
                .HasIndex(s => s.Code)
                .IsUnique();
            #endregion

            #region Questions

            builder.Entity<QuestionsBank>()
                .HasOne(qb => qb.Curriculum)
                .WithMany(c => c.QuestionsBanks)
                .HasForeignKey(qb => qb.CurriculumId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Question>()
                .HasOne(q => q.QuestionBank)
                .WithMany(qb => qb.Questions)
                .HasForeignKey(q => q.QuestionBankId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Option>()
                .HasOne(o => o.Question)
                .WithMany(q => q.Options)
                .HasForeignKey(o => o.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion

            #region Users

            builder.Entity<AppUser>()
                      .Property(u => u.FullName)
                      .IsRequired()
                      .HasMaxLength(150);

            builder.Entity<AppUser>()
                     .HasMany(u => u.RefreshTokens)
                     .WithOne(t => t.AppUser)
                     .HasForeignKey(t => t.AppUserId);

            #endregion

            #region Doctors

            builder.Entity<Doctor>()
                .HasOne(d => d.User)
                .WithOne(u => u.Doctor)
                .HasForeignKey<Doctor>(d => d.AppUserId)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion

            #region Notifications

            builder.Entity<Elm.Domain.Entities.Notifications>()
                .HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.AppUserId);

            #endregion

            #region Year
            builder.Entity<Year>()
                .HasOne(y => y.College)
                .WithMany(c => c.Years)
                .HasForeignKey(y => y.CollegeId)
                .OnDelete(DeleteBehavior.Restrict); // حماية الكلية من حذف سنواتها بالخطأ

            builder.Entity<Year>()
                .HasIndex(y => new { y.CollegeId, y.Name })
                .IsUnique(); // ضمان عدم تكرار أسماء السنوات داخل نفس الكلية
            #endregion

            #region Curriculum
            builder.Entity<Curriculum>(entity =>
            {
                // تم تغييرها لـ Restrict لضمان عدم حذف المنهج إذا حذفت المادة أو الدكتور بالخطأ
                entity.HasOne(c => c.Year).WithMany().HasForeignKey(c => c.YearId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(c => c.Subject).WithMany(s => s.Curriculums).HasForeignKey(c => c.SubjectId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(c => c.Department).WithMany(d => d.Curriculums).HasForeignKey(c => c.DepartmentId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(c => c.Doctor).WithMany(d => d.Curriculums).HasForeignKey(c => c.DoctorId).OnDelete(DeleteBehavior.Restrict);
            });
            #endregion

            #region Files

            builder.Entity<Files>()
                .HasOne(f => f.UploadedBy)
                .WithMany(x => x.UploadedFiles)
                .HasForeignKey(f => f.UploadedById)
                .OnDelete(DeleteBehavior.Restrict); // لضمان بقاء الملفات حتى لو حُذف حساب الرافع

            builder.Entity<Files>()
                .HasOne(f => f.Curriculum)
                .WithMany(x => x.Files)
                .HasForeignKey(f => f.CurriculumId)
                .OnDelete(DeleteBehavior.Cascade);

            // this is optional description
            builder.Entity<Files>()
                .Property(f => f.Description)
                  .IsRequired(false);

            builder.Entity<Files>()
                .HasOne(f => f.RatedByDoctor)
                .WithMany(d => d.RatedFiles)
                .HasForeignKey(f => f.RatedByDoctorId)
                .OnDelete(DeleteBehavior.Restrict); // لضمان بقاء الملفات حتى لو حُذف حساب الدكتور الذي قيم الملف

            #endregion

            #region Students
            builder.Entity<Student>()
                .HasOne(s => s.User)
                .WithOne(u => u.Student)
                .HasForeignKey<Student>(s => s.AppUserId)
                .OnDelete(DeleteBehavior.Cascade); // إذا حذف المستخدم يختفي سجل الطالب - سليم

            builder.Entity<Student>()
                .HasOne(s => s.Year)
                .WithMany(y => y.Students)
                .HasForeignKey(s => s.YearId)
                .OnDelete(DeleteBehavior.Restrict); // تعديل لـ Restrict لتجنب Multiple Cascade Paths

            builder.Entity<Student>()
                .HasOne(s => s.Department)
                .WithMany(d => d.Students)
                .HasForeignKey(s => s.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict); // تعديل لـ Restrict
            #endregion

            #region Permissions 
            builder.Entity<RolePermissions>()
                .HasOne(rp => rp.Role)
                .WithMany(x => x.RolePermissions)
                .HasForeignKey(rp => rp.RoleId);

            builder.Entity<RolePermissions>()
                .HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionId);

            builder.Entity<UserPermissions>()
                .HasOne(up => up.User)
                .WithMany(u => u.UserPermissions)
                .HasForeignKey(up => up.AppUserId);
            builder.Entity<UserPermissions>()
                .HasOne(up => up.Permission)
                .WithMany(u => u.UserPermissions)
                .HasForeignKey(up => up.PermissionId);

            #endregion


        }

        public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            // I want to when Delete an file or image delete the harde file from the storage
            // 1. التقاط كل الكيانات التي تحتوي على أحداث
            var entitiesWithEvents = ChangeTracker.Entries<BaseEntity>()
                .Select(e => e.Entity)
                .Where(e => e.DomainEvents.Any())
                .ToList();

            // 2. استخراج الأحداث
            var domainEvents = entitiesWithEvents.SelectMany(e => e.DomainEvents).ToList();

            // 3. حفظ التغييرات في قاعدة البيانات أولاً
            var result = await base.SaveChangesAsync(ct);

            // 4. إذا نجح الحفظ، قم بنشر الأحداث
            foreach (var @event in domainEvents)
            {
                await _mediator.Publish(@event, ct);
            }

            // 5. تنظيف الأحداث من الكيانات
            entitiesWithEvents.ForEach(e => e.ClearDomainEvents());

            return result;
        }
    }
}
