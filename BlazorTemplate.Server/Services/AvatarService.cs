using BlazorTemplate.Server.Entities;
using BlazorTemplate.Server.Infrastructures.Stores;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorTemplate.Server.Services
{
    public class AvatarService
    {
        AvatarStore<Guid> AvatarStore { get; set; }

        public AvatarService( [FromServices] AvatarStore<Guid> avatarStore)
        {
            AvatarStore = avatarStore;
        }

        public async Task<bool> CreateAsync(User owner, CancellationToken cancellationToken = default)
        {
            var avatar = await AvatarStore.ReadByOwnerAsync(owner, cancellationToken);
            if (avatar != null) return false;

            var newAvatar = new Avatar()
            {
                AvatarId = new Guid(),
                OwnerId = owner.Id,
                Image = null,
                LastUpdated = DateTime.Now
            };

            var result = await AvatarStore.CreateAsync(newAvatar, cancellationToken);
            return result;
        }


        public async Task<bool> UpdateAsync(User owner, byte[] image, CancellationToken cancellationToken = default)
        {
            var avatar = await AvatarStore.ReadByOwnerAsync(owner, cancellationToken);
            if (avatar == null) return false;

            avatar.Image = image;
            avatar.LastUpdated = DateTime.Now;

            var result = await AvatarStore.UpdateAsync(avatar, owner, cancellationToken);
            return result;
        }

        public async Task<byte[]> GetImageAsync(User owner, CancellationToken cancellationToken = default)
        {
            var result = await AvatarStore.ReadByOwnerAsync(owner, cancellationToken);
            if (result == null) return null;

            return result.Image;
        }

        public async Task<bool> ExistsAsync(User owner, CancellationToken cancellationToken = default)
        {
            var result = await AvatarStore.ReadByOwnerAsync(owner, cancellationToken);
            if(result == null) return false;

            return true;
        }

        public async Task<bool> DeleteAsync(User owner, CancellationToken cancellationToken = default)
        {
            var avatar = await AvatarStore.ReadByOwnerAsync(owner, cancellationToken);
            if (avatar == null) return false;

            var result = await AvatarStore.DeleteAsync(avatar, cancellationToken);

            return result;
        }
    }
}
