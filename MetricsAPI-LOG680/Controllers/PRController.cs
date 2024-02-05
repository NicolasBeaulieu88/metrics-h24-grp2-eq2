using System.Runtime.InteropServices;
using GraphQL.Client.Http;
using MetricsAPI_LOG680.Helpers;
using MetricsAPI_LOG680.Services;
using Microsoft.AspNetCore.Mvc;

namespace MetricsAPI_LOG680.Controllers;

[ApiController]
[Route("[controller]")]
public class PRController : ControllerBase
{
    private readonly ILogger<PRController> _logger;
    private readonly ApiDbContext _dbContext;
    private readonly IGraphQLHelper _graphQlHelper;
    private readonly IPRMetricsService _prMetricsService;


    public PRController(ILogger<PRController> logger, ApiDbContext dbContext, IGraphQLHelper graphQlHelper, IPRMetricsService prMetricsService)
    {
        _logger = logger;
        _dbContext = dbContext;
        _graphQlHelper = graphQlHelper;
        _prMetricsService = prMetricsService;
    }

    [HttpGet("GetPRIds")]
    public async Task<IActionResult> GetPRIds([FromQuery]GithubAuth githubToken)
    {
        var graphQLSettings = _graphQlHelper.GetGraphQLSettings();
        var graphQLClient = _graphQlHelper.GetClient(githubToken.token);
        
        var username = githubToken.username ?? graphQLSettings.GetSection("username").Value;
        var repository = githubToken.repository ?? graphQLSettings.GetSection("repository").Value;
        
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
    public async Task<IActionResult> GetPRLeadTime([FromQuery]GithubAuth githubToken, [FromQuery]GraphQLPullRequest pr){

        var graphQLSettings = _graphQlHelper.GetGraphQLSettings();
        var graphQLClient = _graphQlHelper.GetClient(githubToken.token);
        
        var username = githubToken.username ?? graphQLSettings.GetSection("username").Value;
        var repository = githubToken.repository ?? graphQLSettings.GetSection("repository").Value;

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

            var prLeadtime = _prMetricsService.CreatePRLeadTime(pr.number, username, repository, DateTime.Now, createdAt, closedAt, leadTime);
            await _dbContext.PRLeadTimes.AddAsync(prLeadtime);
            await _dbContext.SaveChangesAsync();

            return Ok(leadTime.Days + " days " + leadTime.Hours + " hours");

        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

    }

    [HttpGet("GetPRMergedTime")]
    public async Task<IActionResult> GetPRMergedTime([FromQuery]GithubAuth githubToken, [FromQuery]GraphQLPullRequest pr){

        var graphQLSettings = _graphQlHelper.GetGraphQLSettings();
        var graphQLClient = _graphQlHelper.GetClient(githubToken.token);
        
        var username = githubToken.username ?? graphQLSettings.GetSection("username").Value;
        var repository = githubToken.repository ?? graphQLSettings.GetSection("repository").Value;

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

            var prMergedTime = _prMetricsService.CreatePRMergedTime(pr.number, username, repository, DateTime.Now, mergedAt, mergedTime);
            await _dbContext.PRMergedTimes.AddAsync(prMergedTime);
            await _dbContext.SaveChangesAsync();

            return Ok(mergedTime.Days + " days " + mergedTime.Hours + " hours");

        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

    }

    [HttpGet("GetPRSize")]
    public async Task<IActionResult> GetPRSize([FromQuery]GithubAuth githubToken, [FromQuery]GraphQLPullRequest pr){

        var graphQLSettings = _graphQlHelper.GetGraphQLSettings();
        var graphQLClient = _graphQlHelper.GetClient(githubToken.token);
        
        var username = githubToken.username ?? graphQLSettings.GetSection("username").Value;
        var repository = githubToken.repository ?? graphQLSettings.GetSection("repository").Value;

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

            var prSize = _prMetricsService.CreatePRSize(pr.number, username, repository, DateTime.Now, additions, deletions, additions + deletions);
            await _dbContext.PRSizes.AddAsync(prSize);
            await _dbContext.SaveChangesAsync();

            return Ok(additions + deletions + " lines changed");

        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("GetPRFlowRatio")] // PRFlowRatio = sum of opened pull requests over the sum of closed pull requests over the last 30 days
    public async Task<IActionResult> GetPRFlowRatio([FromQuery]GithubAuth githubToken){

        var graphQLSettings = _graphQlHelper.GetGraphQLSettings();
        var graphQLClient = _graphQlHelper.GetClient(githubToken.token);
        
        var username = githubToken.username ?? graphQLSettings.GetSection("username").Value;
        var repository = githubToken.repository ?? graphQLSettings.GetSection("repository").Value;

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

            var flowRatio = (double)openedPR/(double)closedPR;

            var prFlowRatio = _prMetricsService.CreatePRFlowRatio(username, repository, DateTime.Now, openedPR, closedPR, flowRatio);
            await _dbContext.PRFlowRatios.AddAsync(prFlowRatio);
            await _dbContext.SaveChangesAsync();

            return Ok("Flow ratio: " + openedPR + " opened PRs / " + closedPR + " closed PRs, or "  + flowRatio);

        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("GetPRDiscussions")] // PRDiscussions: the sum of comments, reviews and review requests for a given PR
    public async Task<IActionResult> GetPRDiscussions([FromQuery]GithubAuth githubToken, [FromQuery]GraphQLPullRequest pr){

        var graphQLSettings = _graphQlHelper.GetGraphQLSettings();
        var graphQLClient = _graphQlHelper.GetClient(githubToken.token);
        
        var username = githubToken.username ?? graphQLSettings.GetSection("username").Value;
        var repository = githubToken.repository ?? graphQLSettings.GetSection("repository").Value;

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

            var prDiscussions = _prMetricsService.CreatePRDiscussions(pr.number, username, repository, DateTime.Now, comments, reviews, reviewRequests, comments + reviews + reviewRequests);
            await _dbContext.PRDiscussions.AddAsync(prDiscussions);
            await _dbContext.SaveChangesAsync();

            return Ok("Discussion: " + comments + " comments, " + reviews + " reviews, " + reviewRequests + " review requests, \nTotal: " + (comments + reviews + reviewRequests) + " interactions");

        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    

    
}


public class GithubAuth
{
    public string? token { get; set; }
    public string? username { get; set; }
    public string? repository { get; set; }
}
public class GraphQLPullRequest
{
    public string? number { get; set; }
}


