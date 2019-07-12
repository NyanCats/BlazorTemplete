using BlazorTemplate.Server.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
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
        UserManager<User> UserManager { get; set; }
        SignInManager<User> SignInManager { get; set; }

        public SessionService(  [FromServices] UserManager<User> userManager,
                                [FromServices] SignInManager<User> signInManager)
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

        public async Task<bool> ValidateCookie(HttpContext context)
        {
            var authResult = await context.AuthenticateAsync();
            return authResult.Succeeded;
        }
    }
}
