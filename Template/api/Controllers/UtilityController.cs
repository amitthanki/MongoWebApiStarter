﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MongoWebApiStarter.Api.Base;

namespace MongoWebApiStarter.Api.Controllers
{
    public class UtilityController : BaseController
    {
        private readonly IWebHostEnvironment env;

        public UtilityController(IWebHostEnvironment environment)
        {
            env = environment;
        }

        [AllowAnonymous]
        [HttpGet("show-log")] //todo: blacklist this route with nginx in production [https://docs.nginx.com/nginx/admin-guide/security-controls/configuring-http-basic-authentication/]
        public ActionResult ShowLog()
        {
            if (System.IO.File.Exists("output.log"))
            {
                var file = System.IO.Path.Combine(env.ContentRootPath, "output.log");
                return PhysicalFile(file, "text/plain");
            }

            return Ok("no log output yet...");
        }
    }
}
