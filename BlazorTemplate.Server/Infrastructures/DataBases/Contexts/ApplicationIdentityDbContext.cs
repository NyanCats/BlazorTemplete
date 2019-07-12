using BlazorTemplate.Server.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorTemplate.Server.Infrastructures.DataBases.Contexts
{
    public class ApplicationIdentityDbContext : IdentityDbContext<User, Role, Guid>
    {
        public ApplicationIdentityDbContext(DbContextOptions<ApplicationIdentityDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Override default AspNet Identity table names
            builder.Entity<User>(entity => { entity.ToTable(name: "Users"); });
            builder.Entity<Role>(entity => { entity.ToTable(name: "Roles"); });
            builder.Entity<IdentityUserRole<Guid>>(entity => { entity.ToTable("UserRoles"); });
            builder.Entity<IdentityUserClaim<Guid>>(entity => { entity.ToTable("UserClaims"); });
            builder.Entity<IdentityUserLogin<Guid>>(entity => { entity.ToTable("UserLogins"); });
            builder.Entity<IdentityUserToken<Guid>>(entity => { entity.ToTable("UserTokens"); });
            builder.Entity<IdentityRoleClaim<Guid>>(entity => { entity.ToTable("RoleClaims"); });
        }
    }
}
