using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codecrafters_http_server.src.Controllers
{
    internal interface IHttpResult<T> where T : class
    {
        public int StatusCode { get; }
        public T Value { get; }
    }
}
