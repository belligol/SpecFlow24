using Newtonsoft.Json;
using NUnitTestProject_UNI.Core.Models;
using System.Globalization;

namespace NunitTests.Core.Utils
{
    public class HttpClientHelper
    {
        private readonly HttpClient _httpClient;

        public HttpClientHelper()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://gorest.co.in/public/v2/");
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer ff0cff9050bfee5de9386da126e3f76128cfe323d049505c46612fa55f8c8a66");
        }

        public async Task<HttpResponseMessage> GET(string endpoint)
        {
            return await _httpClient.GetAsync(endpoint);
        }

        public async Task<HttpResponseMessage> POST(string endpoint, StringContent body)
        {
            return await _httpClient.PostAsync(endpoint, body);
        }

        public async Task<HttpResponseMessage> DELETE(string endpoint)
        {
            return await _httpClient.DeleteAsync(endpoint);
        }

        public async Task<HttpResponseMessage> PATCH(string endpoint, StringContent body)
        {
            return await _httpClient.PatchAsync(endpoint, body);
        }

        internal async Task<List<UserResponse>> CreateNewUserAsync(string name)
        {
            var userBody = CreateUserRequestBody(name);
            var responseMessage = await _httpClient.PostAsync("users", userBody);
            var jsonContent = await responseMessage.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<UserResponse>>(jsonContent);
        }

        public StringContent CreateUserRequestBody(string name)
        {
            var user = new UserRequest
            {
                Name = name,
                Email = GenerateEmail(name),
                Gender = "male",
                Status = "active"
            };

            var userBody = JsonConvert.SerializeObject(user);

            return new StringContent(userBody, System.Text.Encoding.UTF8, "application/json");
        }

        private string GenerateEmail(string name)
        {
            return $"{name}@domain.com";
        }
    }
}
