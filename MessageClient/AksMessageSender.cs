using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MessageClient
{
    public class AksMessageSender
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly MessageClientOptions _options;

        public AksMessageSender(IHttpClientFactory httpClientFactory, IOptions<MessageClientOptions> options)
        {
            _httpClientFactory = httpClientFactory;
            _options = options.Value;
        }

        public async Task<int> SendMessage()
        {
            var apiEndpoint = _options.ApiEndpoint;

            var messageId = Guid.NewGuid();
            var message = new
            {
                Id = messageId,
                SendTime = DateTime.UtcNow,
                Content = $"Message {messageId}",
                IpRoutes = new[] {"[AksTest] ConsoleClient"}
            };

            Console.WriteLine($"Send message with id \"{messageId}\" to \"{apiEndpoint}\"");
            var jsonContent = JsonConvert.SerializeObject(message);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Post, apiEndpoint)
            {
                Content = httpContent
            };

            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.SendAsync(request);

            //Console.WriteLine($"Response:{Environment.NewLine}{response}");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Failed to send message to \"{apiEndpoint}\"");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response content: {responseContent}");

            var result = JObject.Parse(responseContent);

            return result.GetValue("MessageCount", StringComparison.InvariantCultureIgnoreCase).Value<int>();
        }
    }
}
