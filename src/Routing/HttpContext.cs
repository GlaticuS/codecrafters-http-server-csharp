using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codecrafters_http_server.src.Routing
{
    public class HttpContext(string path, string method, Dictionary<string, string> headers)
    {
        public string Path { get; set; } = path;
        public string Method { get; set; } = method;
        public Dictionary<string, string> Headers { get; set; } = headers;
    }
}
