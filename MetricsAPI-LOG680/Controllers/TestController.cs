using Microsoft.AspNetCore.Mvc;

namespace MetricsAPI_LOG680.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetTest")]
        public Task<ActionResult> Get()
        {
            return Task.FromResult<ActionResult>(Ok(new{something = "hell yeah"}));
        }
    }
}
