﻿using codecrafters_http_server.src.Controllers;
using codecrafters_http_server.src.HttpResults;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace codecrafters_http_server.src.Routing
{
    public class Router
    {
        private readonly Dictionary<string, (IController Instance, MethodInfo Method, string Template)> _routes = new();

        public void Register(IController controller)
        {
            var methods = controller.GetType().GetMethods().Where(m => m.GetCustomAttribute<RouteAttribute>() != null);

            foreach (var method in methods)
                RegisterEndpointMethod(controller, method);
        }

        private void RegisterEndpointMethod(IController instance, MethodInfo method)
        {
            var routeAttribute = method.GetCustomAttribute<RouteAttribute>();
            if (routeAttribute != null)
                _routes[routeAttribute.Template] = (instance, method, routeAttribute.Template);
        }

        public HttpResult? HandleRequest(HttpResponseContext context)
        {
            var matchedRoute = _routes.FirstOrDefault(route => UrlMatchesTemplate(context.RequestContext.Path, route.Key));

            if (matchedRoute.Value.Method != null)
            {
                var parameters = ExtractParametersFromUrl(matchedRoute.Value.Method, context.RequestContext.Path, matchedRoute.Value.Template);

                var instance = matchedRoute.Value.Instance;
                return matchedRoute.Value.Method.Invoke(instance, [context, .. parameters]) as HttpResult;
            }

            return null;
        }

        private bool UrlMatchesTemplate(string url, string template)
        {
            // This is a simplistic check; you may want to use regex or another library for better matching.
            var urlParts = url.Split('/');
            var templateParts = template.Split('/');

            return urlParts.Length == templateParts.Length &&
                   urlParts.Zip(templateParts, (u, t) => u == t || (t.StartsWith("{") && t.EndsWith("}"))).All(match => match);
        }

        private List<object>? ExtractParametersFromUrl(MethodInfo method, string url, string template)
        {
            var parameters = new List<object>();
            var urlParts = url.Split('/');
            var templateParts = template.Split('/');

            for (int i = 0; i < urlParts.Length; i++)
            {
                if (!templateParts[i].StartsWith("{") || !templateParts[i].EndsWith("}"))
                    continue;

                string paramName = templateParts[i].Trim('{', '}');

                Type? type = method.GetParameters().FirstOrDefault(p => p.Name == paramName)?.ParameterType;
                if (type == null)
                    return null;
                Type? nullableInnerType = Nullable.GetUnderlyingType(type);
                Type convertTo = nullableInnerType ?? type;
                TypeConverter converter = TypeDescriptor.GetConverter(type);
                object value = Convert.ChangeType(urlParts[i], convertTo);

                parameters.Add(value);
            }

            return parameters;
        }
    }
}
