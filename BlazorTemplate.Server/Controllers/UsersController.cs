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
    [Consumes("application/json")]
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

        [HttpGet]
        public IEnumerable<int> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5)
            .ToArray();
        }
    }
}
