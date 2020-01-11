using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using System.Threading;
using System.Net.Http.Headers;
using System.IO;
using Blazor.FileReader;
using System;
using BlazorTemplate.Shared.WebApis.Avatars;
using Microsoft.AspNetCore.Components;

namespace BlazorTemplate.Client.Services
{
    public delegate void AvatarEventHandler(object sender, string avatar);

    public class AvatarService : NetworkServiceBase
    {
        public event AvatarEventHandler AvatarChanged;


        public override string EndPointUri => "api/avatar";
        HttpClient HttpClient { get; set; }
        public string MyAvatar { get; protected set; } = string.Empty;

        public AvatarService(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        protected virtual void OnAvatarChanged(object sender, string avatar)
        {
            AvatarChanged?.Invoke(this, avatar);
        }

        public async Task<bool> UpdateAsync(byte[] data, IFileInfo fileInfo, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response;

            using (var content = new MultipartFormDataContent())
            using (var memoryStream = new MemoryStream(data))
            using (var streamContent = new StreamContent(memoryStream))
            {
                streamContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = "file",
                    FileName = fileInfo.Name
                };
                streamContent.Headers.ContentType = new MediaTypeHeaderValue(fileInfo.Type);
                content.Add(streamContent);

                response = await HttpClient.PutAsync(EndPointUri, content, cancellationToken);

                return response.IsSuccessStatusCode;
            }
        }

        public async Task<bool> DeleteAsync()
        {
            await HttpClient.DeleteAsync("avatar");
            await GetMyAvatarAsync();

            OnAvatarChanged(this, string.Empty);
            return true;
        }
        
        public async Task<bool> GetMyAvatarAsync()
        {
            var response = await HttpClient.GetAsync(EndPointUri);
            if (!response.IsSuccessStatusCode) return false;

            var data = await response.Content.ReadAsByteArrayAsync();

            // TODO:
            var type = "image/png";
            MyAvatar = $"data:{type};base64,{Convert.ToBase64String(data)}";

            OnAvatarChanged(this, MyAvatar);
            return true;
        }
    }
}
