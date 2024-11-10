using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codecrafters_http_server.src.Routing
{
    public class HttpContext(string url, string method, Dictionary<string, string> headers)
    {
        public string url = url; 
        public string method = method;
        public Dictionary<string, string> headers = headers;
    }
}
