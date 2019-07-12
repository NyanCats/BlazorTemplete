using BlazorTemplate.Server.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorTemplate.Server.Infrastructures.DataBases.Contexts
{
    public class AvatarDbContext : DbContext
    {
        public DbSet<UserAvatar> UserAvatar { get; set; }
        public DbSet<Avatar> Avatars { get; set; }

        public AvatarDbContext(DbContextOptions<AvatarDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
