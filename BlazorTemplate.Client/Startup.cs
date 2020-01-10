using Blazor.FileReader;
using BlazorTemplate.Client.Services;
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

            services.AddSingleton<AccountService>();
            services.AddSingleton<SessionService>();
            services.AddSingleton<AvatarService>();
        }
        
        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
