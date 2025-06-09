using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace DoAnMonHocNT106
{
    public static class FileOnlineTranslator
    {
        private static readonly HttpClient client = new HttpClient();
        private static readonly string apiKey = "YOUR_GOOGLE_TRANSLATE_API_KEY"; // Thay bằng API key của bạn
        private static readonly string apiUrl = "https://translation.googleapis.com/language/translate/v2";

        public static async Task<string> Translate(string text, string targetLanguage)
        {
            if (string.IsNullOrEmpty(text)) return text;

            try
            {
                var requestBody = new
                {
                    q = text,
                    target = targetLanguage, // Ví dụ: "vi", "en", "zh", "ru"
                    format = "text"
                };

                var response = await client.PostAsJsonAsync($"{apiUrl}?key={apiKey}", requestBody);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(responseBody);
                return json["data"]["translations"][0]["translatedText"].ToString();
            }
            catch
            {
                return text; // Trả về text gốc nếu lỗi
            }
        }
    }
}