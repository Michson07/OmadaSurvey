using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Omada.Areas.Identity.Data;

namespace Omada.Models
{
    public class OmadaContext : IdentityDbContext<OmadaUser>
    {
        public OmadaContext(DbContextOptions<OmadaContext> options)
            : base(options)
        {
        }
        public OmadaContext() : base (){ } 

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
