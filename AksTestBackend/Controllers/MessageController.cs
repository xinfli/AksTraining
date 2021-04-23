using System.Collections.Generic;
using AksTestBackend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AksTestBackend.Controllers
{
	[ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        public static List<MessageModel> AllMessages { get; private set; } = new List<MessageModel>();
        private readonly ILogger<MessageController> _logger;

		public MessageController(ILogger<MessageController> logger)
		{
			_logger = logger;
		}

        [HttpPost("[action]")]
        public ResultModel SendMessage(MessageModel message)
		{
            AllMessages.Add(message);
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
