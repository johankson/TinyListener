using System;
using System.Threading.Tasks;
using Xunit;

namespace TinyListener.Client.Tests
{
    public class StaticTest
    {
        [Fact]
        public async Task SayTest()
        {
            await TinyListener.Say("debug", "Hello world!");
        }

        [Fact]
        public async Task ConfigureTest()
        {
            // Arrange
            var factory = new MyClientIdFactory();

            // Act
            TinyListener.Configure(factory);
            await TinyListener.Say("debug", "Hello world!");

            // Assert

        }
    }

    public class MyClientIdFactory : IClientIdFactory
    {
        public string Create()
        {
            return "debug-client-id-factory";
        }
    }
}
