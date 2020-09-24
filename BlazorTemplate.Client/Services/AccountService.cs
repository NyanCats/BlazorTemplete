using System;
using System.Net.Http;
using System.Threading.Tasks;
using BlazorTemplate.Shared.WebApis.Accounts;
using Blazored.LocalStorage;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net;

namespace BlazorTemplate.Client.Services
{
    public delegate void UserNameEventHandler(AccountService sender, string username);

    public class AccountService : NetworkServiceBase, INetworkService
    {
        public event UserNameEventHandler UserNameChanged;

        ILocalStorageService LocalStorage { get; set; }

        public override string EndPointUri => "api/account";

        public AccountService(  HttpClient httpClient,
                                AuthenticationStateProvider authenticationStateProvider,
                                ILocalStorageService localStorage) : base(httpClient)
        {
            LocalStorage = localStorage;
        }



        protected virtual void OnUserNameChanged(AccountService sender, string username) => UserNameChanged?.Invoke(this, username);



        public async Task<CreateAccountResponse> CreateAsync(CreateAccountRequest request)
        {
            var(code, response) = await PostAsync<CreateAccountRequest, CreateAccountResponse>(request);

            return response;

            //var result = await HttpClient.PostAsJsonAsync<CreateAccountResponse>(EndPointUri, request);
            //return result;
        }

        public new async Task DeleteAsync()
        {
            await base.DeleteAsync();
            //var response = await HttpClient.DeleteAsync(EndPointUri);
            //return response.IsSuccessStatusCode;
        }

        public async Task<bool> ValidateAsync(ValidateAccountRequest request)
        {
            var (code, response) = await PutAsync<ValidateAccountRequest, ValidateAccountResponse>(request);
            
            if (code == HttpStatusCode.OK) return true;
            return false;
        }

        public async Task<GetUserInfomationResponse> GetUserInfomationAsync()
        {
            try
            {
                var response = await HttpClient.GetFromJsonAsync<GetUserInfomationResponse>(EndPointUri);
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
