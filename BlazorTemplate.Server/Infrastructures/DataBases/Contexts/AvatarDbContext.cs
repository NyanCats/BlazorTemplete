using BlazorTemplate.Server.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorTemplate.Server.Infrastructures.DataBases.Contexts
{
    public class AvatarDbContext<TKey> : DbContext
    {
        public DbSet<Avatar<TKey>> Avatars { get; set; }

        public AvatarDbContext(DbContextOptions<AvatarDbContext<TKey>> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Core Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Core Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
