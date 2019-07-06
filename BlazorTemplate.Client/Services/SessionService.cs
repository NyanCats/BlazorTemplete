using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

using BlazorTemplate.Shared.WebApis.Sessions;
using System;

namespace BlazorTemplate.Client.Services
{
    public class SessionService : ClientServiceBase
    {
        private IJSRuntime JSRuntime { get; set; }


        public SessionService(IJSRuntime jsRuntime)
        {
            JSRuntime = jsRuntime;
        }

        public async Task<bool> Login(HttpClient http, LoginRequest request)
        {
            // await AddCsrfToken(http);

            try
            {
                await http.PostJsonAsync("session", request);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> Logout(HttpClient http)
        {
            // await AddCsrfToken(http);
            var response = await http.DeleteAsync("session");
            switch(response.StatusCode)
            {
                case HttpStatusCode.OK  :   return true;
                default                 :   return false;
            }
        }

        public async Task<bool> ValidateCookie(HttpClient http)
        {
            // await AddCsrfToken(http);
            var response =  await http.GetAsync("session");
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK  : return true;
                default                 : return false;
            }
        }
    }
}
