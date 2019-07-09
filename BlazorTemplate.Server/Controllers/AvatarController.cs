using System;
using System.Collections.Generic;
using System.IO;
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

        private AvatarService AvatarService { get; set; }

        public AvatarController(AccountService accountService, AvatarService avatarService)
        {
            AccountService = accountService;
            AvatarService = avatarService;
        }

        [HttpGet]
        [Authorize]
        //[ValidateAntiForgeryToken]
        public async Task<FileStreamResult> GetMyAvatar()
        {
            var name = HttpContext.User.Identity.Name;
            var user = await AccountService.GetUser(name);
            var avatarExisting = await AvatarService.ExistsAsync(user);

            FileStreamResult getDefaultAvatar()
            {
                var stream = new FileStream(@"Resources/default.png", FileMode.Open);
                return File(stream, "image/png");
            }

            if (!avatarExisting) return getDefaultAvatar();

            var avatarImage = await AvatarService.GetImageAsync(user);

            if(avatarImage == null) return getDefaultAvatar();

            return File(new MemoryStream(avatarImage), "image/png");
        }
    }
}
