using System.ComponentModel;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace AI.NET.Network.AI
{
    /// <summary>
    /// Mem0 API network. Can be used as data context for the UI
    /// </summary>
    internal class Mem0 : INotifyPropertyChanged
    {
        private string? _baseUrl;
        public string BaseUrl
        {
            get => _baseUrl ?? "";
            set { _baseUrl = value; OnPropertyChanged(nameof(BaseUrl)); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async void AddMemoryAsync(string message, string user_id)
        {
            RequestBody requestBody = new RequestBody()
            { message = message, user_id = user_id };
            //POST request
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(BaseUrl + "/memory/add"),
                Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json")
            };
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
        }
        /// <summary>
        /// Search memory using mem0 API
        /// </summary>
        /// <param name="message">The search message</param>
        /// <param name="user_id">The user's id</param>
        /// <returns>The API's respond representing related memories</returns>
        public async Task<string> SearchMemoryAsync(string message, string user_id)
        {
            RequestBody requestBody = new RequestBody()
            { message = message, user_id = user_id };
            //POST request
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(BaseUrl + "/memory/search"),
                Content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json")
            };
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
        /// <summary>
        /// Represents the data to send to the API
        /// </summary>
        public class RequestBody
        {
#pragma warning disable IDE1006 // Naming Styles
            public string? message { get; set; }
            public string? user_id { get; set; }
#pragma warning restore IDE1006 // Naming Styles
        }

    }
}
