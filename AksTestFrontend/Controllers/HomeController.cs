using System.Collections.Generic;
using System.Diagnostics;
using AksTestFrontend.Models;
using AksTestFrontend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AksTestFrontend.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
        private readonly AksTestFrontendOption _options;
        private readonly NetworkInfoService _networkInfoSvc;

        public HomeController(ILogger<HomeController> logger, IOptions<AksTestFrontendOption> options, NetworkInfoService networkInfoSvc)
        {
            _logger = logger;
            _options = options.Value;
            _networkInfoSvc = networkInfoSvc;
        }

		public IActionResult Index()
        {
            return View(new Messages
            {
                ForwardTarget = _options.BackendApiEndpoint,
                AllMessages = MessageController.AllMessages
            });
        }

		public IActionResult NetworkInfo()
		{
			return View(_networkInfoSvc.GetNetworkInfo());
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
		}
	}

    public class Messages
    {
        public string ForwardTarget { get; set; }
        public IEnumerable<MessageModel> AllMessages { get; set; }
    }
}
