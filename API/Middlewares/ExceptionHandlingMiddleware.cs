using System;
using System.Net;
using System.Threading.Tasks;
using API.Models;
using Core.Exceptions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;

namespace API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        [UsedImplicitly]
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch (Exception e)
            {
                await HandleExceptionAsync(httpContext, e);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; // By default

            // We might want to change standard messages for some exceptions
            var errorResponseModel = new ErrorResponseModel
            {
                Success = false
            };

            switch (exception)
            {
                case UserAlreadyExistsException _:
                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                    errorResponseModel.Message = exception.Message;
                    break;
                case UserNotFoundException _:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    errorResponseModel.Message = exception.Message;
                    break;
                case AuthenticationException _:
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    errorResponseModel.Message = exception.Message;
                    break;
                default:
                    errorResponseModel.Message = "Internal Server Error";
                    break;
            }

            await context.Response.WriteAsync(errorResponseModel.ToString());
        }
    }
}