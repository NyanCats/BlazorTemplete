using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

using BlazorTemplate.Shared.WebApis.Sessions;
using System;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using System.Text.Json;
using System.Text;
using System.Net.Http.Headers;

namespace BlazorTemplate.Client.Services
{
    public delegate void SessionEventHandler(SessionService sender);

    public class SessionService : NetworkServiceBase
    {
        public event SessionEventHandler LoggedIn;
        public event SessionEventHandler LoggedOut;

        protected virtual void OnLoggedIn(SessionService sender) => LoggedIn?.Invoke(this);
        protected virtual void OnLoggedOut(SessionService sender) => LoggedOut?.Invoke(this);

        public override string EndPointUri => "session";
        HttpClient HttpClient { get; set; }
        AuthenticationStateProvider AuthenticationStateProvider { get; set; }
        ILocalStorageService LocalStorage { get; set; }

        public SessionService(  HttpClient httpClient,
                                AuthenticationStateProvider authenticationStateProvider,
                                ILocalStorageService localStorage)
        {
            HttpClient = httpClient;
            AuthenticationStateProvider = authenticationStateProvider;
            LocalStorage = localStorage;
        }

        public async Task<bool> LoginAsync(LoginRequest request)
        {
            var loginAsJson = JsonSerializer.Serialize(request);
            var response = await HttpClient.PostAsync(EndPointUri, new StringContent(loginAsJson, Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode) return false;

            var loginResult = JsonSerializer.Deserialize<LoginResult>(await response.Content.ReadAsStringAsync());

            await LocalStorage.SetItemAsync("authToken", loginResult.Token);
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", loginResult.Token);

            (AuthenticationStateProvider as ApiAuthenticationStateProvider).MarkUserAsAuthenticated(request.UserName);
            OnLoggedIn(this);

            return true;
        }

        public async Task LogoutAsync()
        {
            await LocalStorage.RemoveItemAsync("authToken");
            HttpClient.DefaultRequestHeaders.Authorization = null;

            await HttpClient.DeleteAsync(EndPointUri);

            (AuthenticationStateProvider as ApiAuthenticationStateProvider).MarkUserAsLoggedOut();
            OnLoggedOut(this);
        }

        public async Task<bool> VerifyAsync()
        {
            var task = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            return task.User.Identity.IsAuthenticated;
        }

        public async Task<bool> ValidateAsync()
        {
            var response =  await HttpClient.GetAsync(EndPointUri);
            if (response.IsSuccessStatusCode) return true;

            await LogoutAsync();

            return false;
        }
    }
}
