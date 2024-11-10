using codecrafters_http_server.src.Controllers;
using codecrafters_http_server.src.HttpResults;
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
            var tcpRequest = server.AcceptTcpClient(); // Wait for client.

            var networkStream = tcpRequest.GetStream();
            using var reader = new StreamReader(networkStream, Encoding.UTF8);

            string[] firstLine = reader.ReadLine()!.Split(' ');
            string method = firstLine[0]; // TODO: Enum.Prase().
            string path = firstLine[1];
            string version = firstLine[2];

            Dictionary<string, string> headers = new Dictionary<string, string>();
            string line;
            while (!string.IsNullOrEmpty(line = reader.ReadLine()!))
            {
                string[] headerPair = line.Split([':'], 2);
                string header = headerPair[0];
                string value = headerPair[1].TrimStart(' ');
                headers[header] = value; // TODO: should be case-insensitive.
            }

            HttpContext context = new HttpContext(path, method, headers);
            HttpResult? response = router.HandleRequest(context);

            if (response?.Value != null)
            {
                networkStream.Write(Encoding.UTF8.GetBytes(
                    $"HTTP/1.1 200 OK\r\nContent-Type: text/plain\r\nContent-Length: {response.Value.Length}\r\n\r\n{response.Value}")
                );
            }
            else if (response?.Value == null && response?.StatusCode == 200)
            {
                networkStream.Write(Encoding.UTF8.GetBytes("HTTP/1.1 200 OK\r\n\r\n"));
            }
            else
            {
                networkStream.Write(Encoding.UTF8.GetBytes("HTTP/1.1 404 Not Found\r\n\r\n"));
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
