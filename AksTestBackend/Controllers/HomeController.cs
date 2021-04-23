using System.Diagnostics;
using AksTestBackend.Models;
using AksTestBackend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AksTestBackend.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
        private readonly NetworkInfoService _networkInfoSvc;

        public HomeController(ILogger<HomeController> logger, NetworkInfoService networkInfoSvc)
        {
            _logger = logger;
            _networkInfoSvc = networkInfoSvc;
        }

		public IActionResult Index()
        {
            return View(MessageController.AllMessages);
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
}
