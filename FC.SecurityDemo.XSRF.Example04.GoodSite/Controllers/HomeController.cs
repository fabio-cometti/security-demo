using FC.SecurityDemo.XSRF.Example04.GoodSite.Data;
using FC.SecurityDemo.XSRF.Example04.GoodSite.Models;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FC.SecurityDemo.XSRF.Example04.GoodSite.Controllers
{
    public class HomeController : Controller
    {        
        private readonly ApplicationDbContext db;

        public HomeController(ApplicationDbContext db)
        {            
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
            //SetXSRF();
            return View();
        }

        [Authorize]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Transfer([FromForm]string username, [FromForm]decimal amount)
        {
            //ValidateXSRF();
            //SetXSRF();
            var source = await db.Accounts.FirstAsync(acc => acc.UserName == User.Identity.Name);
            var target = await db.Accounts.FirstAsync(acc => acc.UserName == username);
            source.Credit -= amount;
            target.Credit += amount;
            await db.SaveChangesAsync();
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private void SetXSRF()
        {
            var xsrf = Guid.NewGuid().ToString("N");
            ViewData["xsrf"] = xsrf;
            Response.Cookies.Append("xsrf", xsrf);
        }

        private void ValidateXSRF()
        {
            try
            {
                string form = Request.Form["xsrf"];
                string cookie = Request.Cookies["xsrf"];
                
                if(string.IsNullOrEmpty(form) || string.IsNullOrEmpty(cookie) || form != cookie)
                {
                    throw new AntiforgeryValidationException("Form token and cookie token are not the same");
                }
            }
            catch (Exception)
            {
                throw new AntiforgeryValidationException("Cannot validate your request!!!");
            }
            
        }
    }
}
