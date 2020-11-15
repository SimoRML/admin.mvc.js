﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FAIS.Controllers
{
    public class PingController : ApiController
    {

        [HttpPost]
        [Route("api/ping/{d}")]
        public IHttpActionResult Ping(string d)
        {
            return Ok(DateTime.Now.ToLongTimeString() + '\t'+ d);
        }
    }
}
