using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    public class AvatarController : ControllerBase
    {
        private AccountService AccountService { get; set; }

        public AvatarController(AccountService accountService)
        {
            AccountService = accountService;
        }

        [HttpGet]
        [Authorize]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> GetMyAvatarUrl()
        {
            throw new Exception();
        }
    }
}
