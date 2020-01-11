using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BlazorTemplate.Server.Services;
using BlazorTemplate.Server.SharedServices;
using BlazorTemplate.Shared.WebApis.Avatars;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlazorTemplate.Server.Controllers
{
    //[Produces("application/json")]
    [Route("api/[controller]")]
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

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMyAvatar( [FromBody] CreateAvatarRequest request)
        {
            var user = await AccountService.FindByNameAsync(HttpContext.User.Identity.Name);
            if (user == null) BadRequest();

            var createResult = await AvatarService.CreateAsync(user);

            if (!createResult) BadRequest();

            return Ok();
        }

        [HttpGet]
        [Authorize]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> GetMyAvatar()
        {
            var user = await AccountService.FindByNameAsync(HttpContext.User.Identity.Name);
            if (user == null) BadRequest();

            MemoryStream getDefaultAvatar()
            {
                var stream = new FileStream(@"Resources/default.png", FileMode.Open, FileAccess.Read);
                byte[] buffer = new byte[stream.Length];

                stream.Read(buffer, 0, buffer.Length);

                return new MemoryStream(buffer);
            }

            var avatarExisting = await AvatarService.ExistsAsync(user);

            if (!avatarExisting) return NotFound(getDefaultAvatar());

            var avatarImage = await AvatarService.GetImageAsync(user);

            if(avatarImage == null) return Ok(getDefaultAvatar());
            
            return Ok(new MemoryStream(avatarImage));
        }

        [HttpPut]
        [Authorize]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateMyAvatar(IFormFile file)
        {
            var user = await AccountService.FindByNameAsync(HttpContext.User.Identity.Name);
            if (user == null) BadRequest(); 
            if (file == null) BadRequest();

            // TODO: check filesize & format
            
            byte[] buffer;
            using (var stream = file.OpenReadStream())
            {
                buffer = new byte[stream.Length];
                stream.Read(buffer, 0, (int)stream.Length);
            }

            var success = await AvatarService.UpdateAsync(user, buffer);

            if (!success) BadRequest();
            return Ok();
        }
    }
}
