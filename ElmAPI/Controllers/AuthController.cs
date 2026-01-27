using Elm.Application.Contracts.Features.Authentication.Commands;
using Elm.Application.Contracts.Features.Authentication.DTOs;
using Elm.Application.Contracts.Features.Authentication.Queries;
using Elm.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Elm.API.Controllers
{
    //[Authorize]
    //[EnableRateLimiting("LoginPolicy")]
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
        /// <summary>
        /// Registers a new admin user using the provided registration command.
        /// </summary>
        /// <param name="command">The registration details for the new admin user.</param>
        /// <returns>An IActionResult indicating the result of the registration operation.</returns>
        // POST: api/Auth/RegisterAdmin
        //[Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("RegisterAdmin")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterCommand command) =>
            HandleResult(await _mediator.Send(command));

        // POST: api/Auth/RegisterStudent
        //[Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("RegisterStudent")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> RegisterStudent([FromBody] RegisterStudentCommand command) =>
            HandleResult(await _mediator.Send(command));

        // POST: api/Auth/RegisterDoctor
        //[Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("RegisterDoctor")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> RegisterDoctor([FromBody] RegisterDoctorCommand command) =>
            HandleResult(await _mediator.Send(command));

        // POST: api/Auth/Login
        //[AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        [EnableRateLimiting("LoginPolicy")]
        [ProducesResponseType(typeof(AuthModelDto), 200)]
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
        [HttpPost]
        [Route("ChangePassword")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command)
            => HandleResult(await _mediator.Send(command));

        //POST: api/Auth/ResetPassword
        //[HttpPost]
        //[Route("ResetPassword")]
        //[ProducesResponseType(typeof(bool), 200)]
        //public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
        //    => HandleResult(await _mediator.Send(command));

        // POST: api/Auth/RefreshToken
        //[DisableRateLimiting]
        [HttpPost]
        [Route("RefreshToken")]
        [ProducesResponseType(typeof(AuthModelDto), 200)]
        public async Task<IActionResult> RefreshToken() //[FromBody] RefreshTokenCommand command)
        {
            var origin = Request.Headers["Origin"].ToString();

            if (string.IsNullOrEmpty(origin) || IsTrustedDomain(origin))
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
        //[Authorize]
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

        // DELETE: api/Auth/Delete
        //[Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("Delete")]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> Delete([FromBody] DeleteCommand command)
            => HandleResult(await _mediator.Send(command));

        [HttpGet]
        [Route("GetAllUsers/{role:alpha}")]
        public async Task<IActionResult> GetAllUsers([FromRoute] string role)
        => HandleResult(await _mediator.Send(new GetAllUsersQuery(role)));


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
