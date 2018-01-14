using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace TinyListener.Client
{
    public class TinyListener
    {
        private readonly HttpClient _client;

        public TinyListener()
        {
            // TODO Add injection of HttpClient or a factory?
            _client = new HttpClient();
            _client.BaseAddress = new Uri("https://tinylistener.azurewebsites.net");
        }

        public async Task Send(string channel, string data)
        {
            // TODO Json serialization
            var json = "{\"data\":\"" + data + "\"}";

            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            var result = await _client.PostAsync($"/api/listener/{channel}", content);
        }
    }
}
