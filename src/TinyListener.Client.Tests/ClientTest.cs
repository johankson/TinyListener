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

        [Fact]
        public async Task MoreSendTest()
        {
            var client = new TinyListener();
            await client.Send("test-channel", "Gulliga ankor!"); 
        }

        [Fact]
        public async Task SendFileTest()
        {
            var client = new TinyListener("");
            var bytes = new byte[50000];
            await client.SendFile("test-channel", bytes, "MyFile.bin");
        }
    }
}
