using System.Net.Http;
using System.Threading.Tasks;

using BlazorTemplate.Shared.WebApis.Sessions;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using System.Text.Json;
using System.Text;
using System.Net.Http.Headers;
using System.Threading;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using System.Net;

namespace BlazorTemplate.Client.Services
{
    public delegate void SessionEventHandler();
    public delegate void SessionStateEventHandler(SessionState state);

    public enum SessionState : int
    {
        LoggedOut,
        LoggingIn,
        LoggedIn,
        LogInError
    }

    public class SessionService : NetworkServiceBase, INetworkService
    {
        public event SessionStateEventHandler StateChanged;
        public event SessionEventHandler LoggedIn;
        public event SessionEventHandler LoggingIn;
        public event SessionEventHandler LogInError;
        public event SessionEventHandler LoggedOut;
        
        protected virtual void OnStateChanged(SessionState state) => StateChanged?.Invoke(state);
        protected virtual void OnLoggedIn() => LoggedIn?.Invoke();
        protected virtual void OnLoggingIn() => LoggingIn?.Invoke();
        protected virtual void OnLogInError() => LogInError?.Invoke();
        protected virtual void OnLoggedOut() => LoggedOut?.Invoke();

        public override string EndPointUri => "api/session";

        AuthenticationStateProvider AuthenticationStateProvider { get; set; }
        ILocalStorageService LocalStorage { get; set; }

        public SessionService(  HttpClient httpClient,
                                AuthenticationStateProvider authenticationStateProvider,
                                ILocalStorageService localStorage) : base(httpClient)
        {
            AuthenticationStateProvider = authenticationStateProvider;
            LocalStorage = localStorage;
        }

        public async Task<bool> LoginAsync(LoginRequest request, CancellationToken ctoken = default)
        {
            NotifySessionStateChanged(SessionState.LoggingIn);

            var (code, response) = await PostAsync<LoginRequest, LoginResponse>(request);

            if (code != HttpStatusCode.OK)
            {
                NotifySessionStateChanged(SessionState.LogInError);
                return false;
            }
            
            await LocalStorage.SetItemAsync("authToken", response.Token);
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", response.Token);

            (AuthenticationStateProvider as ApiAuthenticationStateProvider).MarkUserAsAuthenticated(request.UserName);

            NotifySessionStateChanged(SessionState.LoggedIn);

            return true;
        }

        public async Task LogoutAsync()
        {
            await LocalStorage.RemoveItemAsync("authToken");
            HttpClient.DefaultRequestHeaders.Authorization = null;

            await HttpClient.DeleteAsync(EndPointUri);

            (AuthenticationStateProvider as ApiAuthenticationStateProvider).MarkUserAsLoggedOut();
            
            NotifySessionStateChanged(SessionState.LoggedOut);
        }

        public async Task<bool> VerifyAsync()
        {
            var task = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            return task.User.Identity.IsAuthenticated;
        }

        public async Task<bool> ValidateAsync()
        {
            var response =  await HttpClient.GetAsync(EndPointUri);
            if (response.IsSuccessStatusCode)
            {
                NotifySessionStateChanged(SessionState.LoggedIn);
                return true;
            }

            NotifySessionStateChanged(SessionState.LoggedOut);
            return false;
        }

        protected void NotifySessionStateChanged(SessionState state)
        {
            OnStateChanged(state);
            switch(state)
            {
                case SessionState.LoggedOut: OnLoggedOut(); break;
                case SessionState.LoggingIn: OnLoggingIn(); break;
                case SessionState.LoggedIn: OnLoggedIn(); break;
                case SessionState.LogInError: OnLogInError(); break;
            }
        }
    }
}
