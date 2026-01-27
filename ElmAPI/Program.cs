using Elm.API.Handler;
using Elm.Application;
using Elm.Application.AutoMapper;
using Elm.Application.Contracts.Abstractions.Excel;
using Elm.Application.Contracts.Abstractions.Files;
using Elm.Application.Contracts.Abstractions.Realtime;
using Elm.Application.Contracts.Abstractions.TestService;
using Elm.Application.Contracts.Repositories;
using Elm.Application.Helper;
using Elm.Domain.Entities;
using Elm.Infrastructure;
using Elm.Infrastructure.BackgroundServices;
using Elm.Infrastructure.Notifications;
using Elm.Infrastructure.Repositories;
using Elm.Infrastructure.Services.Excel;
using Elm.Infrastructure.Services.Files;
using Elm.Infrastructure.Services.Realtime;
using Elm.Infrastructure.Services.TestService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Serilog;
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
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            //************************************************

            // ===== ( Serilog Configuration ) ======
            builder.Host.UseSerilog((context, services, configuration) =>
            {
                configuration.ReadFrom.Configuration(context.Configuration);
            });

            // ===== ( User Handelers Folder ) ======
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

            #endregion

            #region Infrastructure Services

            builder.Services.AddHostedService<NotificationCleanupService>();
            builder.Services.AddHostedService<RefreshTokenCleanupService>();
            builder.Services.AddScoped<IExcelWriter, ExcelWriter>();
            builder.Services.AddScoped<IExcelReader, ExcelReader>();
            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddScoped<ITestSessionService, TestSessionService>();
            builder.Services.AddScoped<ITestScoringService, TestScoringService>();

            #endregion

            #region AutoMapper
            builder.Services.AddAutoMapper(typeof(Mapping).Assembly);

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
                    .AllowAnyMethod()
                    .AllowAnyHeader()
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
                        ValidateIssuer = true,
                        ValidateAudience = false, // if false will be public
                        ValidateLifetime = true,
                        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                        ValidAudience = builder.Configuration["JwtSettings:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]!)),
                        ClockSkew = TimeSpan.Zero
                    };
                    o.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];
                            // If the request is for our hub...
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) &&
                                (path.StartsWithSegments("/notification")))
                            {
                                // Read the token out of the query string
                                context.Token = accessToken;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });
            #endregion

            builder.Services.AddAuthorization();


            builder.Services.AddSignalR();
            builder.Services.AddSingleton<IUserIdProvider, UserIdProvider>();
            builder.Services.AddMemoryCache();
            // 1. تسجيل خدمات الـ Health Check
            builder.Services.AddHealthChecks()
                .AddDbContextCheck<AppDbContext>("Database");


            #region  Rate Limiter Configuration 

            builder.Services.AddRateLimiter(options =>
            {
                options.OnRejected = async (context, token) =>
                {
                    context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    await context.HttpContext.Response.WriteAsJsonAsync(new
                    {
                        Message = "لقد تخطيت الحد المسموح من الطلبات. يرجى الانتظار قليلاً ثم المحاولة مرة أخرى."
                    }, token);
                };

                options.AddPolicy("UserRolePolicy", context =>
                {
                    var userRole = context.User.FindFirstValue(ClaimTypes.Role);
                    var userName = context.User.Identity?.Name ?? context.Connection.RemoteIpAddress?.ToString();
                    if (userRole == "Admin")
                    {
                        return RateLimitPartition.GetFixedWindowLimiter(userName ?? "anonymous", _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 500,
                            Window = TimeSpan.FromMinutes(1),
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                        });
                    }
                    if (userRole == "Doctor")
                    {
                        return RateLimitPartition.GetFixedWindowLimiter(userName ?? "anonymous", _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 150,
                            Window = TimeSpan.FromMinutes(1),
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                        });
                    }
                    if (userRole == "Leader")
                    {
                        return RateLimitPartition.GetFixedWindowLimiter(userName ?? "anonymous", _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 100,
                            Window = TimeSpan.FromMinutes(1),
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                        });
                    }

                    return RateLimitPartition.GetFixedWindowLimiter(userName ?? "anonymous", _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 50,
                        Window = TimeSpan.FromMinutes(1),
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                    });

                });

                options.AddFixedWindowLimiter("LoginPolicy", opt =>
                {
                    opt.PermitLimit = 5;
                    opt.Window = TimeSpan.FromMinutes(5);
                    opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
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
            //======== ( Global Handler Exception  ) ==============

            app.UseExceptionHandler();
            //*********************************

            // 1. التوجيه وتأمين الاتصال
            app.UseHttpsRedirection();
            app.UseStaticFiles(); // مهم جداً لأنك ترفع ملفات (summaries/images)
            app.UseRouting();

            // 2. السماح بالاتصال من الـ Frontend (CORS)
            app.UseCors("policy");

            // 3. الهوية والأمان (يجب أن يكونا بهذا الترتيب)
            app.UseAuthentication();
            app.UseAuthorization();

            // 4. الـ Rate Limiter (يجب أن يكون بعد الـ Authorization لكي يعرف Role المستخدم)
            app.UseRateLimiter();

            // 5. التسجيل (Logging)
            app.UseSerilogRequestLogging();

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
