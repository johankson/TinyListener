using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace TinyListener.Client
{
    public class TinyListener
    {
        private static TinyListener _instance = new TinyListener();

        private readonly HttpClient _client;
        private string _clientId = Guid.NewGuid().ToString().Substring(0, 8); 
        private IClientIdFactory _clientIdFactory = null;

        public TinyListener(HttpClient httpClient = null)
        {
            if (httpClient == null)
            {
                _client = new HttpClient();
                _client.BaseAddress = new Uri("https://tinylistener.azurewebsites.net");
			}
            else
            {
                if (String.IsNullOrWhiteSpace(httpClient.BaseAddress.ToString()))
                {
                    throw new ArgumentException("You must set BaseAddress on the HttpClient you passed in", nameof(httpClient)); 
                }
                _client = httpClient;
            }
        }

        public async Task Send(string channel, string data)
        {
            if (_clientIdFactory != null)
            {
                _clientId = _clientIdFactory.Create();
            }

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

        public static async Task SayAsync(string channel, string data)
        {
            await _instance.Send(channel, data);
        }

        public static void Say(string channel, string data)
        {
            SayAsync(channel, data).GetAwaiter().GetResult();
        }

        public static void Configure(IClientIdFactory factory = null)
        {
            if (factory != null)
            {
				_instance._clientIdFactory = factory;
            }
        }
    }
}
