using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using MetricsAPI_LOG680.DTO;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
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
        
        [HttpGet("TestOctokit")]
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

        [HttpGet("TestGraphQL")]
        public async Task<ActionResult> GetTestGraphQL(string? owner, string? repository, string? token)
        {
            // Replace these values with your GitHub username, repository name, and personal access token
            owner ??= "NicolasBeaulieu88";
            repository ??= "metrics-h24-grp2-eq2";
            token ??= "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";

            var address = "https://api.github.com/graphql";

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(address),
                DefaultRequestHeaders =
                {
                    UserAgent =
                    {
                        new System.Net.Http.Headers.ProductInfoHeaderValue("YourApp", "1.0")
                    },
                    Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token)
                }
            };

            var options = new GraphQLHttpClientOptions
            {
                EndPoint = new Uri(address)
            };

            var graphQLClient = new GraphQLHttpClient(options, new NewtonsoftJsonSerializer(), httpClient);

            var graphQLRequest = new GraphQLHttpRequest
            {
                Query = @"
                    query{
                        repository(name: """ + repository + @""", owner: """ + owner + @"""){
                            id
                            name
                            projectsV2(first: 10) {
                                totalCount
                                nodes {
                                    id
                                    title
                                }
                            }
                        }
                    }"
            };

            try
            {
                var graphQLResponse = await graphQLClient.SendQueryAsync<JObject>(graphQLRequest);
                
                var repositoryNode = graphQLResponse.Data["repository"];
                var repositoryName = repositoryNode["name"].Value<string>();
                
                try
                {
                    var projectsNode = repositoryNode["projectsV2"]["nodes"];
                    var project = projectsNode.First;
                    var projectId = project["id"].Value<string>();
                    var projectName = project["title"].Value<string>();
                    
                    Console.WriteLine($"Project: {projectName}");
                    Console.WriteLine($"Id: {projectId}");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                Console.WriteLine($"Repository: {repositoryName}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return Ok();
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
