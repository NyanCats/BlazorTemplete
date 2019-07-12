using BlazorTemplate.Server.Entities;
using BlazorTemplate.Server.Properties;
using BlazorTemplate.Server.Infrastructures.DataBases.Contexts;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.AspNetCore.Identity;

namespace BlazorTemplate.Server.Infrastructures.Stores
{
    public class UserAvatarStore : IDisposable
    {
        public AvatarDbContext AvatarDbContext { get; private set; }

        public UserAvatarStore(AvatarDbContext avatarDbContext)
        {
            AvatarDbContext = avatarDbContext;
        }

        public async Task<bool> CreateAsync(User user, Avatar avatar, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (avatar == null) throw new ArgumentNullException(nameof(avatar));

            var newUserAvatar = new UserAvatar() { UserId = user.Id, AvatarId = avatar.Id };

            await AvatarDbContext.UserAvatar.AddAsync(newUserAvatar, cancellationToken);
            await AvatarDbContext.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<UserAvatar> ReadAsync(User user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (user == null) throw new ArgumentNullException(nameof(user));

            return await AvatarDbContext.UserAvatar.FindAsync(user.Id);
        }

        public async Task<bool> UpdateAsync(UserAvatar userAvatar, User owner, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (userAvatar == null) throw new ArgumentNullException(nameof(userAvatar));

            AvatarDbContext.Update(userAvatar);
            await AvatarDbContext.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<bool> DeleteAsync(UserAvatar userAvatar, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (userAvatar == null) throw new ArgumentNullException(nameof(userAvatar));

            AvatarDbContext.UserAvatar.Remove(userAvatar);
            await AvatarDbContext.SaveChangesAsync(cancellationToken);

            return true;
        }

        public void Dispose()
        {
            _disposed = true;
        }
        protected void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }
        private bool _disposed;


    }
}
