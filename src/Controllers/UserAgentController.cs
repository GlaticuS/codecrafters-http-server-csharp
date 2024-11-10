using codecrafters_http_server.src.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codecrafters_http_server.src.Controllers
{
    internal class UserAgentController
    {
        [Route("/user-agent")]
        public HttpResult GetUserAgent(HttpContext context)
        {
            return new HttpResult(context.Headers["User-Agent"], "200");
        }
    }
}
