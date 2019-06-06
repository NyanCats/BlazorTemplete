using BlazorTemplate.Server.Entities.Identities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace BlazorTemplate.Server.Services
{
    public class AccountService
    {
        UserManager<ApplicationUser> UserManager { get; set; }
        SignInManager<ApplicationUser> SignInManager { get; set; }

        public AccountService( [FromServices]UserManager<ApplicationUser> userManager,
                               [FromServices]SignInManager<ApplicationUser> signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public async Task<(IdentityResult result, string password)> Create(string name)
        {
            var user = new ApplicationUser(name);

            var password = GeneratePassword();
            var result = await UserManager.CreateAsync(user, password);
            
            if (!result.Succeeded) return (result, null);

            return (result, password);
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

        public async Task<SignInResult> Validate(string userName, string password)
        {
            var user = await UserManager.FindByNameAsync(userName);

            var result = await SignInManager.CheckPasswordSignInAsync(user, password, true);
            return result;
        }

        public async Task<bool> IdExists(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            if (user == null) return false;

            return true;
        }

        public async Task<bool> UserExists(string userName)
        {
            var user = await UserManager.FindByNameAsync(userName);
            if (user == null) return false;

            return true;
        }

        public async Task<ApplicationUser> GetUser(string userName)
        {
            var user = await UserManager.FindByNameAsync(userName);

            return user;
        }

        protected string GeneratePassword()
        {
            var bs = new byte[24];

            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(bs);
            }
            return Convert.ToBase64String(bs);
        }
    }
}