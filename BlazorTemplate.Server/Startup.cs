using BlazorTemplate.Server.Services;
using BlazorTemplate.Server.SharedServices;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using BlazorTemplate.Server.Entities;
using BlazorTemplate.Server.Properties.SampleProject.Resources;
using BlazorTemplate.Server.Properties;
using System.Net;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Localization;
using System.Collections.Generic;
using System.Globalization;
using BlazorTemplate.Server.Infrastructures.DataBases.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace BlazorTemplate.Server
{
    public class Startup
    {
        IConfiguration Configuration { get; set; }
        IServiceCollection Services { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBlazorDebugging();
                //app.UseDatabaseErrorPage();
                ListAllRegisteredServices(app);
            }
            else
            {
                app.UseExceptionHandler("/Error");
                //app.UseRewriter(new RewriteOptions().AddRedirectToHttps());
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseRouting();
            app.UseResponseCompression();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCookiePolicy();
            //app.UseRequestLocalization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                //endpoints.MapBlazorHub();
                endpoints.MapFallbackToClientSideBlazor<Client.Startup>("index.html");
            });

            //app.UseMiddleware<CsrfTokenCookieMiddleware>();
            app.UseClientSideBlazorFiles<Client.Startup>();
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            Services = services;

            Services.AddResponseCompression(options =>
            {
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[]
                {
                    MediaTypeNames.Application.Octet,
                    WasmMediaTypeNames.Application.Wasm,
                });
            });

            /*
            Services.AddAntiforgery(options =>
            {
                options.HeaderName = "X-CSRF-TOKEN";
            });
            */

            Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressConsumesConstraintForFormFileParameters = true;
                options.SuppressInferBindingSourcesForParameters = true;
                options.SuppressModelStateInvalidFilter = true;
            });

            Services.Configure<IdentityOptions>(options =>
            {
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;

                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 32;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                options.User.RequireUniqueEmail = false;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_";
            });


            Services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => context?.User?.Identity?.IsAuthenticated != true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    // 認証されていないユーザーがリソースにアクセスしようとしたとき
                    options.LoginPath = $"/";

                    options.LogoutPath = $"/";

                    // アクセスが禁止されているリソースにアクセスしようとしたとき
                    options.AccessDeniedPath = $"/";
                    // クッキーの有効期限
                    options.ExpireTimeSpan = TimeSpan.FromDays(30);
                });

            
            services.AddDbContext<ApplicationIdentityDbContext>((serviceProvider, options) => 
            {
                //options.UseMemoryCache(new MemoryCache(new MemoryCacheOptions()));
                //options.UseInternalServiceProvider(serviceProvider);
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });
            

            Services.AddIdentity<ApplicationUser, ApplicationRole>()
                    .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
                    .AddErrorDescriber<IdentityErrorDescriberJapanese>();
                    //.AddDefaultTokenProviders();

            Services.ConfigureApplicationCookie(config =>
            {
                config.Events = new CookieAuthenticationEvents
                {
                    OnRedirectToAccessDenied = ctx =>
                    {
                        ctx.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        return Task.FromResult(0);
                    },
                    OnRedirectToLogout = ctx =>
                    {
                        ctx.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        return Task.FromResult(0);
                    },
                    OnRedirectToReturnUrl = ctx =>
                    {
                        ctx.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        return Task.FromResult(0);
                    },
                    OnSignedIn = ctx =>
                    {
                        ctx.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        return Task.FromResult(0);
                    },
                    OnSigningIn = ctx =>
                    {
                        ctx.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        return Task.FromResult(0);
                    },
                    OnSigningOut = ctx =>
                    {
                        ctx.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        return Task.FromResult(0);
                    },
                    OnValidatePrincipal = ctx =>
                    {
                        ctx.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        return Task.FromResult(0);
                    },
                    OnRedirectToLogin = ctx => 
                    {
                        ctx.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                        return Task.FromResult(0);
                    }
                };
                /*
                configure.ReturnUrlParameter = "";
                configure.AccessDeniedPath = new PathString("");
                configure.LoginPath = new PathString("");
                configure.LogoutPath = new PathString("");
                configure.ExpireTimeSpan = TimeSpan.FromDays(30);
                */
            });

            Services
                .AddMvc(options =>
                {
                    options.ModelMetadataDetailsProviders.Add(new ValidationMetadataProviderJapanese("BlazorTemplate.Server.Properties.ValidationResourceJapanese", typeof(ValidationResourceJapanese)));
                    //options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            //services.AddDbContext<AuthenticationDbContext>(options =>);

            //Services.AddRazorPages();
            //Services.AddServerSideBlazor();

            Services.AddScoped<AccountService>();
            Services.AddSingleton<SpamBlockSharedService>();
            //Services.AddTransient< IUserStore<ApplicationUser>, TestApplicationUserStore >();
            //Services.AddTransient< IRoleStore<ApplicationRole>, TestApplicationRoleStore >();
        }

        private void ListAllRegisteredServices(IApplicationBuilder app)
        {
            app.Map("/allservices", builder => builder.Run(async context =>
            {
                var sb = new StringBuilder();
                sb.Append("<h1>全てのサービス</h1>");
                sb.Append("<table><thead>");
                sb.Append("<tr><th>タイプ名</th><th>ライフタイム</th><th>インスタンス</th></tr>");
                sb.Append("</thead><tbody>");
                foreach (var svc in Services.OrderBy(key => key.ServiceType.FullName))
                {
                    sb.Append("<tr>");
                    sb.Append($"<td>{svc.ServiceType.Name}</td>");
                    sb.Append($"<td>{svc.Lifetime}</td>");
                    sb.Append($"<td>{svc.ImplementationType?.FullName}</td>");
                    sb.Append("</tr>");
                }
                sb.Append("</tbody></table>");
                await context.Response.WriteAsync(sb.ToString());
            }));
        }
    }

    public class CsrfTokenCookieMiddleware
    {
        private readonly IAntiforgery _antiforgery;
        private readonly RequestDelegate _next;

        public CsrfTokenCookieMiddleware(IAntiforgery antiforgery, RequestDelegate next)
        {
            _antiforgery = antiforgery;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Cookies["X-CSRF-TOKEN"] == null)
            {
                var token = _antiforgery.GetAndStoreTokens(context);
                context.Response.Cookies.Append("X-CSRF-TOKEN", token.RequestToken, new CookieOptions { HttpOnly = false });
            }
            await _next(context);
        }
    }
}
