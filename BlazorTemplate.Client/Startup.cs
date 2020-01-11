using Blazor.FileReader;
using Blazored.LocalStorage;
using BlazorTemplate.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
using Sotsera.Blazor.Toaster.Core.Models;

namespace BlazorTemplate.Client
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddFileReaderService(options =>
            {
                options.UseWasmSharedBuffer = true;
            });
            services.AddAuthorizationCore();
            services.AddToaster(config =>
            {
                config.PositionClass = Defaults.Classes.Position.BottomRight;
                config.PreventDuplicates = true;
                config.NewestOnTop = true;
            });

            services.AddBlazoredLocalStorage();
            services.AddSingleton<AccountService>();
            services.AddSingleton<SessionService>();
            services.AddSingleton<AvatarService>();
            services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
        }
        
        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
