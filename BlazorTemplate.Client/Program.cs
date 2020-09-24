using System;
using System.Net.Http;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using BlazorTemplate.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Sotsera.Blazor.Toaster.Core.Models;
using Tewr.Blazor.FileReader;

namespace BlazorTemplate.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddFileReaderService(options =>
            {
                options.UseWasmSharedBuffer = true;
            });
            builder.Services.AddAuthorizationCore();
            builder.Services.AddToaster(config =>
            {
                config.PositionClass = Defaults.Classes.Position.BottomRight;
                config.PreventDuplicates = true;
                config.NewestOnTop = true;
            });
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddScoped<AccountService>();
            builder.Services.AddScoped<SessionService>();
            builder.Services.AddScoped<AvatarService>();
            builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();

            await builder.Build().RunAsync();
        }
    }
}
