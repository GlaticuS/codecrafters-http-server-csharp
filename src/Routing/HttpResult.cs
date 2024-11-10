using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codecrafters_http_server.src.Routing
{
    public class HttpResult(string value, int code)
    {
        public int Code { get; private set; } = code;
        public string Value { get; private set; } = value;

        public static HttpResult Ok(string value)
        {
            return new HttpResult(value, 200);
        }
    }
}
