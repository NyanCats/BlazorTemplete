using Microsoft.JSInterop;
using System.Threading.Tasks;
using System.Net.Http;
using System;
using Microsoft.AspNetCore.Components;
using BlazorTemplate.Commons.WebApis.Accounts;

namespace BlazorTemplate.Client.Services
{
    public delegate void UserInfomationEventHandler(object sender, UserInfomationEventArgs e);

    public class AccountService : ClientServiceBase
    {
        public event UserInfomationEventHandler UserInfomationChanged;

        private /*readonly*/ IJSRuntime JSRuntime { get; set; }

        public AccountService(IJSRuntime jsRuntime)
        {
            JSRuntime = jsRuntime;
        }



        protected virtual void OnUserInfomationChanged(object sender, UserInfomationEventArgs e)
        {
            UserInfomationChanged?.Invoke(this, e);
        }



        public async Task<CreateUserResult> Create(HttpClient http, CreateUserRequest request)
        {
            // await AddCsrfToken(http, JSRuntime);
            return await http.PostJsonAsync<CreateUserResult>("account", request);
        }

        public async Task<bool> Delete(HttpClient http)
        {
            // await AddCsrfToken(http, JSRuntime);
            var response = await http.DeleteAsync("account");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Validate(HttpClient http, ValidateRequest request)
        {
            // await AddCsrfToken(http, JSRuntime);
            var response = await http.PutJsonAsync<HttpResponseMessage>("account", request);

            return response.IsSuccessStatusCode;
        }

        public async Task<UserInfomationResult> GetUserInfomation(HttpClient http)
        {
            // await AddCsrfToken(http, JSRuntime);
            try
            {
                var response = await http.GetJsonAsync<UserInfomationResult>("account");
                OnUserInfomationChanged(this, new UserInfomationEventArgs(true, response.UserName));
                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                OnUserInfomationChanged(this, new UserInfomationEventArgs(false, string.Empty));
                return null;
            }
        }
    }

    public class UserInfomationEventArgs
    {
        public bool IsValid { get; protected set; }
        public string UserName { get; protected set; }

        public UserInfomationEventArgs(bool isValid, string userName)
        {
            IsValid = isValid;
            UserName = userName;
        }
    }
}
