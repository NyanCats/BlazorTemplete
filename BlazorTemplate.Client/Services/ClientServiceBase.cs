using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace BlazorTemplate.Client.Services
{
    public class ClientServiceBase
    {
        protected async Task AddCsrfToken(HttpClient http, IJSRuntime js)
        {
            var cookie = await js.InvokeAsync<string>("getCookies");

            var csrf = cookie.Split(';')
            .Select(v => v.TrimStart().Split('='))
            .Where(s => s[0] == "X-CSRF-TOKEN")
            .Select(s => s[1])
            .FirstOrDefault();

            if (csrf != null && !http.DefaultRequestHeaders.Contains("X-CSRF-TOKEN")) http.DefaultRequestHeaders.Add("X-CSRF-TOKEN", csrf);
        }
    }
}
