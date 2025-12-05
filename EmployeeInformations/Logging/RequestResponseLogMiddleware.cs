using Microsoft.IO;

namespace EmployeeInformations
{
    public class RequestResponseLogMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILog _logger;
        private NLog.Logger logger;
        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;
        private readonly IConfiguration _config;

        public RequestResponseLogMiddleware(RequestDelegate next, IConfiguration config,
                                                ILog logger)
        {
            _next = next;
            _logger = logger;
            _config = config;
            this.logger = NLog.LogManager.GetCurrentClassLogger();
            _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
        }

        public async Task Invoke(HttpContext context)
        {
            await LogRequest(context);
            await LogResponse(context);
        }

        private async Task LogRequest(HttpContext context)
        {
            context.Request.EnableBuffering();

            await using var requestStream = _recyclableMemoryStreamManager.GetStream();
            await context.Request.Body.CopyToAsync(requestStream);
            if (context.Response.StatusCode == 200)
            {
                string headerContents = "";
                foreach (var header in context.Request.Headers)
                {
                    headerContents += header.Key + " : " + header.Value + Environment.NewLine;
                }
                logger.Info($"Http Request Information:{Environment.NewLine}" +
                                       $"Headers:{Environment.NewLine} {headerContents}" +
                                       $"ContentType:{context.Request.ContentType} {Environment.NewLine}" +
                                       $"Schema:{context.Request.Scheme} " +
                                       $"Host: {context.Request.Host} " +
                                       $"Path: {context.Request.Path} " +
                                       $"QueryString: {context.Request.QueryString} " +
                                       $"Request Body: {ReadStreamInChunks(requestStream)}");

            }
            context.Request.Body.Position = 0;
        }

        private static string ReadStreamInChunks(Stream stream)
        {
            const int readChunkBufferLength = 4096;

            stream.Seek(0, SeekOrigin.Begin);

            using var textWriter = new StringWriter();
            using var reader = new StreamReader(stream);

            var readChunk = new char[readChunkBufferLength];
            int readChunkLength;

            do
            {
                readChunkLength = reader.ReadBlock(readChunk, 0, readChunkBufferLength);
                textWriter.Write(readChunk, 0, readChunkLength);
            } while (readChunkLength > 0);

            return textWriter.ToString();
        }

        private async Task LogResponse(HttpContext context)
        {
            try
            {
                var originalBodyStream = context.Response.Body;

                await using var responseBody = _recyclableMemoryStreamManager.GetStream();
                context.Response.Body = responseBody;

                await _next(context);

                context.Response.Body.Seek(0, SeekOrigin.Begin);
                var text = await new StreamReader(context.Response.Body).ReadToEndAsync();
                context.Response.Body.Seek(0, SeekOrigin.Begin);

                await responseBody.CopyToAsync(originalBodyStream);
            }
            catch (Exception ex)
            {


                //_logger.WriteErrorLog($"Http Response Information:{Environment.NewLine}" +
                //                      $"Schema:{context.Request.Scheme} " +
                //                      $"Host: {context.Request.Host} " +
                //                      $"Path: {context.Request.Path} " +
                //                      $"QueryString: {context.Request.QueryString} " +
                //                      $"Response Error Code: {context.Response.StatusCode}" +
                //                      $"{Environment.NewLine} " +
                //                      $"Exception:{ex.Message.ToString() + " " + ex.StackTrace.ToString()}");


                //context.Response.Redirect("/Home/Error?host=" + System.Net.WebUtility.UrlEncode(context.Request.Host.ToString()) + "&path=" + System.Net.WebUtility.UrlEncode(context.Request.Path.ToString())
                //     + "&exmsg=" + System.Net.WebUtility.UrlEncode(ex.Message.ToString()) + "&stacktrace=" + System.Net.WebUtility.UrlEncode(ex.StackTrace.ToString()));
                // context.Response.Redirect("/Home/Error?except=" + System.Net.WebUtility.UrlEncode(context.Request.Path + "?" + context.Request.QueryString));             
                //context.Response.Redirect("/Home/Error");


            }
        }
    }
}
