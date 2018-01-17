using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace TinyListener.Client
{
    public class TinyListener
    {
        private readonly HttpClient _client;
        private readonly string _clientId = Guid.NewGuid().ToString().Substring(0, 8); // TODO Add a provider interface instead to inject the client id

        public TinyListener()
        {
            // TODO Add injection of HttpClient or a factory?
            _client = new HttpClient();
            _client.BaseAddress = new Uri("https://tinylistener.azurewebsites.net");
        }

        public async Task Send(string channel, string data)
        {
            var obj = new
            {
                data,
                clientid = _clientId
            };

            var json = JsonSerializer.Serialize(obj);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var result = await _client.PostAsync($"/api/listener/{channel}", content);

            result.EnsureSuccessStatusCode();
        }
    }
}
