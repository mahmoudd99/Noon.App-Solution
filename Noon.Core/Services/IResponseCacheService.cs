using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noon.Core.Services
{
    public interface IResponseCacheService
    {
        Task CacheResponseAsync(string cacheKey, object responce, TimeSpan timeToLive);
        Task<string> GetCacheResponseAsync(string cacheKey);


    }
}
