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

        // TODO:
        // CreateAsync
        // GetImagesAsync
        // SetDefaultAsync

        public async Task<bool> CreateAsync(User owner, byte[] image, CancellationToken cancellationToken = default)
        {
            if (ExistsAsync(owner, cancellationToken).Result) return false;

            var newAvatar = new Avatar()
            {
                Id = new Guid(),
                Image = image,
                LastUpdated = DateTime.Now
            };

            return await AvatarStore.CreateAsync(newAvatar, cancellationToken);
        }


        public async Task<bool> UpdateAsync(User owner, byte[] image, CancellationToken cancellationToken = default)
        {
            var result = await AvatarStore.FindByOwnerAsync(owner, cancellationToken);
            if (result == null) return false;

            result.Image = image;
            result.LastUpdated = DateTime.Now;

            return await AvatarStore.UpdateAsync(result, owner, cancellationToken);
        }

        public async Task<byte[]> GetImageAsync(User owner, CancellationToken cancellationToken = default)
        {
            var result = await AvatarStore.FindByOwnerAsync(owner, cancellationToken);

            if (result == null) return null;
            return result.Image;
        }

        public async Task<bool> ExistsAsync(User owner, CancellationToken cancellationToken = default)
        {
            var result = await AvatarStore.FindByOwnerAsync(owner, cancellationToken);
            
            if(result == null) return false;
            return true;
        }
    }
}
