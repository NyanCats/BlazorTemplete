using System;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using BlazorTemplate.Shared.WebApis.Accounts;
using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;

namespace BlazorTemplate.Client.Services
{
    public delegate void UserNameEventHandler(AccountService sender, string username);

    public class AccountService : NetworkServiceBase
    {
        public event UserNameEventHandler UserNameChanged;

        HttpClient HttpClient { get; set; }
        ILocalStorageService LocalStorage { get; set; }

        public override string EndPointUri => "api/account";

        public AccountService(  HttpClient httpClient,
                                AuthenticationStateProvider authenticationStateProvider,
                                ILocalStorageService localStorage)
        {
            HttpClient = httpClient;
            LocalStorage = localStorage;
        }



        protected virtual void OnUserNameChanged(AccountService sender, string username) => UserNameChanged?.Invoke(this, username);



        public async Task<CreateAccountResult> CreateAsync(CreateAccountRequest request)
        {

            var result = await HttpClient.PostJsonAsync<CreateAccountResult>(EndPointUri, request);
            return result;
        }

        public async Task<bool> DeleteAsync()
        {
            var response = await HttpClient.DeleteAsync(EndPointUri);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ValidateAsync(ValidateAccountRequest request)
        {
            var response = await HttpClient.PutJsonAsync<HttpResponseMessage>(EndPointUri, request);
            return response.IsSuccessStatusCode;
        }

        public async Task<GetUserInfomationResult> GetUserInfomationAsync()
        {
            try
            {
                var response = await HttpClient.GetJsonAsync<GetUserInfomationResult>(EndPointUri);
                OnUserNameChanged(this, response.UserName);
                return response;
            }
            catch (Exception)
            {
                OnUserNameChanged(this,  string.Empty);
                return null;
            }
        }
    }
}
