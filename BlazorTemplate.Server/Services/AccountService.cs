using BlazorTemplate.Server.Entities;
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
        UserManager<User> UserManager { get; set; }
        SignInManager<User> SignInManager { get; set; }

        public AccountService( [FromServices] UserManager<User> userManager,
                               [FromServices] SignInManager<User> signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public async Task<(IdentityResult result, string password)> Create(string name)
        {
            var user = new User() { UserName = name };

            var password = GeneratePassword();
            var result = await UserManager.CreateAsync(user, password);
            
            if (!result.Succeeded) return (result, null);

            return (result, password);
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

        public async Task<User> GetUser(string userName)
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