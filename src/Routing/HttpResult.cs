using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codecrafters_http_server.src.Routing
{
    public class HttpResult(string message, string code)
    {
        public string message = message;
        public string code = code;
    }
}
