using System.Threading.Tasks;
using BlazorTemplate.Shared.WebApis.Sessions;
using BlazorTemplate.Server.Services;
using BlazorTemplate.Server.SharedServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace BlazorTemplate.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Consumes("application/json")]
    [Produces("application/json")]
    //[AutoValidateAntiforgeryToken]    
    public class SessionController : ControllerBase
    {
        private AccountService AccountService { get; set; }
        private SessionService SessionService { get; set; }
        private SpamBlockSharedService SpamBlockService { get; set; }

        public SessionController(   [FromServices] AccountService accountService,
                                    [FromServices] SessionService sessionService,
                                    [FromServices] SpamBlockSharedService spamBlockSharedService)
        {
            AccountService = accountService;
            SessionService = sessionService;
            SpamBlockService = spamBlockSharedService;
        }
        
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Login( LoginRequest request)
        {
            
            var(result, token) = await SessionService.LoginAsync(request.UserName, request.Password);

            if (!result.Succeeded) return BadRequest();

            return Ok(new LoginResponse() { Token = token });
        }

        [HttpDelete]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await SessionService.LogoutAsync();
            return Ok();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ValidateToken()
        {
            var result = await SessionService.ValidateTokenAsync(HttpContext.Request.Headers["Authorization"]);
            if (!result) return BadRequest();
            return Ok();
        }
    }
}
