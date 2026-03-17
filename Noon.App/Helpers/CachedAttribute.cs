using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Noon.Core.Services;
using System.Security.Cryptography.Xml;
using System.Text;

namespace Noon.App.Helpers
{
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLiveSecond;

        public CachedAttribute(int timeToLiveSecond)
        {
            _timeToLiveSecond = timeToLiveSecond;
        }

        

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();
            var cacheKey=GenarateCacheKeyFromRequest(context.HttpContext.Request);
            var cachedResponse =  await cacheService.GetCacheResponseAsync(cacheKey);
            if(!string.IsNullOrEmpty(cachedResponse))
            {
                var contentResult= new ContentResult()
                { Content = cachedResponse ,
                 ContentType = "application/json",
                 StatusCode = 200
                };
                context.Result= contentResult;  
                return;
            }

           var executedEndpointContext= await next();
            if(executedEndpointContext.Result is OkObjectResult okObjectResult)
            {
                await cacheService.CacheResponseAsync(cacheKey,okObjectResult.Value,TimeSpan.FromSeconds(_timeToLiveSecond));
            }


        }

        private string GenarateCacheKeyFromRequest(HttpRequest request)
        {

            var keyBuilder = new StringBuilder();
            keyBuilder.Append(request.Path);

            foreach (var (key, value) in request.Query.OrderBy(O=>O.Key))
            {
                keyBuilder.Append($"|{key}-{value}");
            }
            return keyBuilder.ToString();
        }
    }
}
