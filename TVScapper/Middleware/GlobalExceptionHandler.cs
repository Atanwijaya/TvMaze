using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TVScapper.Exceptions;

namespace TVScapper.Middleware
{
    public class GlobalExceptionHandler
    {
        private readonly RequestDelegate _next;

        public GlobalExceptionHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {

                int statusCode = 0;
                bool useCustomExceptionMessage = false;

                if (error is BadHttpRequestException)
                {
                    statusCode = (int)HttpStatusCode.BadRequest;
                    useCustomExceptionMessage = true;
                }
                else if (error is NotFoundException)
                {
                    statusCode = (int)HttpStatusCode.NotFound;
                    useCustomExceptionMessage = true;
                }

                //and many more

                if (useCustomExceptionMessage)
                {
                    var response = context.Response;
                    response.ContentType = "application/json";
                    response.StatusCode = statusCode;

                    await response.WriteAsync(error.Message.ToString());
                }
            }
        }
    }
}
