using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Asn1.Ocsp;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DapperApiWithAuth.Middlewares
{
    public class ScriptTagBlockerMiddleware
    {
        // _next ek delegate hai jo agla middleware pipeline me call karta hai
        private readonly RequestDelegate _next;

        public ScriptTagBlockerMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        // Ye middleware ka core method hai jo har request ke liye call hota hai.
        //HttpContext me request aur response ka sara data hota hai.
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Method.Equals("POST", StringComparison.OrdinalIgnoreCase) ||
                context.Request.Method.Equals("PUT", StringComparison.OrdinalIgnoreCase))
            {
                //EnableBuffering() se hum body ko seekable (repeatable read) bana dete hain.
                context.Request.EnableBuffering();

                using var reader = new StreamReader(
                    context.Request.Body,
                    encoding: Encoding.UTF8,
                    detectEncodingFromByteOrderMarks: false,
                    bufferSize: 1024,
                    leaveOpen: true
                );

                string body = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;

                if (body.Contains("<script>", StringComparison.OrdinalIgnoreCase) ||
                    body.Contains("</script>", StringComparison.OrdinalIgnoreCase))
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync("❌ Request blocked: <script> tag is not allowed.");
                    return;
                }
            }

            await _next(context);
        }
    }
}
