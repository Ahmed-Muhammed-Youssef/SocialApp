﻿using Application.Authentication.Google;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Infrastructure.ExternalServices.Google
{
    public class GoogleAuthService(IConfiguration configuration) : IGoogleAuthService
    {
        private readonly string _clientId = configuration["Authentication:Google:ClientId"];
        private readonly string _clientSecret = configuration["Authentication:Google:ClientSecret"];
        private readonly string _redirectUri = configuration["Authentication:Google:RedirectUri"];
        private readonly string _googleAuthorizationEndpoint = configuration["Authentication:Google:GoogleAuthorizationEndpoint"];
        private readonly string _scope = configuration["Authentication:Google:Scope"];
        private readonly string _tokenEndpoint = configuration["Authentication:Google:TokenEndpoint"];
        public string BuildGoogleSignInUrl()
        {
            UriBuilder builder = new(_googleAuthorizationEndpoint);
            Dictionary<string, string> queryParams = [];
            queryParams.Add("client_id", _clientId);
            queryParams.Add("redirect_uri", _redirectUri);
            queryParams.Add("response_type", "code");
            queryParams.Add("scope", _scope);
            queryParams.Add("access_type", "offline"); // Request refresh token for extended access

            builder.Query = string.Join("&", queryParams.Select(p => $"{p.Key}={Uri.EscapeDataString(p.Value)}"));
            return builder.ToString();
        }

        public async Task<AppUser> GetUserFromGoogleAsync(string code)
        {
            using var client = new HttpClient();
            const string userInfoEndpoint = "https://www.googleapis.com/userinfo/v2/me";
            var tokenResponse = await GetAccessToken(client, code);

            // Extract access token from the response
            var accessToken = tokenResponse["access_token"];

            // Use the access token to retrieve user information
            var userInfoResponse = await client.GetAsync($"{userInfoEndpoint}?access_token={accessToken}");

            // Check for successful response
            if (!userInfoResponse.IsSuccessStatusCode)
            {
                throw new Exception("Failed to retrieve user information");
            }

            // Parse JSON response and extract relevant information
            var userInfoJson = await userInfoResponse.Content.ReadAsStringAsync();
            var userInfo = JsonConvert.DeserializeObject<Dictionary<string, string>>(userInfoJson);

            // Create new user out of the data returned
            AppUser newUser = new()
            {
                FirstName = userInfo["name"],
                Email = userInfo["email"],
                EmailConfirmed = bool.Parse(userInfo["verified_email"])
            };

            return newUser;
        }

        private async Task<Dictionary<string, string>> GetAccessToken(HttpClient client, string code)
        {
            var formContent = new FormUrlEncodedContent(
            [
                new KeyValuePair<string, string>("code", code),
                new KeyValuePair<string, string>("client_id", _clientId),
                new KeyValuePair<string, string>("client_secret", _clientSecret),
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("redirect_uri", _redirectUri),
            ]);

            var tokenResponse = await client.PostAsync(_tokenEndpoint, formContent);

            if (!tokenResponse.IsSuccessStatusCode)
            {
                throw new Exception("Failed to exchange code for tokens");
            }

            var tokenResponseJson = await tokenResponse.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(tokenResponseJson);
        }
    }
}
