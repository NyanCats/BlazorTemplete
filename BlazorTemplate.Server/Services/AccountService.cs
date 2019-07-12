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

        public async Task<(IdentityResult result, string password)> CreateAsync(string name)
        {
            var user = new User()
            {
                UserName = name
                
            };
            
            var password = GeneratePassword();
            var result = await UserManager.CreateAsync(user, password);
            
            if (!result.Succeeded) return (result, null);

            return (result, password);
        }
        
        public async Task<SignInResult> ValidateAsync(string userName, string password)
        {
            var user = await UserManager.FindByNameAsync(userName);

            var result = await SignInManager.CheckPasswordSignInAsync(user, password, true);
            return result;
        }

        public async Task<bool> ExistsByUserAsync(User user)
        {
            var result = await UserManager.FindByIdAsync(user.Id.ToString());
            if (result == null) return false;

            return true;
        }

        public async Task<bool> ExistsByIdAsync(Guid id)
        {
            var user = await UserManager.FindByIdAsync(id.ToString());
            if (user == null) return false;

            return true;
        }

        public async Task<bool> ExistsByNameAsync(string userName)
        {
            var user = await UserManager.FindByNameAsync(userName);
            if (user == null) return false;

            return true;
        }

        public async Task<User> FindByIdAsync(Guid id)
        {
            return await UserManager.FindByIdAsync(id.ToString());
        }

        public async Task<User> FindByNameAsync(string userName)
        {
            return await UserManager.FindByNameAsync(userName);
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