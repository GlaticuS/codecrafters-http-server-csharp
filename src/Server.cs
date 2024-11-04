using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;

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

string[] splittedURL = URL.Split('/', StringSplitOptions.RemoveEmptyEntries);

string path = splittedURL[0];
string parameter = null;
if (splittedURL.Length > 1)
    parameter = splittedURL[1];


if (path.Length == 0)
{
    sock.Send(System.Text.Encoding.UTF8.GetBytes("HTTP/1.1 200 OK\r\n\r\n"));

}
else if (path == "echo")
{

}
else
{
    sock.Send(System.Text.Encoding.UTF8.GetBytes("HTTP/1.1 404 Not Found\r\n\r\n"));
}



