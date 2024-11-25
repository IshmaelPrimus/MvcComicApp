using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

public class ComicVineService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public ComicVineService(HttpClient httpClient, string apiKey)
    {
        _httpClient = httpClient;
        _apiKey = "2194908e26505271c0a8b22937d61d9af0d9ac54";
    }

    public async Task<string?> GetIssueImageAsync(string issueName, int issueNumber)
    {
        var url = $"https://comicvine.gamespot.com/api/search/?api_key={_apiKey}&format=json&sort=name:asc&resources=issue&query=\"{issueName}\"&filter=name:{issueName},issue_number:{issueNumber}";
        var response = await _httpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(content);
            var results = json["results"]?.FirstOrDefault();
            return results?["image"]?["original_url"]?.ToString();
        }

        return null;
    }
}

