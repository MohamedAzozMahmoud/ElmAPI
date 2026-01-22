using Elm.Domain.Enums;

namespace Elm.Domain.Entities
{
    public class Files
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string StorageName { get; set; } = null!;
        public string ContentType { get; set; } = null!;
        public long Size { get; set; }

        // --- حقول تقييم الدكتور الجديدة ---
        public DoctorRating ProfessorRating { get; set; } = DoctorRating.NotRated;
        public string? ProfessorComment { get; set; } // ملاحظة تعليمية من الدكتور
        public DateTime? RatedAt { get; set; } // تاريخ التقييم

        // ربط بالدكتور الذي قيم (للتوثيق)
        public int? RatedByDoctorId { get; set; }
        public Doctor? RatedByDoctor { get; set; }
        //UploadedBy
        public int UploadedById { get; set; }
        public Student UploadedBy { get; set; }
        public int CurriculumId { get; set; }
        public Curriculum Curriculum { get; set; }
    }
}
