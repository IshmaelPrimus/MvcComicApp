using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace MvcComic.Services;
//public class ComicVineService
//{
//    private readonly HttpClient _httpClient;
//    private readonly string _apiKey;

//    public ComicVineService(HttpClient httpClient, string apiKey)
//    {
//        _httpClient = httpClient;
//        _apiKey = "2194908e26505271c0a8b22937d61d9af0d9ac54";
//    }

//    public async Task<string?> GetIssueImageAsync(string issueName, int issueNumber)
//    {
//        var query = $"\"{issueName} ({issueNumber})\"";
//        var url = $"https://comicvine.gamespot.com/api/search/?api_key={_apiKey}&format=json&sort=name:asc&resources=issue&query={query}&filter=name:{issueName},issue_number:{issueNumber}";
//        var response = await _httpClient.GetAsync(url);

//        if (response.IsSuccessStatusCode)
//        {
//            var content = await response.Content.ReadAsStringAsync();
//            var json = JObject.Parse(content);
//            var results = json["results"]?.FirstOrDefault();
//            return results?["image"]?["original_url"]?.ToString();
//        }

//        return null;
//    }
//}
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
        try
        {
            // Construct the API request URL
            var requestUrl = $"https://comicvine.gamespot.com/api/issues/?api_key={_apiKey}&filter=name:{issueName},issue_number:{issueNumber}&format=json";

            // Send the request
            var response = await _httpClient.GetAsync(requestUrl);
            response.EnsureSuccessStatusCode();

            // Parse the response
            var content = await response.Content.ReadAsStringAsync();
            var jsonResponse = JsonDocument.Parse(content);

            // Extract the image URL from the response
            var imageUrl = jsonResponse.RootElement
                .GetProperty("results")
                .EnumerateArray()
                .FirstOrDefault()
                .GetProperty("image")
                .GetProperty("original_url")
                .GetString();

            return imageUrl;
        }
        catch (Exception ex)
        {
            // Log the exception details
            Console.WriteLine($"Exception: {ex.Message}");
            Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            return null;
        }
    }
}

