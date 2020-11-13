using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace OnlineStore.Api.ExceptionMiddleware
{
    public class ExceptionHandlingMiddleware
    {
        private RequestDelegate Next { get; }
        private IJsonHelper JsonHelper { get; }


        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            IJsonHelper jsonHelper)
        {
            Next = next;
            JsonHelper = jsonHelper;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await Next(context);
            }
            catch (Exception ex)
            {
                await SendResponse(context, (int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        private async Task SendResponse(HttpContext context, int? statuscode, object messages = null)
        {
            context.Response.Clear();
            context.Response.StatusCode = statuscode ?? (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(
                JsonHelper.Serialize(messages ?? new[] { "Internal Server Error" })?.ToString());
        }
    }
}