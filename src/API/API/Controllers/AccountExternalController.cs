using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using API.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using API.Application.Interfaces.Services;
using System.Linq;
using API.Filters;
using System;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net.Http;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(LogUserActivity))]
    public class AccountExternalController(IConfiguration configuration, UserManager<AppUser> userManager, ITokenService tokenService) : ControllerBase
    {
        private readonly string _clientId = configuration["Authentication:Google:ClientId"];
        private readonly string _clientSecret = configuration["Authentication:Google:ClientSecret"];
        private readonly string _redirectUri = configuration["Authentication:Google:RedirectUri"];

        // GET: api/AccountExternal/login-google
        [HttpGet("login-google")]
        public IActionResult GoogleSignIn()
        {
            var url = BuildGoogleSignInUrl(_clientId, _redirectUri);
            return Redirect(url);
        }

        // GET: api/AccountExternal/callback-google
        [HttpGet("callback-google")]
        public async Task<IActionResult> GoogleCallback(string code)
        {
            // Validate the code and state if needed for security
            var userInfo = await GetUserInfoFromGoogleAsync(code);

            return Ok(userInfo); // Or redirect to another page
        }
        private async Task<Dictionary<string, string>> GetUserInfoFromGoogleAsync(string code)
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

            return userInfo;
        }

        private async Task<Dictionary<string, string>> GetAccessToken(HttpClient client, string code)
        {
            const string tokenEndpoint = "https://oauth2.googleapis.com/token";
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("code", code),
                new KeyValuePair<string, string>("client_id", _clientId),
                new KeyValuePair<string, string>("client_secret", _clientSecret),
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("redirect_uri", _redirectUri),
            });

            var tokenResponse = await client.PostAsync(tokenEndpoint, formContent);

            if (!tokenResponse.IsSuccessStatusCode)
            {
                throw new Exception("Failed to exchange code for tokens");
            }

            var tokenResponseJson = await tokenResponse.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(tokenResponseJson);
        }

        private static string BuildGoogleSignInUrl(string clientId, string redirectUri)
        {
            const string googleAuthorizationEndpoint = "https://accounts.google.com/o/oauth2/v2/auth";
            const string scope = "profile email"; // Adjust the scope as needed

            UriBuilder builder = new(googleAuthorizationEndpoint);

            Dictionary<string, string> queryParams = [];
            queryParams.Add("client_id", clientId);
            queryParams.Add("redirect_uri", redirectUri);
            queryParams.Add("response_type", "code");
            queryParams.Add("scope", scope);
            queryParams.Add("access_type", "offline"); // Request refresh token for extended access

            builder.Query = string.Join("&", queryParams.Select(p => $"{p.Key}={Uri.EscapeDataString(p.Value)}"));
            return builder.ToString();
        }
    }
}
