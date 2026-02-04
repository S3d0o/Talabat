using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace Presentation.Attributes
{
    internal class RedisCacheAttribute(int durationsInSeconds = 120) : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!HttpMethods.IsGet(context.HttpContext.Request.Method))
            {
                await next();
                return;
            }
            var casheService = context.HttpContext.RequestServices.GetRequiredService<IServiceManager>().CasheService;
            string key = GenerateKey(context.HttpContext.Request);
            var result = await casheService.GetCacheValueAsync(key);
            if (result != null)
            {
                context.Result = new ContentResult
                {
                    Content = result,
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK
                };
                return;
            }
            // Execute controller action
            var executedContext = await next();
            if (executedContext.Result is ObjectResult objectResult)
            {
                await casheService.SetCacheValueAsync(key, objectResult.Value!, TimeSpan.FromSeconds(durationsInSeconds));
            }
        }

        private string GenerateKey(HttpRequest request)
        {
            var key = new StringBuilder();
            key.Append(request.Path);
            foreach (var item in request.Query.OrderBy(x => x.Key))
            {
                key.Append($"{item.Key}-{item.Value}");
            }
            return key.ToString();
        }
    }
}
