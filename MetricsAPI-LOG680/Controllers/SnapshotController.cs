using GraphQL.Client.Http;
using MetricsAPI_LOG680.DTO;
using MetricsAPI_LOG680.Helpers;
using MetricsAPI_LOG680.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace MetricsAPI_LOG680.Controllers;

[ApiController]
[Route("[controller]")]
public class SnapshotController : ControllerBase
{
    private readonly ILogger<TestController> _logger;
    private readonly ApiDbContext _dbContext;
    private readonly IGraphQLHelper _graphQlHelper;
    private readonly ISnapshotService _snapshotService;
    
    private const string BACKLOG = "Backlog";
    private const string A_FAIRE = "À faire";
    private const string EN_COURS = "En cours";
    private const string REVUE = "Revue";
    private const string TERMINEE = "Terminée";

    private int _backlogCmpt;
    private int _aFaireCmpt;
    private int _enCoursCmpt;
    private int _revueCmpt;
    private int _termineeCmpt;

    public SnapshotController(ILogger<TestController> logger, ApiDbContext dbContext, IGraphQLHelper graphQlHelper, ISnapshotService snapshotService)
    {
        _logger = logger;
        _dbContext = dbContext;
        _graphQlHelper = graphQlHelper;
        _snapshotService = snapshotService;
    }
    
    [HttpPost(Name = "PostSnapshot")]
    public async Task<ActionResult> PostSnapshot(string? token, string? repository, string? owner, string? projectId)
    {
        var graphQLClient = _graphQlHelper.GetClient(token);
        bool isProjectId = false;

        JToken? projectsNode;

        if (projectId != null)
        {
            projectsNode = await QueryByProjectId(projectId, graphQLClient);
            isProjectId = true;
        }
        else if (repository != null && owner != null)
        {
            projectsNode = await QueryByRepoAndOwner(repository, owner, graphQLClient);
        }
        else
        {
            return BadRequest("Missing required parameters");
        }

        var title = projectsNode["title"].ToString();
        projectId = projectsNode["id"].Value<string>();
        
        foreach (var item in projectsNode["items"]["nodes"])
        {
            var columnName = item["fieldValues"]["nodes"].Last["name"].Value<string>();

            switch (columnName)
            {
                case BACKLOG:
                    _backlogCmpt++;
                    break;
                case A_FAIRE:
                    _aFaireCmpt++;
                    break;
                case EN_COURS:
                    _enCoursCmpt++;
                    break;
                case REVUE:
                    _revueCmpt++;
                    break;
                case TERMINEE:
                    _termineeCmpt++;
                    break;
                default:
                    throw new Exception("Column Type was not found");
            }
        }

        var snapshot = _snapshotService.CreateSnapshot(_backlogCmpt, _aFaireCmpt,
                                                    _enCoursCmpt, _revueCmpt,
                                                    _termineeCmpt, DateTime.UtcNow);

        if (!isProjectId)
        {
            snapshot.Repository_name = repository;
            snapshot.Owner = owner;
        }

        snapshot.Project_id = projectId;
        snapshot.Title = title;
            
        await _dbContext.Snapshots.AddAsync(snapshot);
        await _dbContext.SaveChangesAsync();
        
        return Ok();
    }

    private async Task<JToken?> QueryByProjectId(string projectId, GraphQLHttpClient graphQLClient)
    {
        var graphQLRequest = new GraphQLHttpRequest
        {
            Query = @"
                query($projectId: ID!) {
                  node(id: $projectId) {
                    ... on ProjectV2 {
                      title
                      items(first: 100) {
                        totalCount
                        nodes {
                          fieldValues(first: 100) {
                            nodes {
                              ... on ProjectV2ItemFieldSingleSelectValue {
                                name
                              }
                            }
                          }
                        }
                      }
                    }
                  }
                }",
            Variables = new
            {
                projectId
            }
        };

        try
        {
            var graphQLResponse = await graphQLClient.SendQueryAsync<JObject>(graphQLRequest);
                
            var projectsNode = graphQLResponse.Data["node"];
            
            return projectsNode;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return null;
    }

    private async Task<JToken?> QueryByRepoAndOwner(string repo, string owner, GraphQLHttpClient graphQLClient)
    {
        var graphQLRequest = new GraphQLHttpRequest
        {
            Query = @"
                query ($repo: String!, $owner: String!) {
                  repository(name: $repo, owner: $owner){
                    projectsV2(last:1){
                      nodes{
                        ... on ProjectV2 {
                          id
                          title
                          items(first: 100) {
                            totalCount
                            nodes {
                              fieldValues(first: 100) {
                                nodes {
                                  ... on ProjectV2ItemFieldSingleSelectValue {
                                    name
                                  }
                                }
                              }
                            }
                          }
                        }
                      }
                    }
                  }
                }
            ",
            Variables = new
            {
                repo,
                owner
            }
        };

        try
        {
            var graphQLResponse = await graphQLClient.SendQueryAsync<JObject>(graphQLRequest);
                
            var projectsNode = graphQLResponse.Data["repository"]["projectsV2"]["nodes"].First;

            return projectsNode;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return null;
    }
    
    [HttpGet("GetSnapshotOnDate")]
    public async Task<ActionResult> GetSnapshotOnDate([FromQuery] DateTime startDate,
                                                    string? owner, string? repository, string? projectId)
    {
        var endDate = startDate.AddDays(1);
        var snapshots = await GetSnapshotsByDates(startDate, endDate, owner, repository, projectId);
        
        return Ok(snapshots);
    }
    
    [HttpGet("GetProjectTasksMeanBetweenTwoDates")]
    public async Task<ActionResult> GetProjectTasksMeanBetweenTwoDates(
                            [FromQuery] DateTime startDate, [FromQuery] DateTime endDate,
                            string? owner, string? repository, string? projectId)
    {
        var snapshots = await GetSnapshotsByDates(startDate, endDate, owner, repository, projectId);

        int moy = 0;
        foreach (var snapshot in snapshots)
        {
            moy += snapshot.Total_items;
        }
        
        return Ok($"Moyenne de issues entre {startDate} et {endDate} : {moy / snapshots.Count()} issues");
    }

    private async Task<IEnumerable<Snapshot>> GetSnapshotsByDates(DateTime startDate, DateTime endDate,
                                            string? owner, string? repository, string? projectId)
    {
        if (projectId != null)
        {
            return await _dbContext.Snapshots
                .Where(s => s.Project_id == projectId 
                        && s.Timestamps >= startDate.ToUniversalTime()
                        && s.Timestamps <= endDate.ToUniversalTime())
                .ToListAsync();
        }
        if (repository != null && owner != null)
        {
            return await _dbContext.Snapshots
                .Where(s => s.Repository_name == repository && s.Owner == owner
                        && s.Timestamps >= startDate.ToUniversalTime()
                        && s.Timestamps <= endDate.ToUniversalTime())
                .ToListAsync();
        }
        return await _dbContext.Snapshots
                                .Where(s => s.Timestamps >= startDate.ToUniversalTime()
                                            && s.Timestamps <= endDate.ToUniversalTime())
                                .ToListAsync();
    }
}