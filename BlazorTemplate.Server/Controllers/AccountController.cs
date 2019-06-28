using System;
using System.Threading.Tasks;

using BlazorTemplate.Server.Services;
using BlazorTemplate.Server.SharedServices;
using BlazorTemplate.Shared.WebApis.Accounts;

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
    public class AccountController : ControllerBase
    {
        private AccountService AccountService { get; set; }
        private SpamBlockSharedService SpamBlockService { get; set; }

        public AccountController(AccountService accountService, SpamBlockSharedService spamBlockSharedService)
        {
            AccountService = accountService;
            SpamBlockService = spamBlockSharedService;
        }

        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody]CreateUserRequest request) 
        {
            // ログイン済みか
            var loginResult = HttpContext.AuthenticateAsync().Result.Succeeded;
            if(loginResult) return BadRequest();

            // モデルの検証
            if (!ModelState.IsValid) return BadRequest();

            /*
            // リモートIPの取得
            var userIp = HttpContext.Connection.RemoteIpAddress.ToString();

            // BANされていた場合   
            if (SpamBlockService.IsRejected(userIp))
            {
                return Ok(new CreateUserResult(null, new List<string>() { "新規登録はできません。BANされています。" }));
            }

            // BANされてないが、何度も登録してくる場合
            if (SpamBlockService.IsRegistered(userIp))
            {
                SpamBlockService.AddOrUpdate(userIp);
                return Ok
                (
                    new CreateUserResult(null, new List<string>() { $"{SpamBlockService.MonitorTime}秒後にもう一度お試しください。" +
                                                    $"[{SpamBlockService.GetBlockCount(userIp)}/{SpamBlockService.AllowableCount}]" })
                );
            }
            */

            (IdentityResult result, string password) = await AccountService.Create(request.UserName);

            if (!result.Succeeded) return BadRequest();

            //SpamBlockService.AddOrUpdate(userIp);

            return Ok( new CreateUserResult(password) );
        }

        [HttpGet]
        [Authorize]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> UserInfomation()
        {
            var user = await AccountService.GetUser(HttpContext.User.Identity.Name);
            if (user == null) return BadRequest();

            return Ok(new UserInfomationResult(user.Name));
        }

        // TODO
        [HttpPut]
        [Authorize]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Update()
        {
            throw new NotImplementedException();
        }

        // TODO
        [HttpDelete]
        [Authorize]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete()
        {
            throw new NotImplementedException();
        }
    }
}
