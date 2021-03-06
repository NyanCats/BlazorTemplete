﻿using BlazorTemplate.Server.Entities;
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
    public class AvatarStore : IDisposable
    {
        public AvatarDbContext AvatarDbContext { get; private set; }

        // TODO: Review database structure.
        public AvatarStore(AvatarDbContext avatarDbContext)
        {
            AvatarDbContext = avatarDbContext;
        }

        public async Task<bool> CreateAsync(Avatar avatar, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            
            if (avatar == null) throw new ArgumentNullException(nameof(avatar));
            
            await AvatarDbContext.AddAsync(avatar, cancellationToken);
            await AvatarDbContext.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<Avatar> ReadAsync(Guid id, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            return await AvatarDbContext.Avatars.FindAsync(id);
        }

        public async Task<bool> UpdateAsync(Avatar avatar, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (avatar == null) throw new ArgumentNullException(nameof(avatar));

            AvatarDbContext.Update(avatar);
            await AvatarDbContext.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<bool> DeleteAsync(Avatar avatar, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            if (avatar == null) throw new ArgumentNullException(nameof(avatar));

            AvatarDbContext.Remove(avatar);
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
