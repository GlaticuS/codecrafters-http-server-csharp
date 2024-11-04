﻿using codecrafters_http_server.src.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codecrafters_http_server.src.Controllers
{
    internal class EchoController
    {
        [Route("/echo/{message}")]
        public string GetEcho(string message)
        {
            return message;
        }
    }
}
