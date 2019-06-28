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
        private SpamBlockSharedService SpamBlockService { get; set; }

        public SessionController(AccountService accountService, SpamBlockSharedService spamBlockSharedService)
        {
            AccountService = accountService;
            SpamBlockService = spamBlockSharedService;
        }
        
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromBody]LoginRequest request)
        {
            // ログイン済みか
            var loginResult = HttpContext.AuthenticateAsync().Result.Succeeded;
            if (loginResult) return BadRequest();

            // モデルの検証
            if (!ModelState.IsValid) return BadRequest();

            var result = await AccountService.Login(request.UserName, request.Password);

            if (!result.Succeeded) return BadRequest();

            return Ok(new LoginResult(null));
        }

        [HttpDelete]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            var result = await HttpContext.AuthenticateAsync();
            if (!result.Succeeded) return Ok();

            await AccountService.Logout();
            return Ok();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ValidateCookie()
        {
            var result = await HttpContext.AuthenticateAsync();

            if(!result.Succeeded) return Unauthorized();

            return Ok();
        }
    }
}
