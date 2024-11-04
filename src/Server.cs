using codecrafters_http_server.src.Controllers;
using codecrafters_http_server.src.Router;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;

namespace codecrafters_http_server
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var router = new Router();
            router.Register(typeof(EchoController));

            // You can use print statements as follows for debugging, they'll be visible when running tests.
            Console.WriteLine("Logs from your program will appear here!");

            // Uncomment this block to pass the first stage
            TcpListener server = new TcpListener(IPAddress.Any, 4221);
            server.Start();
            var sock = server.AcceptSocket(); // wait for client

            var buffer = new byte[1024];
            sock.Receive(buffer);

            var bufferStringify = Encoding.UTF8.GetString(buffer);
            string URL = bufferStringify.Split(" ")[1];

            string? response = router.HandleRequest(URL);

            if (response != null)
            {
                sock.Send(Encoding.UTF8.GetBytes(
                    $"HTTP/1.1 200 OK\r\nContent-Type: text/plain\r\nContent-Length: {response.Length}\r\n\r\n{response}")
                );
            }
            else
            {
                sock.Send(Encoding.UTF8.GetBytes("HTTP/1.1 404 Not Found\r\n\r\n"));
            }

            string[] splittedURL = URL.Split('/', StringSplitOptions.RemoveEmptyEntries);


            string? path = splittedURL.Length > 0 ? splittedURL[0] : null;
            string parameter = "";
            if (splittedURL.Length > 1)
            {
                parameter = splittedURL[1];
            }

            if (path is null)
            {
                sock.Send(Encoding.UTF8.GetBytes("HTTP/1.1 200 OK\r\n\r\n"));

            }
            else if (path == "echo")
            {
                sock.Send(Encoding.UTF8.GetBytes(
                    $"HTTP/1.1 200 OK\r\nContent-Type: text/plain\r\nContent-Length: {parameter.Length}\r\n\r\n{parameter}")
                );
            }
            else
            {
                sock.Send(Encoding.UTF8.GetBytes("HTTP/1.1 404 Not Found\r\n\r\n"));
            }
        }
    }
}
