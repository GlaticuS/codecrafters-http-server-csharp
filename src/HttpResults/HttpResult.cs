using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codecrafters_http_server.src.HttpResults
{
    public class HttpResult(string? value, int statusCode) : IHttpResult
    {
        public int StatusCode { get; private set; } = statusCode;
        public string? Value { get; private set; } = value;

        public static HttpResult Ok(string value)
        {
            return new HttpResult(value, 200);
        }

        internal static HttpResult NotFound()
        {
            return new HttpResult(null, 404);
        }
    }
}
