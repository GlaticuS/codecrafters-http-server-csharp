using codecrafters_http_server.src.Controllers;
using codecrafters_http_server.src.HttpResults;
using codecrafters_http_server.src.Routing;
using System.IO.Compression;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace codecrafters_http_server.src
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string path = string.Empty;
            if (args.Length >= 2 && args[0] == "--directory")
                path = args[1];

            var router = new Router();
            router.Register(new EchoController());
            router.Register(new DefaultController());
            router.Register(new UserAgentController());
            router.Register(new FilesController(path));

            // You can use print statements as follows for debugging, they'll be visible when running tests.
            //Console.WriteLine("Logs from your program will appear here!");

            // Uncomment this block to pass the first stage
            TcpListener server = new TcpListener(IPAddress.Any, 4221);
            server.Start();
            while (true)
            {
                var tcpRequest = server.AcceptTcpClient(); // Wait for client.
                Thread thread = new Thread(() =>
                {
                    try
                    {
                        HandleConnection(router, tcpRequest);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                });
                thread.Start();
            }
        }

        private static void HandleConnection(Router router, TcpClient client)
        {
            var networkStream = client.GetStream();
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

            HashSet<string> encodings = [];
            if (headers.ContainsKey("Accept-Encoding"))
            {
                encodings = headers["Accept-Encoding"].Split(", ").ToHashSet();
            }

            string body = string.Empty;
            string? contentLengthHeader = headers.Keys.FirstOrDefault(h => h.ToLower() == "content-length");
            if (contentLengthHeader != null)
            {
                if (encodings.Contains("gzip"))
                {
                    var decompressor = new GZipStream(networkStream, CompressionMode.Decompress);
                    using var sr = new StreamReader(decompressor);
                    body = sr.ReadToEnd();
                }
                else
                {
                    int contentLength = int.Parse(headers[contentLengthHeader]);
                    char[] buffer = new char[contentLength];
                    reader.Read(buffer, 0, contentLength);
                    body = new string(buffer);
                }
            }

            HttpRequestContext requestContext = new HttpRequestContext(path, method, headers, body);
            HttpResponseContext responseContext = new HttpResponseContext(requestContext);
            HttpResult? result = router.HandleRequest(responseContext);

            if (result == null) // No handler - 404.
                responseContext.StatusCode = 404;
            else
            {
                responseContext.StatusCode = result.StatusCode;

                if (!string.IsNullOrEmpty(result.Value))
                {
                    if (encodings.Contains("gzip"))
                    {
                        byte[] bodyBuffer = Encoding.UTF8.GetBytes(result.Value);

                        MemoryStream compressedStream = new MemoryStream();
                        GZipStream compressor = new GZipStream(compressedStream, CompressionMode.Compress, true);
                        compressor.Write(bodyBuffer, 0, bodyBuffer.Length);
                        compressor.Flush();
                        compressor.Close();

                        responseContext.Body = compressedStream.ToArray();
                        responseContext.Headers["Content-Encoding"] = "gzip";
                    }
                    else
                    {
                        responseContext.Body = Encoding.UTF8.GetBytes(result.Value);
                    }

                    responseContext.Headers["Content-Length"] = responseContext.Body.Length.ToString();
                }
            }

            StringBuilder response = new StringBuilder();
            response.AppendHttpLine($"HTTP/1.1 {responseContext.Status}");
            foreach (KeyValuePair<string, string> pair in responseContext.Headers)
                if (!string.IsNullOrEmpty(pair.Value))
                    response.AppendHttpLine($"{pair.Key}: {pair.Value}");
            response.AppendHttpLine();
            byte[] responseBuffer = Encoding.UTF8.GetBytes(response.ToString());

            if (responseContext.Body != null && responseContext.Body.Length > 0)
                responseBuffer = [.. responseBuffer, .. responseContext.Body];

            networkStream.Write(responseBuffer);
            client.Close();
        }
    }
}
