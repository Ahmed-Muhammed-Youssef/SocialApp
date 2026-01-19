using Newtonsoft.Json;

namespace Infrastructure.Auth.External;

public class GoogleAuthService(IConfiguration configuration, IHttpClientFactory httpClientFactory) : IGoogleAuthService
{
    private readonly HttpClient _client = httpClientFactory.CreateClient("GoogleAuth");
    private readonly string _clientId = configuration["Authentication:Google:ClientId"]!;
    private readonly string _clientSecret = configuration["Authentication:Google:ClientSecret"]!;
    private readonly string _redirectUri = configuration["Authentication:Google:RedirectUri"]!;
    private readonly string _tokenEndpoint = configuration["Authentication:Google:TokenEndpoint"]!;

    public async Task<GoogleUserInfo> GetUserFromGoogleAsync(string code)
    {
        const string userInfoEndpoint = "userinfo/v2/me";
        var tokenResponse = await GetAccessToken(code);

        // Extract access token from the response
        var accessToken = tokenResponse["access_token"];

        // Use the access token to retrieve user information
        var userInfoResponse = await _client.GetAsync($"{userInfoEndpoint}?access_token={accessToken}");

        // Check for successful response
        if (!userInfoResponse.IsSuccessStatusCode)
        {
            throw new Exception("Failed to retrieve user information");
        }

        // Parse JSON response and extract relevant information
        var userInfoJson = await userInfoResponse.Content.ReadAsStringAsync();

        var userInfo = JsonConvert.DeserializeObject<Dictionary<string, string>>(userInfoJson);

        GoogleUserInfo googleUserInfo = new()
        {
            Name = userInfo!["name"],
            Email = userInfo["email"],
            VerifiedEmail = bool.Parse(userInfo["verified_email"]),
            PictureUrl = userInfo["picture"]
        };

        return googleUserInfo;
    }

    private async Task<Dictionary<string, string>> GetAccessToken(string code)
    {
        using var formContent = new FormUrlEncodedContent(
        [
            new KeyValuePair<string, string>("code", code),
            new KeyValuePair<string, string>("client_id", _clientId),
            new KeyValuePair<string, string>("client_secret", _clientSecret),
            new KeyValuePair<string, string>("grant_type", "authorization_code"),
            new KeyValuePair<string, string>("redirect_uri", _redirectUri),
        ]);

        var tokenResponse = await _client.PostAsync(_tokenEndpoint, formContent);

        if (!tokenResponse.IsSuccessStatusCode)
        {
            throw new Exception("Failed to exchange code for tokens");
        }

        var tokenResponseJson = await tokenResponse.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<Dictionary<string, string>>(tokenResponseJson) ?? [];
    }
}
