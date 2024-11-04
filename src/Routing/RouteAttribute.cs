using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codecrafters_http_server.src.Routing
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class RouteAttribute : Attribute
    {
        public string Template { get; }

        public RouteAttribute(string template)
        {
            Template = template;
        }
    }
}
