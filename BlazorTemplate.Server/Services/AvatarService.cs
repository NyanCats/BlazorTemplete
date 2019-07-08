using BlazorTemplate.Server.Entities;
using BlazorTemplate.Server.Infrastructures.Stores;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorTemplate.Client.Services
{
    public class AvatarService
    {
        AvatarStore<Guid> AvatarStore { get; set; }

        public AvatarService( [FromServices] AvatarStore<Guid> avatarStore)
        {
            AvatarStore = avatarStore;
        }

        // TODO:
        // CreateAsync
        // GetImagesAsync
        // SetDefaultAsync

        public async Task<bool> UpdateAsync(string ownerId, byte[] image, CancellationToken cancellationToken = default)
        {
            Guid guid;
            if (!Guid.TryParse(ownerId, out guid)) return false;
            if (!ExistsAsync(ownerId, cancellationToken).Result) return false;

            var newAvatar = new Avatar<Guid>()
            {
                OwnerId = guid,
                Image = image,
                LastUpdated = DateTime.Now
            };

            return await AvatarStore.UpdateAsync(newAvatar, cancellationToken);
        }

        public async Task<byte[]> GetImageAsync(string ownerId, CancellationToken cancellationToken = default)
        {
            Guid guid;
            if (!Guid.TryParse(ownerId, out guid)) return null;

            var result = await AvatarStore.FindByOwnerIdAsync(guid, cancellationToken);

            if (result == null) return null;
            return result.Image;
        }

        public async Task<bool> ExistsAsync(string ownerId, CancellationToken cancellationToken = default)
        {
            Guid guid;
            if (!Guid.TryParse(ownerId, out guid)) return false;

            var result = await AvatarStore.FindByOwnerIdAsync(guid, cancellationToken);
            
            if(result == null) return false;
            return true;
        }
    }
}
