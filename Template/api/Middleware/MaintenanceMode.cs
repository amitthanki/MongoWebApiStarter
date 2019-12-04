﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace MongoWebApiStarter.Api.Middleware
{
    public class MaintenanceMode
    {
        private bool maintenanceMode = false;
        private readonly string content;
        private readonly RequestDelegate next;

        public MaintenanceMode(RequestDelegate next, IWebHostEnvironment environment)
        {
            this.next = next;
            content = File.ReadAllText(Path.Combine(environment.WebRootPath, "maintenance.htm"));
        }

        public async Task Invoke(HttpContext context)
        {
            switch (context.Request.Path)
            {
                case "/turn-site-off": //todo: blacklist this route with nginx in production [https://docs.nginx.com/nginx/admin-guide/security-controls/configuring-http-basic-authentication/]
                    maintenanceMode = true;
                    break; ;
                case "/turn-site-on": //todo: blacklist this route with nginx in production [https://docs.nginx.com/nginx/admin-guide/security-controls/configuring-http-basic-authentication/]
                    maintenanceMode = false;
                    context.Response.Clear();
                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                    context.Response.ContentType = "text/html";
                    await context.Response.WriteAsync("<h1>maintenance off...</h1>");
                    return;
            }

            if (maintenanceMode)
            {
                context.Response.Clear();
                context.Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
                context.Response.ContentType = "text/html";
                await context.Response.WriteAsync(content);
                return;
            }

            await next.Invoke(context);
        }
    }

    public static class MaintenanceModeExtensions
    {
        public static IApplicationBuilder UseMaintenanceModeMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<MaintenanceMode>();
        }
    }
}
