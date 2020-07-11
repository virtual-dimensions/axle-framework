﻿using Axle.Configuration;
using Axle.Modularity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace Axle.Web.AspNetCore.StaticFiles
{
    [Module]
    [Utilizes(typeof(DefaultFilesModule))]
    public sealed class StaticFilesModule : IApplicationConfigurer
    {
        public StaticFilesModule(IConfiguration configuration)
        {

        }

        void IApplicationConfigurer.Configure(
            Microsoft.AspNetCore.Builder.IApplicationBuilder app, 
            IHostingEnvironment env)
        {
            app.UseStaticFiles();
        }
    }
}