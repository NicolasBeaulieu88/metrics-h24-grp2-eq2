using GraphQL.Client.Http;
using MetricsAPI_LOG680.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace MetricsAPI_LOG680.Controllers;

[ApiController]
[Route("[controller]")]
public class PRController : ControllerBase
{
    private readonly ILogger<PRController> _logger;
    private readonly ApiDbContext _dbContext;
    private readonly IGraphQLHelper _graphQlHelper;

    public PRController(ILogger<PRController> logger, ApiDbContext dbContext, IGraphQLHelper graphQlHelper)
    {
        _logger = logger;
        _dbContext = dbContext;
        _graphQlHelper = graphQlHelper;
    }

    [HttpGet("GetPRIds")]
    public async Task<IActionResult> GetPRIds([FromQuery]GithubToken githubToken)
    {
        var graphQLSettings = _graphQlHelper.GetGraphQLSettings();
        var graphQLClient = _graphQlHelper.GetClient(githubToken.token);
        
        var projectId = graphQLSettings.GetSection("projectId").Value;

        var graphQLRequest = new GraphQLHttpRequest
        {
            Query = @"
                query {
                    repository(owner: ""NicolasBeaulieu88"", name: ""metrics-h24-grp2-eq2"") {
                        pullRequests(first: 100) {
                            nodes {
                                number
                            }
                        }
                    }
                }"
        };
        try
        {
            var graphQLResponse = await graphQLClient.SendQueryAsync<dynamic>(graphQLRequest);

            var simplifiedJson = graphQLResponse.Data.ToString();

            return Ok(simplifiedJson);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("GetPRLeadTime")]
    public async Task<IActionResult> GetPRLeadTime([FromQuery]GithubToken githubToken, [FromQuery]GraphQLPullRequest pr){

        var graphQLSettings = _graphQlHelper.GetGraphQLSettings();
        var graphQLClient = _graphQlHelper.GetClient(githubToken.token);
        
        var projectId = graphQLSettings.GetSection("projectId").Value;

        var graphQLRequest = new GraphQLHttpRequest
        {
            Query = @"
                query {
                    repository(owner: ""NicolasBeaulieu88"", name: ""metrics-h24-grp2-eq2"") {
                        pullRequest(number: " + pr.number + @") {
                            number
                            createdAt
                            closedAt
                        }
                    }
                }"
        };
        try
        {
            var graphQLResponse = await graphQLClient.SendQueryAsync<dynamic>(graphQLRequest);
            

            var pullRequest = graphQLResponse.Data["repository"]["pullRequest"];

            DateTime createdAt = DateTime.Parse(pullRequest["createdAt"].ToString());
            DateTime closedAt = DateTime.Parse(pullRequest["closedAt"].ToString());

            var leadtime = closedAt - createdAt;
            
            return Ok(leadtime.Days + " days " + leadtime.Hours + " hours");

        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

    }

    
}

public class GithubToken
{
    public string? token { get; set; }
}
public class GraphQLPullRequest
{
    public string? number { get; set; }
}


