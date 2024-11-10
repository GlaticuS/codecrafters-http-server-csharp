using codecrafters_http_server.src.Controllers;
using codecrafters_http_server.src.HttpResults;
using codecrafters_http_server.src.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codecrafters_http_server.src
{
    internal class DefaultController : IController
    {
        [Route("/")]
        public HttpResult GetDefault(HttpResponseContext context)
        {
            context.ContentType = "";
            return HttpResult.Ok("");
        }
    }
}
