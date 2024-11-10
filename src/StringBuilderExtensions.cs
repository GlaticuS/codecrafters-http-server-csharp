using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codecrafters_http_server.src
{
    public static class StringBuilderExtensions
    {
        public static StringBuilder AppendHttpLine(this StringBuilder builder)
        {
            return builder.Append("\r\n");
        }

        public static StringBuilder AppendHttpLine(this StringBuilder builder, string? value)
        {
            return builder.Append(value).Append("\r\n");
        }
    }
}
