using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorTemplate.Shared.WebApis;
using System.Runtime.InteropServices.ComTypes;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;

namespace BlazorTemplate.Client.Services
{
    public interface INetworkService
    {
        public abstract string EndPointUri { get; }
    }

    public abstract class NetworkServiceBase
    {
        public abstract string EndPointUri { get; }

        protected HttpClient HttpClient { get; set; }

        public NetworkServiceBase(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        protected async Task<(HttpStatusCode, T1)> GetAsync<T1>() where T1 : IApiResponse
        {
            var response = await HttpClient.GetFromJsonAsync<HttpResponseMessage>(EndPointUri);
            return (response.StatusCode, await response.Content.ReadFromJsonAsync<T1>());
        }

        protected async Task<(HttpStatusCode, T2)> PostAsync<T1,T2>(T1 request) where T1 : IApiRequest where T2 : IApiResponse
        {
            var response = await HttpClient.PostAsJsonAsync(EndPointUri, request);
            return (response.StatusCode, await response.Content.ReadFromJsonAsync<T2>());
        }

        protected async Task<(HttpStatusCode, T2)> PutAsync<T1, T2>(T1 request) where T1 : IApiRequest where T2 : IApiResponse
        {
            var response = await HttpClient.PutAsJsonAsync(EndPointUri, request);
            return (response.StatusCode, await response.Content.ReadFromJsonAsync<T2>());
        }

        protected async Task<HttpStatusCode> DeleteAsync()
        {
            var response = await HttpClient.DeleteAsync(EndPointUri);
            return response.StatusCode;
        }


    }
}
