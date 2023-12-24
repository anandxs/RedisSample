using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using RedisSample.Api.Extensions;

namespace RedisSample.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IDistributedCache _cache;

        public ValuesController(IDistributedCache cache)
        {
            _cache = cache;
        }

        [HttpGet]
        public async Task<IActionResult> GetValues()
        {
            var recordKey = $"Values: {DateTime.Now.ToString("yyyymmdd_hhmm")}";
            var values = await _cache.GetRecordAsync< List<string>>(recordKey);

            if (values == null)
            {
	            await Task.Delay(2000);
                var dbValues = new List<string>()
                {
                    "Anand", "23", "Jason", "Redis", "Docker", "SignalR"
                };

                await _cache.SetRecordAsync(recordKey, dbValues);

                return Ok(dbValues);
            }

            return Ok(values);
        }
    }
}
