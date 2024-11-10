using codecrafters_http_server.src.HttpResults;
using codecrafters_http_server.src.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codecrafters_http_server.src.Controllers
{
    internal class FilesController : IController
    {
        public string Directory { get; }

        public FilesController(string path)
        {
            Directory = path;
        }

        [Route("/files/{filename}")]
        public HttpResult ProcessFile(HttpResponseContext context, string filename)
        {
            string fullPath = Path.Join(Directory, filename);

            if (context.RequestContext.Method == "GET")
            {
                if (File.Exists(fullPath))
                {
                    string content = File.ReadAllText(fullPath);
                    context.ContentType = "application/octet-stream";

                    return HttpResult.Ok(content);
                }

                return HttpResult.NotFound();
            }
            else if (context.RequestContext.Method == "POST")
            {
                File.WriteAllText(fullPath, context.Body);

                return HttpResult.Created();
            }

            return HttpResult.NotFound();

        }
    }
}
