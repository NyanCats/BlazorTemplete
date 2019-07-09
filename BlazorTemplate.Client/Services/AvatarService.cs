using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using System.Threading;
using System.Net.Http.Headers;
using System.IO;
using Blazor.FileReader;
using System;

namespace BlazorTemplate.Client.Services
{
    public delegate void AvatarEventHandler(object sender, string avatar);

    public class AvatarService : ClientServiceBase
    {
        public event AvatarEventHandler AvatarChanged;

        public string MyAvatar { get; protected set; }

        private IJSRuntime JSRuntime { get; set; }

        public AvatarService(IJSRuntime jsRuntime)
        {
            JSRuntime = jsRuntime;
        }



        protected virtual void OnAvatarChanged(object sender, string avatar)
        {
            AvatarChanged?.Invoke(this, avatar);
        }

        

        public async Task<bool> Update(HttpClient http, byte[] data, IFileInfo fileInfo, CancellationToken cancellationToken = default)
        {
            HttpResponseMessage response;
            // await AddCsrfToken(http, JSRuntime);
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

                response = await http.PutAsync("avatar", content, cancellationToken);

                return response.IsSuccessStatusCode;
            }
        }
        /*
        public async Task<bool> Update(HttpClient http, IFormFile file, CancellationToken cancellationToken = default)
        {
            using (var stream = new StreamContent(file.OpenReadStream()))
            {
                await http.PutAsync("avatar", stream, cancellationToken);
            }

            OnAvatarChanged(this, new AvatarEventArgs());
            return true;
        }

        public async Task<bool> Delete(HttpClient http)
        {
            await http.DeleteAsync("avatar");

            OnAvatarChanged(this, new AvatarEventArgs());
            return true;
        }
        */
        
        public async Task GetMyAvatar(HttpClient http)
        {
            // await AddCsrfToken(http, JSRuntime);
            var response = await http.GetAsync("avatar");
            if (!response.IsSuccessStatusCode) return;
            var data = await response.Content.ReadAsByteArrayAsync();
            var type = "image/png";
            MyAvatar = $"data:{type};base64,{Convert.ToBase64String(data)}";

            OnAvatarChanged(this, MyAvatar);
        }
    }
}
