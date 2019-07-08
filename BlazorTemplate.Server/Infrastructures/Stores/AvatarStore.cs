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
    public class AvatarStore<TKey> : IDisposable
    {
        public AvatarDbContext<TKey> AvatarDbContext { get; private set; }

        public AvatarStore(AvatarDbContext<TKey> avatarDbContext)
        {
            AvatarDbContext = avatarDbContext;
        }

        public async Task<bool> CreateAsync(Avatar<TKey> avatar, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            
            if (avatar == null) throw new ArgumentNullException(nameof(avatar));

            await AvatarDbContext.AddAsync(avatar, cancellationToken);
            await AvatarDbContext.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<bool> DeleteAsync(Avatar<TKey> avatar, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (avatar == null) throw new ArgumentNullException(nameof(avatar));

            AvatarDbContext.Remove(avatar);
            await AvatarDbContext.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<Avatar<TKey>> FindByOwnerIdAsync(TKey ownerId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            return await AvatarDbContext.FindAsync<Avatar<TKey>>(typeof(TKey), ownerId, cancellationToken);
        }

        public async Task<bool> UpdateAsync(Avatar<TKey> avatar, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (avatar == null) throw new ArgumentNullException(nameof(avatar));

            var existingAvatar = await FindByOwnerIdAsync(avatar.OwnerId, cancellationToken);
            if(existingAvatar == null) return false;

            AvatarDbContext.Update(avatar);
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
