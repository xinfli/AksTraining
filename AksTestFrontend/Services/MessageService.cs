using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AksTestFrontend.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace AksTestFrontend.Services
{
    public interface IMessageService
    {
        Task<ResultModel> SendMessage(MessageModel message);
    }

    public class MessageService : IMessageService
    {
        private readonly HttpClient _httpClient;
        private readonly AksTestFrontendOption _options;

        public MessageService(HttpClient httpClient, IOptions<AksTestFrontendOption> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }

        public async Task<ResultModel> SendMessage(MessageModel message)
        {
            var jsonContent = JsonConvert.SerializeObject(message);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var apiEndpoint = _options.BackendApiEndpoint;
            Console.WriteLine($"Forward message to {apiEndpoint}");

            var httpResponse = await _httpClient.PostAsync(apiEndpoint, httpContent);
            var responseContent = await httpResponse.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<ResultModel>(responseContent);
            return result;
        }
    }
}