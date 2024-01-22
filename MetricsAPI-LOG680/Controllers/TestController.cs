using MetricsAPI_LOG680.DTO;
using Microsoft.AspNetCore.Mvc;

namespace MetricsAPI_LOG680.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;
        private readonly ApiDbContext _dbContext;

        public TestController(ILogger<TestController> logger, ApiDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [HttpGet(Name = "GetTests")]
        public IEnumerable<TodoItem> Get()
        {
            return _dbContext.TodoItems.ToList();
        }

        [HttpPost(Name = "PostTest")]
        public async Task<ActionResult> Insert(TodoItem item)
        {
            await _dbContext.TodoItems.AddAsync(item);
            await _dbContext.SaveChangesAsync();
            return await Task.FromResult<ActionResult>(Ok());
        }
    }
}
