using Microsoft.AspNetCore.Mvc;
using ProductManagement.API.Patterns;

namespace ProductManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SystemController : ControllerBase
    {
        // GET: api/system/stats
        [HttpGet("stats")]
        public IActionResult GetStats()
        {
            var stats = ApiStatistics.Instance;     // get THE one instance,get the singleton

            return Ok(new
            {
                startedAt = stats.StartedAt,
                totalRequests = stats.TotalRequests,
                uptime = (DateTime.Now - stats.StartedAt).ToString(@"hh\:mm\:ss"),
                instancesCreated = ApiStatistics.InstancesCreated     // should always be 1
            });
        }

        // GET: api/system/same-instance
        [HttpGet("same-instance")]
        public IActionResult CheckSameInstance()
        {
            var a = ApiStatistics.Instance;
            var b = ApiStatistics.Instance;

            return Ok(new
            {
                areTheSameObject = ReferenceEquals(a, b)     // true = literally the same object in memory
            });
        }
    }
}