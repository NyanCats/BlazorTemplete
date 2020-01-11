using BlazorTemplate.Server.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace BlazorTemplate.Server.Services
{
    public class SessionService
    {
        UserManager<User> UserManager { get; set; }
        SignInManager<User> SignInManager { get; set; }

        IConfiguration Configuration { get; set; }

        public SessionService(  [FromServices] UserManager<User> userManager,
                                [FromServices] SignInManager<User> signInManager,
                                [FromServices] IConfiguration configuration)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            Configuration = configuration;
        }

        public async Task<(SignInResult, string)> LoginAsync(string name, string password)
        {
            var result = await SignInManager.PasswordSignInAsync(name, password, true, true);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, name)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtSecurityKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.Now.AddDays(Convert.ToInt32(Configuration["JwtExpiryInDays"]));

            var token = new JwtSecurityToken(
                Configuration["JwtIssuer"],
                Configuration["JwtAudience"],
                claims,
                expires: expiry,
                signingCredentials: creds
            );
            
            return (result, new JwtSecurityTokenHandler().WriteToken(token));
        }

        public async Task LogoutAsync()
        {
            await SignInManager.SignOutAsync();
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            //var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            


            return true;
        }
    }
}
