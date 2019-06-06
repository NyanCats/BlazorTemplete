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
    public class UsersController : ControllerBase
    {
        private AccountService AccountService { get; set; }
        private SpamBlockSharedService SpamBlockService { get; set; }

        public UsersController(AccountService accountService, SpamBlockSharedService spamBlockSharedService)
        {
            AccountService = accountService;
            SpamBlockService = spamBlockSharedService;
        }
        /*
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create() 
        {

        }
        */

        [HttpGet]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Get()
        {
            return NotFound();
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Read(string id)
        {
            throw new NotImplementedException();
        }

        /*
        [HttpPut]
        [Authorize]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Update()
        {

        }
        */
        /*
        [HttpDelete]
        [Authorize]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete()
        {

        }
        */
    }
}
