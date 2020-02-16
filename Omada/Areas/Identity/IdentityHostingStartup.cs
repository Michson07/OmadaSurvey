using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Omada.Areas.Identity.Data;
using Omada.Models;

[assembly: HostingStartup(typeof(Omada.Areas.Identity.IdentityHostingStartup))]
namespace Omada.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<OmadaContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("OmadaContextConnection")));

                services.AddDefaultIdentity<OmadaUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<OmadaContext>();
            });
        }
    }
}