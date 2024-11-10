using codecrafters_http_server.src.Controllers;
using codecrafters_http_server.src.Routing;
using Microsoft.VisualBasic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using static System.Net.WebRequestMethods;

namespace codecrafters_http_server.src
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var router = new Router();
            router.Register(typeof(EchoController));
            router.Register(typeof(DefaultController));
            router.Register(typeof(UserAgentController));

            // You can use print statements as follows for debugging, they'll be visible when running tests.
            //Console.WriteLine("Logs from your program will appear here!");

            // Uncomment this block to pass the first stage
            TcpListener server = new TcpListener(IPAddress.Any, 4221);
            server.Start();
            var sock = server.AcceptSocket(); // wait for client

            var buffer = new byte[1024];
            sock.Receive(buffer);

            var bufferStringify = Encoding.UTF8.GetString(buffer);
            string URL = bufferStringify.Split(" ")[1];

            //stringData:= string(data[:length])
            //splittedMessage:= strings.Split(stringData, "\r\n\r\n")
            //splittedData:= strings.SplitN(splittedMessage[0], " ", 3)
            //splitHeaders:= strings.Split(strings.Join(splittedMessage, " "), "\n")[1:]
            //// Headers parsing
            //headersMap:= make(map[string]string)
            //for i := 0; i < len(splitHeaders); i++ {
            //        keyValue:= strings.Split(splitHeaders[i], ": ")
            //    headersMap[keyValue[0]] = strings.TrimSpace(keyValue[1])
            //}
            //GET
            /// user - agent
            //HTTP / 1.1
            //\r\n

            //// Headers
            //Host: localhost: 4221\r\n
            //User - Agent: foobar / 1.2.3\r\n  // Read this value
            //Accept: */*\r\n
            //\r\n

            string headersString = bufferStringify.Split("\r\n\r\n")[1];
            string[] headersStringArray = headersString.Split("\r\n");

            Dictionary<string, string> headers = new Dictionary<string, string>();

            foreach (var item in headersStringArray)
            {
                string[] itemSplit = item.Split(":");
                if (itemSplit.Length == 2)
                {
                    headers.Add(itemSplit[0], itemSplit[1].Trim());
                }
            }


            HttpContext context = new HttpContext(URL, "GET", headers);
            HttpResult? response = router.HandleRequest(context);

            if (response?.message != null)
            {
                sock.Send(Encoding.UTF8.GetBytes(
                    $"HTTP/1.1 200 OK\r\nContent-Type: text/plain\r\nContent-Length: {response.message.Length}\r\n\r\n{response.message}")
                );
            }
            else if (response?.message == null && response?.code == "200")
            {
                sock.Send(Encoding.UTF8.GetBytes("HTTP/1.1 200 OK\r\n\r\n"));
            }
            else
            {
                sock.Send(Encoding.UTF8.GetBytes("HTTP/1.1 404 Not Found\r\n\r\n"));
            }

            //string[] splittedURL = URL.Split('/', StringSplitOptions.RemoveEmptyEntries);


            //string? path = splittedURL.Length > 0 ? splittedURL[0] : null;
            //string parameter = "";
            //if (splittedURL.Length > 1)
            //{
            //    parameter = splittedURL[1];
            //}

            //if (path is null)
            //{
            //    sock.Send(Encoding.UTF8.GetBytes("HTTP/1.1 200 OK\r\n\r\n"));

            //}
            //else if (path == "echo")
            //{
            //    sock.Send(Encoding.UTF8.GetBytes(
            //        $"HTTP/1.1 200 OK\r\nContent-Type: text/plain\r\nContent-Length: {parameter.Length}\r\n\r\n{parameter}")
            //    );
            //}
            //else
            //{
            //    sock.Send(Encoding.UTF8.GetBytes("HTTP/1.1 404 Not Found\r\n\r\n"));
            //}
        }
    }
}
