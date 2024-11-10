using codecrafters_http_server.src.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codecrafters_http_server.src
{
    internal class DefaultController
    {
        [Route("/")]
        public HttpResult GetDefault(HttpContext context)
        {
            return new HttpResult("", "200");
        }
    }
}
