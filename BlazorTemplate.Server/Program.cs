using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace BlazorTemplate.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost
                .CreateDefaultBuilder(args)
                .UseKestrel()
                .UseConfiguration
                (
                    new ConfigurationBuilder()
                        .AddCommandLine(args)
                        .Build()
                )
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseUrls("http://*:80/")
                .UseStartup<Startup>()
                .Build();
    }
}
