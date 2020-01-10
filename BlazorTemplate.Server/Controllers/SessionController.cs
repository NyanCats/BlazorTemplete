using System.Threading.Tasks;
using BlazorTemplate.Shared.WebApis.Sessions;
using BlazorTemplate.Server.Services;
using BlazorTemplate.Server.SharedServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorTemplate.Server.Controllers
{
    [Produces("application/json")]
    [Route("[controller]")]
    [ApiController]
    //[AutoValidateAntiforgeryToken]    
    public class SessionController : ControllerBase
    {
        private AccountService AccountService { get; set; }
        private SessionService SessionService { get; set; }
        private SpamBlockSharedService SpamBlockService { get; set; }

        public SessionController(   [FromServices] AccountService accountService,
                                    [FromServices] SessionService sessionService,
                                    [FromServices] SpamBlockSharedService spamBlockSharedService)
        {
            AccountService = accountService;
            SessionService = sessionService;
            SpamBlockService = spamBlockSharedService;
        }
        
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Login( [FromBody] LoginRequest request)
        {
            var result = await SessionService.LoginAsync(request.UserName, request.Password);

            if (!result.Succeeded) return BadRequest();

            return Ok();
        }

        [HttpDelete]
        [Authorize]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await SessionService.LogoutAsync();
            return Ok();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ValidateCookie()
        {
            var result = await SessionService.ValidateCookieAsync(HttpContext);
            if (!result) return Unauthorized();

            return Ok();
        }
    }
}
