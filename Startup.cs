using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace testowo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            GlobalConfiguration.Configuration.UseSqlServerStorage("CourseContext");
            app.UseHangfireDashboard();
            app.UseHangfireServer();
        }

    }
}