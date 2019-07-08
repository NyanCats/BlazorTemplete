using BlazorTemplate.Server.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace BlazorTemplate.Server.Services
{
    public class SessionService
    {
        UserManager<ApplicationUser> UserManager { get; set; }
        SignInManager<ApplicationUser> SignInManager { get; set; }

        public SessionService(  [FromServices] UserManager<ApplicationUser> userManager,
                                [FromServices] SignInManager<ApplicationUser> signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public async Task<SignInResult> Login(string name, string password)
        {
            var result = await SignInManager.PasswordSignInAsync(name, password, true, true);
            return result;
        }

        public async Task Logout()
        {
            await SignInManager.SignOutAsync();
        }
    }
}
