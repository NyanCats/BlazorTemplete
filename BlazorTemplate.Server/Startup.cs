using BlazorTemplate.Server.Infrastructures.Stores;
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
using BlazorTemplate.Server.Entities.Identities;
using BlazorTemplate.Server.Properties.SampleProject.Resources;
using BlazorTemplate.Server.Properties;
using System.Net;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.Extensions.Hosting;

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
                //app.UseHsts();
            }

            app.UseRouting();
            app.UseResponseCompression();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();

            //app.UseHttpsRedirection();

            /*
            app.UseRequestLocalization
            (
                new RequestLocalizationOptions
                {
                    DefaultRequestCulture = new RequestCulture("jp-JP"),
                    SupportedCultures = new List<CultureInfo>()
                    {
                        new CultureInfo("jp-JP")
                    },
                    SupportedUICultures = new List<CultureInfo>()
                    {
                        new CultureInfo("jp-JP")
                    }
                }
            );
            */
            app.UseCookiePolicy();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
            
            //app.UseMiddleware<CsrfTokenCookieMiddleware>();
            app.UseBlazor<Client.Startup>();
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
            });

            Services.AddAuthentication(options =>
                    {
                        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    })
                    .AddCookie(options =>
                    {
                        /*
                        options.AccessDeniedPath = new PathString("/Account/AccessDenied");
                        options.LoginPath = new PathString("/Account/Login");
                        options.LogoutPath = new PathString("/Account/Logout");
                        */
                        options.ExpireTimeSpan = TimeSpan.FromDays(30);
                    });

            Services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => context?.User?.Identity?.IsAuthenticated != true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            
            Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme);

            Services.AddIdentity<ApplicationUser, ApplicationRole>()
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

            Services.AddMvc(options =>
                    {
                        options.ModelMetadataDetailsProviders.Add(new ValidationMetadataProviderJapanese("BlazorTemplate.Server.Properties.ValidationResourceJapanese", typeof(ValidationResourceJapanese)));
                        //options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                    })

                    .AddNewtonsoftJson(options =>
                    {
                        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                        options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                        // options.SerializerSettings.Converters.Add(new StringEnumConverter());
                    })
                    .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            /*
            services.AddDbContext<AuthenticationDbContext>(options =>
            {
                // Configure the context to use Microsoft SQL Server.
                options.UseSqlServer(Configuration["ConnectionStrings:AuthenticationConnection"]);
                options.UseOpenIddict();
            });
            */

            // note:
            // Transient 	AddTransient() 	インジェクション毎にインスタンスが生成されます。
            // Scoped 	    AddScoped() 	１リクエスト毎に１インスタンス生成されます。
            // Singleton 	AddSingleton() 	アプリケーション内で１つのインスタンスが生成されます。
            Services.AddScoped<AccountService>();
            Services.AddSingleton<SpamBlockSharedService>();

            Services.AddTransient< IUserStore<ApplicationUser>, TestApplicationUserStore >();
            Services.AddTransient< IRoleStore<ApplicationRole>, TestApplicationRoleStore >();
        }

        private void ListAllRegisteredServices(IApplicationBuilder app)
        {
            app.Map("/allservices", builder => builder.Run(async context =>
            {
                var sb = new StringBuilder();
                sb.Append("<h1>All Services</h1>");
                sb.Append("<table><thead>");
                sb.Append("<tr><th>Type</th><th>Lifetime</th><th>Instance</th></tr>");
                sb.Append("</thead><tbody>");
                foreach (var svc in Services)
                {
                    sb.Append("<tr>");
                    sb.Append($"<td>{svc.ServiceType.FullName}</td>");
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
