using FC.SecurityDemo.XSRF.Example04.GoodSite.Data;
using FC.SecurityDemo.XSRF.Example04.GoodSite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FC.SecurityDemo.XSRF.Example04.GoodSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            this.db = db;            
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult Transfer()
        {
            //setXSRF();
            return View();
        }

        [Authorize]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Transfer([FromForm]string username, [FromForm]decimal amount)
        {
            //validateXSRF();
            //setXSRF();
            var source = await db.Accounts.FirstAsync(acc => acc.UserName == User.Identity.Name);
            var target = await db.Accounts.FirstAsync(acc => acc.UserName == username);
            source.Credit = source.Credit - amount;
            target.Credit = target.Credit + amount;
            await db.SaveChangesAsync();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private void setXSRF()
        {
            var xsrf = Guid.NewGuid().ToString("N");
            ViewData["xsrf"] = xsrf;
            Response.Cookies.Append("xsrf", xsrf);
        }

        private void validateXSRF()
        {
            try
            {
                string form = Request.Form["xsrf"];
                string cookie = Request.Cookies["xsrf"];
                
                if(string.IsNullOrEmpty(form) || string.IsNullOrEmpty(cookie) || form != cookie)
                {
                    throw new Exception("Form token and cookie token are not the same");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Cannot validate your request!!!");
            }
            
        }
    }
}
