using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

using BlazorTemplate.Shared.WebApis.Sessions;

namespace BlazorTemplate.Client.Services
{
    public class SessionService : ClientServiceBase
    {
        private /*readonly*/ IJSRuntime JSRuntime { get; set; }


        public SessionService(IJSRuntime jsRuntime)
        {
            JSRuntime = jsRuntime;
        }

        public async Task<LoginResult> Login(HttpClient http, LoginRequest request)
        {
            // await AddCsrfToken(http);
            return await http.PostJsonAsync<LoginResult>("session", request);/*
            try
            {
                return await http.PostJsonAsync<LoginResult>("session", request);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.Message);
            }
            catch(ArgumentException e)
            {
                Console.WriteLine(e.Message);
            }
            return null;*/
            /*
            string postData = Json.Serialize(command);

            var resposnse = await http.PostAsync("account/login", new StringContent(postData, Encoding.UTF8, "application/json"));
            var content = await resposnse.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(content))
            {
                await UserInfomation(http);
                return new LoginCommandResult(false, new List<string>() { "ログインに失敗しました。" });
            }

            var jsonObject = JsonConvert.DeserializeObject<JObject>(content, new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Populate });

            var result = new LoginCommandResult(false) { Errors = new List<string>() };

            if (jsonObject.TryGetValue("success", out JToken successToken)) result.Success = successToken.Value<bool>();
            if (jsonObject.TryGetValue("errors", out JToken errorsToken)) result.Errors.AddRange(errorsToken.Values<string>());

            return result;*/
        }

        public async Task<bool> Logout(HttpClient http)
        {
            // await AddCsrfToken(http);
            var response = await http.DeleteAsync("session");
            switch(response.StatusCode)
            {
                case HttpStatusCode.OK          :   return true;
                case HttpStatusCode.NoContent   :   return false;
                default: throw new Exception();
            }

            /*
            string postData = Json.Serialize(command);

            var resposnse = await http.PostAsync("account/logout", new StringContent(postData, Encoding.UTF8, "application/json"));
            var content = await resposnse.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(content))
            {
                return new LogoutCommandResult(false);
            }

            var jsonObject = JsonConvert.DeserializeObject<JObject>(content, new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Populate });

            var result = new LogoutCommandResult(false);

            if (jsonObject.TryGetValue("success", out JToken errorsToken)) result.Success = errorsToken.Value<bool>();

            return result;*/
        }

        public async Task<bool> ValidateCookie(HttpClient http)
        {
            // await AddCsrfToken(http);
            var response =  await http.GetAsync("session");
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK: return true;
                case HttpStatusCode.Unauthorized: return false;
                default: throw new Exception();
            }
            /*
            var resposnse = await http.GetAsync("account/validatecookie");
            var content = await resposnse.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(content)) return new ValidateCookieCommandResult();

            var jsonObject = JsonConvert.DeserializeObject<JObject>(content, new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Populate });

            var result = new ValidateCookieCommandResult();

            if (jsonObject.TryGetValue("isValid", out JToken isValidToken)) result.IsValid = isValidToken.Value<bool>();

            return result;*/
        }
    }
}
