using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using TodoListManagement.Business.Exceptions;

namespace TodoListManagement.Api.Middlewares
{
    public static class GlobalValidationMiddleware
    {
        public static IApplicationBuilder UseGlobalValidationMiddleware(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                try
                {
                    await next();
                }
                catch (NotFoundException ex)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    await context.Response.WriteAsJsonAsync(new { error = ex.Message });
                }
                catch (ArgumentException ae)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    await context.Response.WriteAsJsonAsync(new { error = ae.Message });
                }
            });

            return app;
        }
    }
}
