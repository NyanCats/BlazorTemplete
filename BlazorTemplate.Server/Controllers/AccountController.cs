using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BlazorTemplate.Server.Entities;
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
    //[Produces("application/json")]
    //[Consumes("application/json")]
    [Route("[controller]")]
    [ApiController]
    //[AutoValidateAntiforgeryToken]    
    public class AccountController : ControllerBase
    {
        private AccountService AccountService { get; set; }
        private SpamBlockSharedService SpamBlockService { get; set; }
        private SessionService SessionService { get; set; }

        public AccountController(AccountService accountService, SpamBlockSharedService spamBlockSharedService, SessionService sessionService)
        {
            AccountService = accountService;
            SpamBlockService = spamBlockSharedService;
            SessionService = sessionService;
        }

        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( [FromBody]CreateUserRequest request) 
        {
            if(SessionService.ValidateCookieAsync(HttpContext).Result) return BadRequest();
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

            (IdentityResult result, string password) = await AccountService.CreateAsync(request.UserName);

            if (!result.Succeeded) return BadRequest();

            //SpamBlockService.AddOrUpdate(userIp);

            return Ok( new CreateUserResult(password) );
        }

        [HttpGet]
        [Authorize]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Read()
        {
            var user = await AccountService.FindByNameAsync(HttpContext.User.Identity.Name);
            if (user == null) return BadRequest();

            return Ok(new UserInfomationResult(user.UserName));
        }

        // TODO
        [HttpPut]
        [Authorize]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Update()
        {
            var user = await AccountService.FindByNameAsync(HttpContext.User.Identity.Name);
            if (user == null) return BadRequest();

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
