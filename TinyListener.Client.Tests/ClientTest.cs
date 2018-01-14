using System;
using System.Threading.Tasks;
using Xunit;

namespace TinyListener.Client.Tests
{
    public class ClientTest
    {
        [Fact]
        public async Task SimpleSendTest()
        {
            // 
            var client = new TinyListener();
            await client.Send("test-channel", "Ankor är söta");
        }
    }
}
