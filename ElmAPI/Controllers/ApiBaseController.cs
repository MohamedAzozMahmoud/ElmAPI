using Elm.Application.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Elm.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiBaseController : ControllerBase
    {
        private ILogger<ApiBaseController>? _logger;

        // نستخدم ميزة الـ RequestServices لجلب الـ Logger بدون إجبار الـ Controllers الوارثة على تمريره في الـ Constructor
        protected ILogger<ApiBaseController> Logger =>
            _logger ??= HttpContext.RequestServices.GetRequiredService<ILogger<ApiBaseController>>();

        protected IActionResult HandleResult<T>(Result<T> result)
        {
            if (result == null) return NotFound();

            if (result.IsSuccess)
            {
                return Ok(result);
            }

            // --- جزء الـ Logging في حالة الفشل ---
            LogFailure(result);

            return result.StatusCode switch
            {
                400 => BadRequest(result),
                404 => NotFound(result),
                401 => Unauthorized(result),
                _ => StatusCode(500, result)
            };
        }

        private void LogFailure<T>(Result<T> result)
        {
            var requestPath = HttpContext.Request.Path;
            var method = HttpContext.Request.Method;

            if (result.StatusCode >= 500)
            {
                // خطأ فني جسيم 
                Logger.LogError("الخطأ الفني الجسيم: {Method} {Path}  كود {Code}. رسالة: {Message}",
                    method, requestPath, result.StatusCode, result.Message);
            }
            else
            {
                // خطأ منطقي (مثل بيانات خاطئة من المستخدم) 
                Logger.LogWarning("خطأ منطقي: {Method} {Path}  كود {Code}. رسالة: {Message}",
                    method, requestPath, result.StatusCode, result.Message);
            }
        }
    }
}
