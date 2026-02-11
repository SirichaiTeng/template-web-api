using OriginalExample.Interfaces.IServices;
using OriginalExample.Models.Entity;
using System.Text.Json;

namespace OriginalExample.Services;

public class TokenService : ITokenService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    public TokenService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClient = httpClientFactory.CreateClient("TokenApi");
        _configuration = configuration;
    }
    public async Task<string> GetTokenAsync()
    {
        var clientId = _configuration["TokenApi:ClientId"];
        var clientSecret = _configuration["TokenApi:ClientSecret"];

        var request = new
        {
            clientId = clientId,
            clientSecret = clientSecret
        };
        // ใช้ PostAsJsonAsync ถ้าต้องการส่ง request ไปทาง Body
        var response = await _httpClient.PostAsJsonAsync("/api/token", request);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Token API error: {response.StatusCode}");
        }

        var result = await response.Content.ReadFromJsonAsync<TokenResponse>();

        return result?.AccessToken ?? throw new Exception("Token empty");
    }

    public async Task<string> GetTokenAsync2()
    {
        var ClientId = _configuration["TokenApi:ClientId"];
        var GrantType = _configuration["TokenApi:GrantType"];
        var ClientSecret = _configuration["TokenApi:ClientSecret"];
        var Scope = _configuration["TokenApi:Scope"];

        try
        {
            var form = new Dictionary<string, string>
            {
                ["client_id"] = ClientId,
                ["client_secret"] = ClientSecret,
                ["grant_type"] = GrantType,
                ["scope"] = Scope
            };

            using var request = new HttpRequestMessage(HttpMethod.Post, "api/microsofttoken")
            {
                Content = new FormUrlEncodedContent(form)
            };

            request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));    
            using HttpResponseMessage response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<TokenResponse>();
            return result?.AccessToken ?? string.Empty;
        }catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return string.Empty;
        }
    }

    public async Task<bool> ValidateTokenAsync(string token)
    {
        var clientId = _configuration["TokenApi:ClientId"];
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "api/token");
            request.Headers.Add("client_id",clientId);
            request.Headers.Add("access_token", token);
            request.Headers.Add("Accept","application/json");

            HttpResponseMessage response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                return response.IsSuccessStatusCode;
            }
            return true;
        }catch (Exception ex)
        {
            return false;
        }
    }
}
