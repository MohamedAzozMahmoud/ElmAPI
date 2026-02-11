using Elm.API.Handler;
using Elm.Application;
using Elm.Application.Contracts.Abstractions.Excel;
using Elm.Application.Contracts.Abstractions.Files;
using Elm.Application.Contracts.Abstractions.Realtime;
using Elm.Application.Contracts.Abstractions.Settings;
using Elm.Application.Contracts.Abstractions.TestService;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Helper;
using Elm.Application.Mapper.Elm.Application.Mappers;
using Elm.Domain.Entities;
using Elm.Infrastructure;
using Elm.Infrastructure.BackgroundServices;
using Elm.Infrastructure.Notifications;
using Elm.Infrastructure.Repositories;
using Elm.Infrastructure.Services.Excel;
using Elm.Infrastructure.Services.Files;
using Elm.Infrastructure.Services.Realtime;
using Elm.Infrastructure.Services.Settings;
using Elm.Infrastructure.Services.TestService;
using Exceptionless;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Serilog;
using System.Globalization;
using System.Security.Claims;
using System.Text;
using System.Threading.RateLimiting;

namespace ElmAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            // Add services to the container.
            //************************************************
            //====== ( Add Identity & DbContext ) ========
            builder.Services.AddIdentity<AppUser, Role>().AddEntityFrameworkStores<AppDbContext>();


            builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(
                builder.Configuration.GetConnectionString("DefaultConnection"),
                sqlOptions => sqlOptions.EnableRetryOnFailure() // إضافة خاصية إعادة المحاولة
            ));

            //************************************************
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .CreateLogger();

            // ===== ( Serilog Configuration ) ======
            builder.Host.UseSerilog();
            // ===== ( Exceptionless Configuration ) ======
            builder.Services.AddExceptionless(builder.Configuration);

            // ===== ( ) ======
            builder.Services.Configure<SettingsOptions>(builder.Configuration.GetSection("SettingsOptions"));
            builder.Services.Configure<JWT>(builder.Configuration.GetSection("JwtSettings"));
            builder.Services.AddApplication();

            #region Repositories 

            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IUserPerissionRepsitory, UserPerissionRepsitory>();
            builder.Services.AddScoped<IRolePermissionRepository, RolePermissionRepsitory>();
            builder.Services.AddScoped<IUniversityRepository, UniversityRepository>();
            builder.Services.AddScoped<ICollegeRepository, CollegeRepository>();
            builder.Services.AddScoped<IYearRepository, YearRepository>();
            builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            builder.Services.AddScoped<ISubjectRepository, SubjectRepository>();
            builder.Services.AddScoped<ICurriculumRepository, CurriculumRepository>();
            builder.Services.AddScoped<IQuestionBankRepository, QuestionBankRepository>();
            builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
            builder.Services.AddScoped<IFileStorageService, FileStorageService>();
            builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
            builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();

            builder.Services.AddScoped<IStudentRepository, StudentRepository>();

            #endregion

            #region Infrastructure Services

            builder.Services.AddHostedService<NotificationCleanupService>();
            builder.Services.AddHostedService<RefreshTokenCleanupService>();
            builder.Services.AddScoped<IExcelWriter, ExcelWriter>();
            builder.Services.AddScoped<IExcelReader, ExcelReader>();
            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddScoped<ITestSessionService, TestSessionService>();
            builder.Services.AddScoped<ITestScoringService, TestScoringService>();
            builder.Services.AddScoped<ISettingsService, SettingsService>();

            #endregion

            #region Mapper

            builder.Services.AddSingleton<MappingProvider>();
            //builder.Services.AddAutoMapper(
            //    //typeof(Program).Assembly, // API Assembly
            //    Assembly.Load("Elm.Application") // Core Assembly
            //);
            #endregion

            #region Global Handler Exception 

            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddProblemDetails();

            #endregion

            #region Policy
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("policy", policy =>
                {
                    policy.WithOrigins(
                        "http://localhost:4200"
                    )
                       .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials();
                });
            });

            #endregion

            #region JwtBearer & Authentication

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false; // for development
                    o.SaveToken = true;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = false,   // if false will be public
                        ValidateAudience = false, // if false will be public
                        ValidateLifetime = true,
                        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                        ValidAudience = builder.Configuration["JwtSettings:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]!)),
                        ClockSkew = TimeSpan.Zero
                    };
                    o.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            // 1. محاولة قراءة التوكن من الـ Query String
                            var accessToken = context.Request.Query["access_token"];

                            // 2. إذا كان التوكن موجوداً والمسار هو للـ Hub
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) &&
                                path.StartsWithSegments("/notificationHub")) // نفس الاسم في MapHub
                            {
                                // 3. تعيين التوكن للسياق ليتم التحقق منه
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });
            #endregion

            builder.Services.AddHttpContextAccessor(); // هذا السطر ضروري جداً
            builder.Services.AddAuthorization();


            builder.Services.AddSignalR();

            builder.Services.AddSingleton<IUserIdProvider, UserIdProvider>();
            builder.Services.AddMemoryCache();
            // 1. تسجيل خدمات الـ Health Check
            builder.Services.AddHealthChecks()
                .AddDbContextCheck<AppDbContext>("Database");

            builder.Services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            #region  Rate Limiter Configuration 

            builder.Services.AddRateLimiter(options =>
            {
                // ═══════════════════════════════════════════════════════
                //  1. Global Rate Limit - حماية من DDoS
                // ════════════════════════════════════��══════════════════
                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
                {
                    return RateLimitPartition.GetFixedWindowLimiter("GlobalLimiter", _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 500,
                        Window = TimeSpan.FromMinutes(1),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 0
                    });
                });

                // ═══════════════════════════════════════════════════════
                //  2. OnRejected - رسالة الرفض مع Headers
                // ═══════════════════════════════════════════════════════
                options.OnRejected = async (context, token) =>
                {
                    var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                    var clientIp = context.HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault()
                        ?? context.HttpContext.Connection.RemoteIpAddress?.ToString();

                    logger.LogWarning(
                        "Rate limit exceeded. IP: {ClientIp}, User: {User}, Path: {Path}",
                        clientIp,
                        context.HttpContext.User.Identity?.Name ?? "anonymous",
                        context.HttpContext.Request.Path);

                    context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;

                    TimeSpan retryAfter = TimeSpan.FromMinutes(1); // Default
                    if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfterValue))
                    {
                        retryAfter = retryAfterValue;
                    }

                    context.HttpContext.Response.Headers.RetryAfter =
                        ((int)retryAfter.TotalSeconds).ToString(NumberFormatInfo.InvariantInfo);

                    await context.HttpContext.Response.WriteAsJsonAsync(new
                    {
                        Type = "https://tools.ietf.org/html/rfc6585#section-4",
                        Title = "Too Many Requests",
                        Status = 429,
                        Message = "لقد تخطيت الحد المسموح من الطلبات. يرجى الانتظار قليلاً ثم المحاولة مرة أخرى.",
                        RetryAfterSeconds = (int)retryAfter.TotalSeconds
                    }, token);
                };

                // ═══════════════════════════════════════════════════════
                //  3. User Role Policy - حسب صلاحيات المستخدم
                // ═══════════════════════════════════════════════════════
                options.AddPolicy("UserRolePolicy", context =>
                {
                    var userRole = context.User.FindFirstValue(ClaimTypes.Role);
                    var userName = context.User.Identity?.Name
                        ?? context.Request.Headers["X-Forwarded-For"].FirstOrDefault()?.Split(',').FirstOrDefault()?.Trim()
                        ?? context.Connection.RemoteIpAddress?.ToString()
                        ?? "anonymous";

                    var (permitLimit, window) = userRole switch
                    {
                        "Doctor" => (200, TimeSpan.FromMinutes(1)),
                        "Leader" => (150, TimeSpan.FromMinutes(1)),
                        _ => (100, TimeSpan.FromMinutes(1))
                    };

                    return RateLimitPartition.GetSlidingWindowLimiter(userName, _ => new SlidingWindowRateLimiterOptions
                    {
                        PermitLimit = permitLimit,
                        Window = window,
                        SegmentsPerWindow = 6,  // كل segment = 10 ثواني (أدق من Fixed Window)
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 0
                    });
                });

                // ═══════════════════════════════════════════════════════
                //  4. Login Policy - حماية من Brute Force (Per-IP)
                // ═══════════════════════════════════════════════════════
                options.AddPolicy("LoginPolicy", context =>
                {
                    var clientIp = context.Request.Headers["X-Forwarded-For"].FirstOrDefault()?.Split(',').FirstOrDefault()?.Trim()
                        ?? context.Connection.RemoteIpAddress?.ToString()
                        ?? "anonymous";

                    return RateLimitPartition.GetSlidingWindowLimiter(clientIp, _ => new SlidingWindowRateLimiterOptions
                    {
                        PermitLimit = 5,
                        Window = TimeSpan.FromMinutes(5),
                        SegmentsPerWindow = 5,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 0
                    });
                });

            });

            #endregion



            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddProblemDetails();

            builder.Services.AddHttpContextAccessor();
            builder.Configuration
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();  // ← هذا مهم جداً!

            if (builder.Environment.IsProduction())
            {
                // في الإنتاج: استخدم مجلد ثابت
                var keysDirectory = Path.Combine(builder.Environment.ContentRootPath, "App_Data", "DataProtection-Keys");

                if (!Directory.Exists(keysDirectory))
                {
                    Directory.CreateDirectory(keysDirectory);
                }

                builder.Services.AddDataProtection()
                    .SetApplicationName("ElmAPI")
                    .PersistKeysToFileSystem(new DirectoryInfo(keysDirectory))
                    .SetDefaultKeyLifetime(TimeSpan.FromDays(90)); // مدة صلاحية المفتاح
            }
            else
            {
                // في التطوير: الإعدادات الافتراضية كافية
                builder.Services.AddDataProtection()
                    .SetApplicationName("ElmAPI");
            }


            // all next code is written by gitHub copilot 
            // في الإنتاج، تأكد من أن Data Protection يستخدم تخزيناً ثابتاً للمفاتيح (مثل مجلد أو Azure Blob Storage)
            // في التطوير، يمكنك استخدام الإعدادات الافتراضية لـ Data Protection
            //builder.Services.AddDataProtection();
            //builder.Services.AddHttpContextAccessor();
            //builder.Services.AddProblemDetails();

            builder.Services.AddOpenApiDocument(cfg =>
            {
                cfg.Title = "ElmAPI";
                cfg.AddSecurity("JWT", new NSwag.OpenApiSecurityScheme
                {
                    Type = NSwag.OpenApiSecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    In = NSwag.OpenApiSecurityApiKeyLocation.Header,
                    Description = "Bearer {token}"
                });

                // يضيف متطلبات الـ security للـ actions التي تحتوي على [Authorize] ويتجاهل [AllowAnonymous]
                cfg.OperationProcessors.Add(new NSwag.Generation.Processors.Security.AspNetCoreOperationSecurityScopeProcessor("JWT"));
            });


            // Add Scalar API Reference
            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            app.MapOpenApi();
            app.MapScalarApiReference();
            //}
            //*********************************

            app.UseExceptionHandler();
            app.UseExceptionless();

            // 1. التوجيه وتأمين الاتصال
            app.UseHttpsRedirection();
            // تفعيل Static Files
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images")),
                RequestPath = "/Images",
                OnPrepareResponse = ctx =>
                {
                    // Cache للصور لمدة 7 أيام
                    ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=604800");

                    // Security Headers
                    ctx.Context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
                }
            });
            // 5. التسجيل (Logging)
            app.UseSerilogRequestLogging();

            app.UseRouting();
            app.UseForwardedHeaders(); // إضافة هذا السطر لمعالجة رؤوس التوجيه الأمامي
            // 2. السماح بالاتصال من الـ Frontend (CORS)
            app.UseCors("policy");

            // 3. الهوية والأمان (يجب أن يكونا بهذا الترتيب)
            app.UseAuthentication();
            app.UseAuthorization();


            // 4. الـ Rate Limiter (يجب أن يكون بعد الـ Authorization لكي يعرف Role المستخدم)
            app.UseRateLimiter();

            // 6. تعريف المسارات (Endpoints)
            app.MapControllers();
            app.MapHub<NotificationHub>("/notificationHub"); // تأكد من الاسم الموحد للمسار

            app.MapHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = async (context, report) =>
                {
                    context.Response.ContentType = "application/json";
                    var response = new
                    {
                        Status = report.Status.ToString(),
                        Checks = report.Entries.Select(e => new
                        {
                            Component = e.Key,
                            Status = e.Value.Status.ToString(),
                            Description = e.Value.Description,
                            Duration = e.Value.Duration
                        }),
                        TotalDuration = report.TotalDuration
                    };
                    await context.Response.WriteAsJsonAsync(response);
                }
            });

            app.Run();
        }
    }
}
