using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codecrafters_http_server.src.HttpResults
{
    internal interface IHttpResult
    {
        public int StatusCode { get; }
        public string Value { get; }
    }
}
