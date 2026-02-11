using Elm.Application.Contracts;
using Elm.Application.Contracts.Features.Authentication.Commands;
using Elm.Application.Contracts.Features.Authentication.DTOs;
using Elm.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Elm.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ApiBaseController
    {
        private const string refreshTokenCookieName = "refreshToken";
        private readonly SignInManager<AppUser> signInManager;
        private readonly IMediator _mediator;
        public AuthController(ILogger<AuthController> logger, IMediator mediator,
                SignInManager<AppUser> signInManager)
        {

            _mediator = mediator;
            this.signInManager = signInManager;
        }


        // POST: api/Auth/Login
        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        [EnableRateLimiting("LoginPolicy")]
        [ProducesResponseType(typeof(Result<AuthModelDto>), 200)]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            var result = HandleResult(await _mediator.Send(command));
            if (result is OkObjectResult okResult && okResult.Value is not null)
            {
                var authModel = (okResult.Value as dynamic).Data;
                if (authModel != null && !string.IsNullOrEmpty(authModel?.RefreshToken))
                {
                    SetRefreshTokenInCookie(authModel?.RefreshToken, authModel?.RefreshTokenExpiration);
                }
            }
            return result;
        }

        // POST: api/Auth/ChangePassword
        [Authorize]
        [HttpPost]
        [Route("ChangePassword")]
        [ProducesResponseType(typeof(Result<bool>), 200)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command)
            => HandleResult(await _mediator.Send(command));

        //POST: api/Auth/ResetPassword
        //[HttpPost]
        //[Route("ResetPassword")]
        //[ProducesResponseType(typeof(Result<bool>), 200)]
        //public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
        //    => HandleResult(await _mediator.Send(command));

        // POST: api/Auth/RefreshToken
        //[DisableRateLimiting]
        [HttpPost]
        [Route("RefreshToken")]
        [ProducesResponseType(typeof(Result<AuthModelDto>), 200)]
        public async Task<IActionResult> RefreshToken() //[FromBody] RefreshTokenCommand command)
        {
            var origin = Request.Headers["Origin"].ToString();

            if (string.IsNullOrEmpty(origin) || !IsTrustedDomain(origin))
            {
                return Forbid("خطأ في الطلب");
            }
            var token = Request.Cookies[refreshTokenCookieName];
            if (string.IsNullOrEmpty(token))
            {
                return NotFound("لا يوجد رمز تحديث");
            }
            //var authModel = await _mediator.Send(command);

            var authModel = await _mediator.Send(new RefreshTokenCommand(token));

            if (authModel.Data == null || string.IsNullOrEmpty(authModel.Data.RefreshToken))
            {
                return HandleResult(authModel);
            }
            SetRefreshTokenInCookie(authModel.Data.RefreshToken, authModel.Data.RefreshTokenExpiration);
            return HandleResult(authModel);
        }

        // POST: api/Auth/RevokeToken
        //[DisableRateLimiting]
        [HttpPost]
        [Route("RevokeToken")]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenCommand request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var token = request.Token ?? Request.Cookies[refreshTokenCookieName];
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("لا يوجد رمز مميز");
            }
            var result = await _mediator.Send(new RevokeTokenCommand(token));
            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }
            Response.Cookies.Delete(refreshTokenCookieName);
            return NoContent();
        }

        //[DisableRateLimiting]
        [HttpPost]
        [Route("Logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            var token = Request.Cookies[refreshTokenCookieName];

            // 3. التحقق من وجوده وإلغائه
            if (!string.IsNullOrEmpty(token))
            {
                // استدعاء خدمة إلغاء التوكن التي تلغيه في قاعدة البيانات
                await _mediator.Send(new RevokeTokenCommand(token));
                Response.Cookies.Delete(refreshTokenCookieName);
            }

            // 5. إرجاع استجابة نجاح (حتى لو لم يكن هناك توكن للحذف)
            return NoContent();
        }


        #region Private Methods

        private void SetRefreshTokenInCookie(string refreshToken, DateTime expires)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = expires.ToUniversalTime(),
                Secure = true,
                IsEssential = true,
                SameSite = SameSiteMode.None // عشان الكوكيز ترسل مع الطلبات من نفس الموقع
            };

            Response.Cookies.Append(refreshTokenCookieName, refreshToken, cookieOptions);
        }

        private static bool IsTrustedDomain(string origin)
        {
            var allowedDomains = new List<string> {
                "https://frontend.yourdomain.com",//  ابق غيره بعدين 
                "http://localhost:4200",
            };
            return allowedDomains.Any(d => origin.StartsWith(d, StringComparison.OrdinalIgnoreCase));
        }

        #endregion

    }
}
