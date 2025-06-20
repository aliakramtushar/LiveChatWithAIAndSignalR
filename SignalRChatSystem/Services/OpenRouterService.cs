using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace SignalRChatSystem.Services
{
    public class OpenRouterService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "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"; // Replace with your key
        private readonly string dataModel = "mistralai/devstral-small:free";

        public OpenRouterService()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            _httpClient.DefaultRequestHeaders.Add("HTTP-Referer", "https://localhost:7183");
            _httpClient.DefaultRequestHeaders.Add("X-Title", "steveAiChatBot");
        }

        public async Task<string> GetAIResponse(string userPrompt)
        {

            var body = new
            {
                model = dataModel,
                messages = new[]
                {
                    new { role = "user", content = userPrompt }
                }
            };


            var response = await _httpClient.PostAsJsonAsync("https://openrouter.ai/api/v1/chat/completions", body);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine("OpenRouter Error: " + error);
                return "Sorry, something went wrong.";
            }

            var result = await response.Content.ReadAsStringAsync();
            dynamic json = JsonConvert.DeserializeObject(result);
            return json.choices[0].message.content.ToString();
        }
    }
}
