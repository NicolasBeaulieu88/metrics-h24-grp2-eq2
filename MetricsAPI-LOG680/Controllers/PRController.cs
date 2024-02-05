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
        

        var username = graphQLSettings.GetSection("username").Value;
        var repository = graphQLSettings.GetSection("repository").Value;
        
        var graphQLRequest = new GraphQLHttpRequest
        {
            Query = @"
                query {
                    repository(owner: """+ username + @""", name: """ + repository + @""") {
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
        
        var username = graphQLSettings.GetSection("username").Value;
        var repository = graphQLSettings.GetSection("repository").Value;

        var graphQLRequest = new GraphQLHttpRequest
        {
            Query = @"
                query {
                    repository(owner: """+ username + @""", name: """ + repository + @""") {
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

            var leadTime = closedAt - createdAt;

            return Ok(leadTime.Days + " days " + leadTime.Hours + " hours");

        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

    }

    [HttpGet("GetPRMergeTime")]
    public async Task<IActionResult> GetPRMergeTime([FromQuery]GithubToken githubToken, [FromQuery]GraphQLPullRequest pr){

        var graphQLSettings = _graphQlHelper.GetGraphQLSettings();
        var graphQLClient = _graphQlHelper.GetClient(githubToken.token);
        
        var username = graphQLSettings.GetSection("username").Value;
        var repository = graphQLSettings.GetSection("repository").Value;

        var graphQLRequest = new GraphQLHttpRequest
        {
            Query = @"
                query {
                    repository(owner: """+ username + @""", name: """ + repository + @""") {
                        pullRequest(number: " + pr.number + @") {
                            number
                            createdAt
                            mergedAt
                        }
                    }
                }"
        };
        try
        {
            var graphQLResponse = await graphQLClient.SendQueryAsync<dynamic>(graphQLRequest);
            

            var pullRequest = graphQLResponse.Data["repository"]["pullRequest"];

            DateTime createdAt = DateTime.Parse(pullRequest["createdAt"].ToString());
            DateTime mergedAt = DateTime.Parse(pullRequest["mergedAt"].ToString());

            var mergedTime = mergedAt - createdAt;

            return Ok(mergedTime.Days + " days " + mergedTime.Hours + " hours");

        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

    }

    [HttpGet("GetPRSize")]
    public async Task<IActionResult> GetPRSize([FromQuery]GithubToken githubToken, [FromQuery]GraphQLPullRequest pr){

        var graphQLSettings = _graphQlHelper.GetGraphQLSettings();
        var graphQLClient = _graphQlHelper.GetClient(githubToken.token);
        
        var username = graphQLSettings.GetSection("username").Value;
        var repository = graphQLSettings.GetSection("repository").Value;

        var graphQLRequest = new GraphQLHttpRequest
        {
            Query = @"
                query {
                    repository(owner: """+ username + @""", name: """ + repository + @""") {
                        pullRequest(number: " + pr.number + @") {
                            number
                            additions
                            deletions
                        }
                    }
                }"
        };
        try
        {
            var graphQLResponse = await graphQLClient.SendQueryAsync<dynamic>(graphQLRequest);
            

            var pullRequest = graphQLResponse.Data["repository"]["pullRequest"];

            int additions = int.Parse(pullRequest["additions"].ToString());
            int deletions = int.Parse(pullRequest["deletions"].ToString());

            return Ok(additions + deletions + " lines changed");

        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("GetPRFlowRatio")] // PRFlowRatio = sum of opened pull requests over the sum of closed pull requests over the last 30 days
    public async Task<IActionResult> GetPRFlowRatio([FromQuery]GithubToken githubToken){

        var graphQLSettings = _graphQlHelper.GetGraphQLSettings();
        var graphQLClient = _graphQlHelper.GetClient(githubToken.token);
        
        var username = graphQLSettings.GetSection("username").Value;
        var repository = graphQLSettings.GetSection("repository").Value;

        var graphQLRequest = new GraphQLHttpRequest
        {
            Query = @"
                query {
                    repository(owner: """+ username + @""", name: """ + repository + @""") {
                        pullRequests(first: 100) {
                            nodes {
                                number
                                createdAt
                                closedAt
                            }
                        }
                    }
                }"
        };
        try
        {
            var graphQLResponse = await graphQLClient.SendQueryAsync<dynamic>(graphQLRequest);
            
            var pullRequests = graphQLResponse.Data["repository"]["pullRequests"]["nodes"];

            int openedPR = 0;
            int closedPR = 0;

            foreach (var pr in pullRequests)
            {
                DateTime createdAt = DateTime.Parse(pr["createdAt"].ToString());
                DateTime closedAt = DateTime.Now;

                if (createdAt > DateTime.Now.AddDays(-30))
                {
                    openedPR++;
                    if (!string.IsNullOrEmpty(pr["closedAt"]?.ToString()))
                    {
                        closedPR++;
                    }
                }

            }
            return Ok("Flow ratio: " + openedPR + " opened PRs / " + closedPR + " closed PRs, or "  + ((double)openedPR/(double)closedPR));

        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("GetPRDiscussion")] // PRDiscussion the sum of comments, reviews and review requests for a given PR
    public async Task<IActionResult> GetPRDiscussion([FromQuery]GithubToken githubToken, [FromQuery]GraphQLPullRequest pr){

        var graphQLSettings = _graphQlHelper.GetGraphQLSettings();
        var graphQLClient = _graphQlHelper.GetClient(githubToken.token);
        
        var username = graphQLSettings.GetSection("username").Value;
        var repository = graphQLSettings.GetSection("repository").Value;

        var graphQLRequest = new GraphQLHttpRequest
        {
            Query = @"
                query {
                    repository(owner: """+ username + @""", name: """ + repository + @""") {
                        pullRequest(number: " + pr.number + @") {
                            number
                            comments {
                                totalCount
                            }
                            reviews {
                                totalCount
                            }
                            reviewRequests {
                                totalCount
                            }
                        }
                    }
                }"
        };
        try
        {
            var graphQLResponse = await graphQLClient.SendQueryAsync<dynamic>(graphQLRequest);
            

            var pullRequest = graphQLResponse.Data["repository"]["pullRequest"];

            int comments = int.Parse(pullRequest["comments"]["totalCount"].ToString());
            int reviews = int.Parse(pullRequest["reviews"]["totalCount"].ToString());
            int reviewRequests = int.Parse(pullRequest["reviewRequests"]["totalCount"].ToString());

            return Ok("Discussion: " + comments + " comments, " + reviews + " reviews, " + reviewRequests + " review requests, \nTotal: " + (comments + reviews + reviewRequests) + " interactions");

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


