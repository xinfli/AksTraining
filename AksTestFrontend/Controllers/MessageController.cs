using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AksTestFrontend.Models;
using AksTestFrontend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AksTestFrontend.Controllers
{
	[ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
	{
        public static List<MessageModel> AllMessages { get; private set; } = new List<MessageModel>();
        private readonly IMessageService _messageService;
        private readonly ILogger<MessageController> _logger;

        public MessageController(ILogger<MessageController> logger, IMessageService messageService)
        {
            _logger = logger;
            _messageService = messageService;
		}

        [HttpPost("[action]")]
        public async Task<ResultModel> SendMessage(MessageModel message)
        {
            AllMessages.Add(message);

            var myHostName = Dns.GetHostName();
            var myIpAddresses = await Dns.GetHostAddressesAsync(myHostName);

            var newIpRoutes = new List<string>(message.IpRoutes)
            {
                $"[AksTest] Backend ({myIpAddresses[0].MapToIPv4()})"
            };
            var newMessage = new MessageModel
            {
                Id = message.Id,
                Content = message.Content,
                IpRoutes = newIpRoutes,
                SendTime = message.SendTime
            };
            await _messageService.SendMessage(newMessage);

            return new ResultModel
            {
                Result = true,
                MessageCount = AllMessages.Count
            };
		}

        [HttpGet("[action]")]
        public List<MessageModel> GetAllMessages()
        {
            return AllMessages;
        }
	}
}
