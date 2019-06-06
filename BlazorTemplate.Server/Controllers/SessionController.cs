using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorTemplate.Commons.WebApis.Sessions;
using BlazorTemplate.Server.Services;
using BlazorTemplate.Server.SharedServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
            // Cookie認証によるセッションチェック
            var cookieLoginResult = HttpContext.AuthenticateAsync().Result.Succeeded;
            if (cookieLoginResult) return Ok(new LoginResult(new List<string>() { "既にログインしています。" }));

            // モデルの検証
            if (!ModelState.IsValid)
            {
                // 仕様に合わせたエラーメッセージの作成
                List<string> errors = new List<string>();
                foreach (var values in ModelState.Values)
                {
                    errors.AddRange(values.Errors.Select(e => e.ErrorMessage));
                }
                return Ok(new LoginResult(errors));
            }

            var result = await AccountService.Login(request.UserName, request.Password);

            if (!result.Succeeded)
            {
                if (result.IsLockedOut) return Ok(new LoginResult(new List<string>() { "ロックアウトされています。" }));
                if (result.IsNotAllowed) return Ok(new LoginResult(new List<string>() { "ログインは許可されていません。" }));

                return Ok(new LoginResult(new List<string>() { "ログインに失敗しました。" }));
            }

            return Ok(new LoginResult(null));
        }

        [HttpDelete]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            var result = await HttpContext.AuthenticateAsync();
            if (!result.Succeeded) return NoContent();

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
