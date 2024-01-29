using MetricsAPI_LOG680.DTO;
using Microsoft.AspNetCore.Mvc;
using Octokit;

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

        [HttpGet("GetTest")]
        public Task<ActionResult> GetTest()
        {
            return Task.FromResult<ActionResult>(Ok(new{something = "hell yeah"}));
        }
        
        [HttpGet("TestGitHubAPI")]
        public async Task<ActionResult> GetTestGitHubAPI(string? username, string? repository, string? token)
        {
            // Replace these values with your GitHub username, repository name, and personal access token
            username ??= "NicolasBeaulieu88";
            repository ??= "metrics-h24-grp2-eq2";
            token ??= "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";

            var client = new GitHubClient(new ProductHeaderValue("MyApp"));

            // Set the access token for authentication
            var credentials = new Credentials(token);
            client.Credentials = credentials;

            string msg = "";
            
            try
            {
                // Get information about the repository
                var repositoryInfo = await client.Repository.Get(username, repository);

                msg = $"Repository: {repositoryInfo.Name}";
                msg += $"Description: {repositoryInfo.Description}";
                msg += $"Default Branch: {repositoryInfo.DefaultBranch}";
                msg += $"Stars: {repositoryInfo.StargazersCount}";
                msg += $"Watchers: {repositoryInfo.WatchersCount}";
                msg += $"Forks: {repositoryInfo.ForksCount}";
            }
            catch (NotFoundException ex)
            {
                Console.WriteLine($"Repository not found: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            
            return await Task.FromResult<ActionResult>(Ok(msg));
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
