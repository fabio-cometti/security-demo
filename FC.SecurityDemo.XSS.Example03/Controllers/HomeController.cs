using FC.SecurityDemo.XSS.Example03.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FC.SecurityDemo.XSS.Example03.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewData["message"] = "";
            ViewData["showAsRaw"] = false;
            ViewData["csp"] = false;
            ViewData["nonce"] = false;
            ViewData["nonce-value"] = "";
            return View();
        }

        [HttpPost]
        public IActionResult Index([FromForm]string message,[FromForm]string showAsRaw, [FromForm] string csp, [FromForm] string nonce)
        {
            ViewData["showAsRaw"] = !string.IsNullOrEmpty(showAsRaw);
            ViewData["csp"] = !string.IsNullOrEmpty(csp);
            ViewData["nonce"] = !string.IsNullOrEmpty(nonce);
            ViewData["message"] = message;

            if(!string.IsNullOrEmpty(csp))
            {
                //Enable CSP policy...
                var n = Guid.NewGuid().ToString("N");
                ViewData["nonce-value"] = n;
                Response.Headers.Add("Content-Security-Policy", $"script-src 'nonce-{n}'");
            }
            else
            {
                ViewData["nonce-value"] = "";
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
