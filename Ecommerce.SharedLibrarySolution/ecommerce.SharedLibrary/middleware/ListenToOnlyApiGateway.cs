using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecommerce.SharedLibrary.middleware
{
    public class ListenToOnlyApiGateway(RequestDelegate _next)
    {
       
        public async Task Invoke(HttpContext context)
        {
            Console.WriteLine("🔥 ListenToOnlyApiGateway middleware triggered");

            var signedHeader = context.Request.Headers["Api-Gateway"];
            Console.WriteLine($"Header: {signedHeader}");

            if (!context.Request.Headers.ContainsKey("Api-Gateway"))
            {
                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                await context.Response.WriteAsync("Sorry, service is unavailable");
                return;
            }


            await _next(context);
        }

    }
}
