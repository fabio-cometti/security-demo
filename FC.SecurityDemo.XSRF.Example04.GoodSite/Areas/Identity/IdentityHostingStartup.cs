using System;
using FC.SecurityDemo.XSRF.Example04.GoodSite.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(FC.SecurityDemo.XSRF.Example04.GoodSite.Areas.Identity.IdentityHostingStartup))]
namespace FC.SecurityDemo.XSRF.Example04.GoodSite.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}