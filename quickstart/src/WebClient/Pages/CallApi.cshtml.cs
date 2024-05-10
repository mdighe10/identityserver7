using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MyApp.Namespace
{

    public class CallApiModel : PageModel
    {
        public string Json = string.Empty;

        public async Task OnGet()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            //log the access token
            Console.WriteLine($"Access Token: {accessToken}");
            var response = await client.GetAsync("https://localhost:6001/identity");

            if (response.IsSuccessStatusCode)
            {
                var content = await client.GetStringAsync("https://localhost:6001/weather");

                var parsed = JsonDocument.Parse(content);

                Console.WriteLine($"Parsed: {parsed}");
                var formatted = JsonSerializer.Serialize(parsed, new JsonSerializerOptions { WriteIndented = true });

                Json = formatted;
            }
            else
            {
                Console.WriteLine($"API Request failed with status code: {response.StatusCode}");
                Json = response.StatusCode.ToString();
            }
        }
    }
}