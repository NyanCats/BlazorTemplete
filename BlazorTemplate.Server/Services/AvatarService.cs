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
        UserAvatarStore UserAvatarStore { get; set; }
        AvatarStore AvatarStore { get; set; }

        public AvatarService(UserAvatarStore userAvatarStore, AvatarStore avatarStore)
        {
            UserAvatarStore = userAvatarStore;
            AvatarStore = avatarStore;
        }

        public async Task<bool> CreateAsync(User owner, CancellationToken cancellationToken = default)
        {
            var userAvatar = await UserAvatarStore.ReadAsync(owner, cancellationToken);
            if (userAvatar != null) return false;

            var newAvatar = new Avatar()
            {
                Id = Guid.NewGuid(),
                Image = null,
                LastUpdated = DateTime.Now
            };

            var result1 = await UserAvatarStore.CreateAsync(owner, newAvatar, cancellationToken);
            var result2 = await AvatarStore.CreateAsync(newAvatar, cancellationToken);
            return result1 && result2;
        }

        public async Task<byte[]> GetImageAsync(Guid avatarId, CancellationToken cancellationToken = default)
        {
            var avatar = await AvatarStore.ReadAsync(avatarId, cancellationToken);
            if (avatar == null) return null;

            return avatar.Image;
        }

        public async Task<byte[]> GetImageAsync(User owner, CancellationToken cancellationToken = default)
        {
            var userAvatar = await UserAvatarStore.ReadAsync(owner, cancellationToken);
            if (userAvatar == null) return null;

            var avatar = await AvatarStore.ReadAsync(userAvatar.AvatarId);
            if (avatar == null) throw new Exception();

            return avatar.Image;
        }

        public async Task<bool> ExistsAsync(User owner, CancellationToken cancellationToken = default)
        {
            var userAvatar = await UserAvatarStore.ReadAsync(owner, cancellationToken);
            if (userAvatar == null) return false;

            return true;
        }

        public async Task<bool> UpdateAsync(User owner, byte[] image, CancellationToken cancellationToken = default)
        {
            var userAvatar = await UserAvatarStore.ReadAsync(owner, cancellationToken);
            if (userAvatar == null) return false;

            var avatar = await AvatarStore.ReadAsync(userAvatar.AvatarId);
            if (avatar == null) throw new Exception();

            avatar.Image = image;
            avatar.LastUpdated = DateTime.Now;

            var result = await AvatarStore.UpdateAsync(avatar, cancellationToken);
            return result;
        }

        public async Task<bool> DeleteAsync(User owner, CancellationToken cancellationToken = default)
        {
            var userAvatar = await UserAvatarStore.ReadAsync(owner, cancellationToken);
            if (userAvatar == null) return false;

            var avatar = await AvatarStore.ReadAsync(userAvatar.AvatarId);
            if (avatar == null) throw new Exception();

            var result1 = await AvatarStore.DeleteAsync(avatar, cancellationToken);
            var result2 = await UserAvatarStore.DeleteAsync(userAvatar, cancellationToken);

            return result1 && result2;
        }
    }
}
