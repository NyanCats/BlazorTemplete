using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorTemplate.Commons.WebApis.Accounts;
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
            // Cookie認証によるセッションチェック
            var loginResult = HttpContext.AuthenticateAsync().Result.Succeeded;
            if(loginResult) return Ok(new CreateUserResult(null, new List<string>() { "ログイン中は登録できません。" }));

            // モデルの検証
            if (!ModelState.IsValid)
            {
                // 仕様に合わせたエラーメッセージの作成
                List<string> errors = new List<string>();
                foreach(var values in ModelState.Values)
                {
                    errors.AddRange(values.Errors.Select( e => e.ErrorMessage));
                }
                return Ok(new CreateUserResult(null, errors));
            }

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

            (IdentityResult result, string password) = await AccountService.Create(request.UserName);

            if (!result.Succeeded) return BadRequest(new CreateUserResult(null, result.Errors.Select(s => s.Description).ToList()));

            SpamBlockService.AddOrUpdate(userIp);

            return Ok( new CreateUserResult(password, null) );
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

        [HttpPut]
        [Authorize]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Update()
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        [Authorize]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete()
        {
            throw new NotImplementedException();
        }
    }
}
