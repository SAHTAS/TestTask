using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace API.Middlewares
{
    public class ContentTypeSettingMiddleware
    {
        private readonly RequestDelegate next;

        public ContentTypeSettingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        [UsedImplicitly]
        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.ContainsKey(HeaderNames.ContentType))
                context.Request.Headers.Add(HeaderNames.ContentType, "application/json; charset=utf-8");
            await next(context);
        }
    }
}