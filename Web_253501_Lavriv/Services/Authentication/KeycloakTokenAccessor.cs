using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using WEB_253501_LAVRIV.HelperClasses;

namespace WEB_253501_LAVRIV.Services.Authentication
{
    public class KeycloakTokenAccessor : ITokenAccessor
    {
        private readonly KeycloakData _keycloakData;
        private readonly HttpContext? _httpContext;
        private readonly HttpClient _httpClient;


        public KeycloakTokenAccessor(IOptions<KeycloakData> options, IHttpContextAccessor httpContextAccessor, HttpClient httpClient)
        {
            _keycloakData = options.Value;
            _httpContext = httpContextAccessor.HttpContext;
            _httpClient = httpClient;
        }

        public async Task<string> GetAccessTokenAsync()
        {
            if (_httpContext.User.Identity.IsAuthenticated)
            {
                var token = await _httpContext.GetTokenAsync("access_token");
                Console.WriteLine($"User Access Token: {token}");
                return token;
            }

            var requestUri = $"{_keycloakData.Host}/realms/{_keycloakData.Realm}/protocol/openid-connect/token";
            HttpContent content = new FormUrlEncodedContent(new[]
            {
        new KeyValuePair<string, string>("client_id", _keycloakData.ClientId),
        new KeyValuePair<string, string>("grant_type", "client_credentials"),
        new KeyValuePair<string, string>("client_secret", _keycloakData.ClientSecret)
    });

            var response = await _httpClient.PostAsync(requestUri, content);

            // Логирование токена
            Console.WriteLine($"Token Request to {requestUri}");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Failed to retrieve token: {response.StatusCode} - {response.ReasonPhrase}");
                throw new HttpRequestException(response.StatusCode.ToString());
            }

            var jsonString = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Token Response: {jsonString}");
            return JsonObject.Parse(jsonString)["access_token"].GetValue<string>();
        }


        public async Task SetAuthorizationHeaderAsync(HttpClient httpClient)
        {
            string token = await GetAccessTokenAsync();
            if (string.IsNullOrEmpty(token))
            {
                throw new Exception("Access token is null or empty");
            }
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

    }
}
