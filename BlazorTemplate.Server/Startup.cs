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
using System;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using BlazorTemplate.Server.Entities;
using BlazorTemplate.Server.Properties.SampleProject.Resources;
using BlazorTemplate.Server.Properties;
using System.Net;
using Microsoft.Extensions.Hosting;
using BlazorTemplate.Server.Infrastructures.DataBases.Contexts;
using Microsoft.EntityFrameworkCore;
using BlazorTemplate.Server.Infrastructures.Stores;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;

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
            app.UseResponseCompression();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBlazorDebugging();
                ListAllRegisteredServices(app);
            }
            else
            {
                app.UseExceptionHandler("/Error");
                //app.UseRewriter(new RewriteOptions().AddRedirectToHttps());
                app.UseHsts();
            }

            //app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseClientSideBlazorFiles<Client.Startup>();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            //app.UseCookiePolicy();
            //app.UseRequestLocalization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapFallbackToClientSideBlazor<Client.Startup>("index.html");
            });

            //app.UseMiddleware<CsrfTokenCookieMiddleware>();
            
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            Services = services;
            
            Services.AddResponseCompression(options =>
            {
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[]
                {
                    MediaTypeNames.Application.Octet
                });
            });

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

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                options.User.RequireUniqueEmail = false;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_";
            });
            Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(30);
                options.LoginPath = "/";
                options.AccessDeniedPath = "/";
                options.SlidingExpiration = true;
            });
            Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["JwtIssuer"],
                        ValidAudience = Configuration["JwtAudience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtSecurityKey"]))
                    };
                    
                })
                .AddCookie(options =>
                {
                    options.LoginPath = $"/";
                    options.LogoutPath = $"/";
                    options.AccessDeniedPath = $"/";
                    options.ExpireTimeSpan = TimeSpan.FromDays(30);
                });
                
            Services.AddDbContext<ApplicationIdentityDbContext>((serviceProvider, options) => 
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });
            Services.AddDbContext<AvatarDbContext>((serviceProvider, options) =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("AvatarDbConnection"));
            });

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
                config.ReturnUrlParameter = string.Empty;
                config.AccessDeniedPath = string.Empty;
                config.LoginPath = string.Empty;
                config.LogoutPath = string.Empty;
                config.ExpireTimeSpan = TimeSpan.FromDays(30);
                */
            });
                Services.AddIdentity<User, Role>()
                    .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
                    .AddErrorDescriber<IdentityErrorDescriberJapanese>();
            //.AddDefaultTokenProviders();

            Services
                .AddMvc(options =>
                {
                    options.ModelMetadataDetailsProviders.Add(new ValidationMetadataProviderJapanese("BlazorTemplate.Server.Properties.ValidationResourceJapanese", typeof(ValidationResourceJapanese)));
                    //options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                });

            //Services.AddRazorPages();

            Services.AddScoped<AccountService>();
            Services.AddScoped<SessionService>();
            Services.AddScoped<AvatarService>();

            Services.AddSingleton<SpamBlockSharedService>();

            Services.AddScoped<AvatarStore>();
            Services.AddScoped<UserAvatarStore>();

            //Services.AddTransient< IUserStore<ApplicationUser>, TestApplicationUserStore >();
            //Services.AddTransient< IRoleStore<ApplicationRole>, TestApplicationRoleStore >();
        }

        private void ListAllRegisteredServices(IApplicationBuilder app)
        {
            app.Map("/allservices", builder => builder.Run(async context =>
            {
                var sb = new StringBuilder();
                sb.Append("<h1>All Services</h1>");
                sb.Append("<table><thead>");
                sb.Append("<tr><th>Type Name</th><th>Lifetime</th><th>Instance</th></tr>");
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
