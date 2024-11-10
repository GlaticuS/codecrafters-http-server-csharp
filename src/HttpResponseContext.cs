using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace codecrafters_http_server.src
{
    public class HttpResponseContext
    {
        public HttpRequestContext RequestContext { get; private set; }
        public int StatusCode { get; set; } = 200;
        public string Status { get { return $"{StatusCode} {ReasonPhrases.GetReasonPhrase(StatusCode)}"; } }
        public Dictionary<string, string> Headers { get; private set; } = [];
        public string ContentType
        {
            get
            {
                Headers.TryGetValue("Content-Type", out string? value);
                return value ?? string.Empty;
            }
            set
            {
                Headers["Content-Type"] = value;
            }
        }
        public string Body { get; set; } = string.Empty;

        public HttpResponseContext(HttpRequestContext request)
        {
            RequestContext = request;
            ContentType = "text/plain";
        }
    }
}
