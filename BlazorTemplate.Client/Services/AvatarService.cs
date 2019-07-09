using System;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

using BlazorTemplate.Shared.WebApis.Accounts;
using System.Threading;
using System.Net.Http.Headers;
using System.IO;

namespace BlazorTemplate.Client.Services
{
    public delegate void AvatarEventHandler(object sender, AvatarEventArgs e);

    public class AvatarService : ClientServiceBase
    {
        public event AvatarEventHandler AvatarChanged;

        public  
        private IJSRuntime JSRuntime { get; set; }

        public AvatarService(IJSRuntime jsRuntime)
        {
            JSRuntime = jsRuntime;
        }



        protected virtual void OnAvatarChanged(object sender, AvatarEventArgs e)
        {
            AvatarChanged?.Invoke(this, e);
        }

        

        public async Task<bool> Update(HttpClient http, byte[] data, CancellationToken cancellationToken = default)
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
                    FileName = "avatar.png"
                };
                streamContent.Headers.ContentType = new MediaTypeHeaderValue("image/png");
                content.Add(streamContent);

                response = await http.PutAsync("avatar", content, cancellationToken);

                OnAvatarChanged(this, new AvatarEventArgs());
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
        /*
        public async Task<UserInfomationResult> GetMyAvatar(HttpClient http)
        {
            // await AddCsrfToken(http, JSRuntime);
            try
            {
                var response = await http.GetAsync("avatar");
                OnAvatarChanged(this, new AvatarEventArgs());
                return response;
            }
            catch (Exception e)
            {
                OnAvatarChanged(this, new AvatarEventArgs());
                return null;
            }
        }*/
    }

    public class AvatarEventArgs
    {


        public AvatarEventArgs()
        {

        }
    }
}
