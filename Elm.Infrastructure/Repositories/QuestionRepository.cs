using Elm.Application.Contracts;
using Elm.Application.Contracts.Abstractions.Settings;
using Elm.Application.Contracts.Features.Options.DTOs;
using Elm.Application.Contracts.Features.Questions.DTOs;
using Elm.Application.Contracts.Features.Test.DTOs;
using Elm.Application.Contracts.Repositories;
using Elm.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Elm.Infrastructure.Repositories
{
    public class QuestionRepository : GenericRepository<Question>, IQuestionRepository
    {
        private readonly AppDbContext context;
        private readonly IMemoryCache memoryCache;
        private readonly ISettingsService settingsService;

        public QuestionRepository(AppDbContext _context, IMemoryCache _memoryCache, ISettingsService _settingsService) : base(_context)
        {
            context = _context;
            memoryCache = _memoryCache;
            settingsService = _settingsService;
        }

        public async Task<Result<bool>> AddRingQuestions(int questionsBankId, List<AddQuestionsDto> questionsDtos)
        {
            var maxQuestionsPerBank = await settingsService.GetMaxQuestionsAsync();
            var existingQuestionsCount = await CountQuestions(questionsBankId);
            if (existingQuestionsCount + questionsDtos.Count > maxQuestionsPerBank)
            {
                return Result<bool>.Failure($"لا يمكن إضافة الأسئلة. تجاوز الحد الأقصى ({maxQuestionsPerBank} سؤالًا في البنك).");
            }
            var questions = new List<Question>();
            foreach (var questionDto in questionsDtos)
            {
                var question = new Question
                {
                    Content = questionDto.Content,
                    QuestionType = Enum.Parse<Domain.Enums.QuestionType>(questionDto.QuestionType),
                    QuestionBankId = questionsBankId
                };
                question.Options = questionDto.Options.Select(o => new Option
                {
                    Content = o.Content,
                    IsCorrect = o.IsCorrect
                }).ToList();
                questions.Add(question);
            }
            context.Questions.AddRange(questions);
            await context.SaveChangesAsync();
            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> AddRingQuestionsFromExcel(int questionsBankId, List<TemplateQuestionsDto> templateQuestions)
        {
            var questions = new List<Question>();
            var maxQuestionsPerBank = await settingsService.GetMaxQuestionsAsync();
            var existingQuestionsCount = await CountQuestions(questionsBankId);
            if (existingQuestionsCount + templateQuestions.Count > maxQuestionsPerBank)
            {
                return Result<bool>.Failure($"لا يمكن إضافة الأسئلة. تجاوز الحد الأقصى ({maxQuestionsPerBank} سؤالًا في البنك).");
            }
            foreach (var templateQuestion in templateQuestions)
            {
                var question = new Question
                {
                    Content = templateQuestion.Content,
                    QuestionType = Enum.Parse<Domain.Enums.QuestionType>(templateQuestion.QuestionType),
                    QuestionBankId = questionsBankId
                };
                var options = new List<Option>();
                for (char optionLabel = 'A'; optionLabel <= 'D'; optionLabel++)
                {
                    var optionContent = templateQuestion.GetType().GetProperty($"Option{optionLabel}")?.GetValue(templateQuestion)?.ToString();
                    if (!string.IsNullOrEmpty(optionContent))
                    {
                        options.Add(new Option
                        {
                            Content = optionContent,
                            IsCorrect = templateQuestion.CorrectOption == optionLabel.ToString()
                        });
                    }
                    question.Options = options;
                    questions.Add(question);
                }
            }
            context.Questions.AddRange(questions);
            await context.SaveChangesAsync();
            return Result<bool>.Success(true);
        }
        public async Task<Result<List<QuestionsDto>>> GetQuestionsByBankId(int questionsBankId)
        {
            var questions = await context.Questions
                .AsNoTracking()
                .Where(q => q.QuestionBankId == questionsBankId)
                .Include(q => q.Options)
                .Select(q => new QuestionsDto
                {
                    Id = q.Id,
                    Content = q.Content,
                    QuestionType = q.QuestionType.ToString(),
                    Options = q.Options.Where(x => x.QuestionId == q.Id).Select(o => new OptionsDto
                    {
                        Id = o.Id,
                        Content = o.Content,
                        IsCorrect = o.IsCorrect
                    }).ToList()
                })
                .ToListAsync();
            return Result<List<QuestionsDto>>.Success(questions);
        }
        public async Task<Result<QuestionsDto>> GetQuestionById(int questionId)
        {
            var question = await context.Questions
                .AsNoTracking()
                .Where(q => q.Id == questionId)
                .Include(q => q.Options)
                .Select(q => new QuestionsDto
                {
                    Id = q.Id,
                    Content = q.Content,
                    QuestionType = q.QuestionType.ToString(),
                    Options = q.Options.Where(x => x.QuestionId == q.Id).Select(o => new OptionsDto
                    {
                        Id = o.Id,
                        Content = o.Content,
                        IsCorrect = o.IsCorrect
                    }).ToList()
                })
                .FirstOrDefaultAsync();
            return Result<QuestionsDto>.Success(question);
        }

        private async Task<int> CountQuestions(int questionsBankId)
        {
            return await context.Questions
                .AsNoTracking()
                .Where(q => q.QuestionBankId == questionsBankId)
                .CountAsync();
        }

        //public async Task<Result<TestDataDto>> GetRandomQuestionsByBankId(
        //    int questionsBankId,
        //    int numberOfQuestions)
        //{
        //    // 1️⃣ التحقق من البنك
        //    var bankInfo = await context.QuestionsBanks
        //        .AsNoTracking()
        //        .Where(qb => qb.Id == questionsBankId)
        //        .Select(qb => new { TotalCount = qb.Questions.Count() })
        //        .FirstOrDefaultAsync();

        //    if (bankInfo is null)
        //        return Result<TestDataDto>.Failure("بنك الأسئلة غير موجود.");

        //    if (bankInfo.TotalCount < numberOfQuestions)
        //        return Result<TestDataDto>.Failure(
        //            $"عدد الأسئلة غير كافٍ.  المتاح: {bankInfo.TotalCount}");

        //    // 2️⃣ جلب أسئلة عشوائية
        //    var randomIds = await context.Questions
        //        .FromSqlInterpolated(
        //            $@"SELECT TOP ({numberOfQuestions}) Id 
        //       FROM Questions 
        //       WHERE QuestionBankId = {questionsBankId} 
        //       ORDER BY NEWID()")
        //        .Select(q => q.Id)
        //        .ToListAsync();

        //    // 3️⃣ جلب البيانات مع الإجابات الصحيحة (بالـ ID مش Content)
        //    var questionsData = await context.Questions
        //        .AsNoTracking()
        //        .Where(q => randomIds.Contains(q.Id))
        //        .Select(q => new
        //        {
        //            QuestionId = q.Id,
        //            Content = q.Content,
        //            QuestionType = q.QuestionType.ToString(),
        //            Options = q.Options.Select(o => new
        //            {
        //                o.Id,
        //                o.Content,
        //                o.IsCorrect
        //            }).ToList()
        //        })
        //        .ToListAsync();

        //    // 4️⃣ إنشاء Session ID فريد
        //    var testSessionId = Guid.NewGuid();
        //    var duration = TimeSpan.FromMinutes(numberOfQuestions * 2);
        //    var expiresAt = DateTime.UtcNow.Add(duration).AddMinutes(1); // ✅ دقيقة إضافية للـ Buffer

        //    // 5️⃣ تجهيز البيانات للـ Cache (بالـ IDs)
        //    var correctOptionsByQuestion = questionsData.ToDictionary(
        //        q => q.QuestionId,
        //        q => q.Options
        //            .Where(o => o.IsCorrect)
        //            .Select(o => o.Id)      // ✅ ID مش Content
        //            .ToHashSet()
        //    );

        //    // 6️⃣ تخزين في الـ Cache بـ Session ID فريد
        //    var cachedSession = new CachedTestSession
        //    {
        //        QuestionBankId = questionsBankId,
        //        ExpiresAt = expiresAt,
        //        CorrectOptionsByQuestionId = correctOptionsByQuestion
        //    };

        //    memoryCache.Set(
        //        $"TestSession_{testSessionId}",  // ✅ Key فريد لكل طالب
        //        cachedSession,
        //        new MemoryCacheEntryOptions
        //        {
        //            AbsoluteExpiration = expiresAt
        //        });

        //    // 7️⃣ تجهيز الـ Response (بدون الإجابات الصحيحة)
        //    var questions = questionsData.Select(q => new QuestionTestDto
        //    {
        //        Id = q.QuestionId,
        //        Content = q.Content,
        //        QuestionType = q.QuestionType,
        //        Options = q.Options
        //            .OrderBy(_ => Random.Shared.Next())  // ✅ ترتيب عشوائي
        //            .Select(o => new OptionTestDto
        //            {
        //                Id = o.Id,
        //                Content = o.Content
        //            })
        //            .ToList()
        //    }).ToList();

        //    return Result<TestDataDto>.Success(new TestDataDto
        //    {
        //        TestSessionId = testSessionId,  // ✅ الطالب يرجعه وقت التسليم
        //        QuestionBankId = questionsBankId,
        //        Duration = duration,
        //        ExpiresAt = expiresAt,
        //        Questions = questions
        //    });
        //}

        //public Task<Result<TestResultDto>> SubmitTest(SubmitTestDto submission)
        //{
        //    // 1️⃣ جلب الـ Session من الـ Cache
        //    if (!memoryCache.TryGetValue(
        //            $"TestSession_{submission.TestSessionId}",  // ✅ نفس الـ Key
        //            out CachedTestSession? cachedSession)
        //        || cachedSession is null)
        //    {
        //        return Task.FromResult(
        //            Result<TestResultDto>.Failure("انتهى وقت الاختبار أو الجلسة غير موجودة. "));
        //    }

        //    // 2️⃣ التحقق من الوقت (حماية إضافية)
        //    if (DateTime.UtcNow > cachedSession.ExpiresAt)
        //    {
        //        memoryCache.Remove($"TestSession_{submission.TestSessionId}");
        //        return Task.FromResult(
        //            Result<TestResultDto>.Failure("انتهى وقت الاختبار. "));
        //    }

        //    // 3️⃣ حساب النتيجة
        //    int correctAnswers = 0;
        //    int totalQuestions = cachedSession.CorrectOptionsByQuestionId.Count;

        //    foreach (var answer in submission.Answers)
        //    {
        //        if (!cachedSession.CorrectOptionsByQuestionId
        //                .TryGetValue(answer.QuestionId, out var correctOptionIds))
        //        {
        //            continue; // سؤال غير موجود، تخطي
        //        }

        //        var selectedIds = answer.SelectedOptionIds.ToHashSet();

        //        // ✅ المقارنة بالـ IDs (أسرع وأدق)
        //        if (selectedIds.SetEquals(correctOptionIds))
        //        {
        //            correctAnswers++;
        //        }
        //    }

        //    // 4️⃣ حذف الـ Session بعد التسليم (منع إعادة التسليم)
        //    memoryCache.Remove($"TestSession_{submission.TestSessionId}");

        //    // 5️⃣ إرجاع النتيجة
        //    return Task.FromResult(Result<TestResultDto>.Success(new TestResultDto
        //    {
        //        TotalQuestions = totalQuestions,
        //        CorrectAnswers = correctAnswers,
        //        ScorePercentage = Math.Round((double)correctAnswers / totalQuestions * 100, 2)
        //    }));
        //}

        public async Task<QuestionBankInfo?> GetBankInfoAsync(int bankId)
        {
            return await context.QuestionsBanks
                .AsNoTracking()
                .Where(qb => qb.Id == bankId)
                .Select(qb => new QuestionBankInfo(qb.Questions.Count()))
                .FirstOrDefaultAsync();
        }

        public async Task<List<int>> GetRandomQuestionIdsAsync(int bankId, int count)
        {
            return await context.Questions
                .FromSqlInterpolated(
                    $@"SELECT TOP ({count}) Id 
                   FROM Questions 
                   WHERE QuestionBankId = {bankId} 
                   ORDER BY NEWID()")
                .Select(q => q.Id)
                .ToListAsync();
        }

        public async Task<List<QuestionWithOptions>> GetQuestionsWithOptionsAsync(List<int> questionIds)
        {
            return await context.Questions
                .AsNoTracking()
                .Where(q => questionIds.Contains(q.Id))
                .Select(q => new QuestionWithOptions(
                    q.Id,
                    q.Content,
                    q.QuestionType.ToString(),
                    q.Options.Select(o => new OptionData(o.Id, o.Content, o.IsCorrect)).ToList()
                ))
                .ToListAsync();
        }

    }
}
